using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Resources;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string xmlString = File.ReadAllText("../../../Datasets/sales.xml");

            using (CarDealerContext context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));

            var suppliersDto = (ImportSupplierDto[])serializer.Deserialize(new StringReader(inputXml));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplierDto in suppliersDto)
            {
                Supplier supplier = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = supplierDto.IsImporter
                };

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();


            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));

            var partsDto = (ImportPartDto[])serializer.Deserialize(new StringReader(inputXml));

            List<Part> parts = new List<Part>();

            foreach (var partDto in partsDto)
            {
                if (!context.Suppliers.Any(x => x.Id == partDto.SupplierId))
                {
                    continue;
                }

                Part part = new Part()
                {
                    Name = partDto.Name,
                    Price = partDto.Price,
                    Quantity = partDto.Quantity,
                    SupplierId = partDto.SupplierId
                };

                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));

            var carsDto = (ImportCarDto[])serializer.Deserialize(new StringReader(inputXml));

            //List<Car> cars = new List<Car>();

            foreach (var carDto in carsDto)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                int[] carsPartIds = carDto.Parts.Select(x => x.Id)
                    .Distinct()
                    .ToArray();

                context.Cars.Add(car);

                List<PartCar> partCars = new List<PartCar>();

                foreach (var carPartId in carsPartIds)
                {
                    if (context.Parts.Any(x => x.Id == carPartId))
                    {
                        PartCar partcar = new PartCar()
                        {
                            CarId = car.Id,
                            PartId = carPartId
                        };
                        partCars.Add(partcar);
                    }
                }
                context.PartCars.AddRange(partCars);
            }
            context.SaveChanges();

            return $"Successfully imported {context.Cars.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomerDto[])serializer.Deserialize(new StringReader(inputXml));

            List<Customer> customers = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                Customer customer = new Customer()
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = customerDto.IsYoungDriver
                };
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));

            var salesDto = (ImportSaleDto[])serializer.Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();

            foreach (var saleDto in salesDto)
            {
                if (!context.Cars.Any(x => x.Id == saleDto.CarId))
                {
                    continue;
                }

                Sale sale = new Sale()
                {
                    CarId = saleDto.CarId,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(d => d.TravelledDistance > 2000000)
                .Select(car => new ExportCarsWithDistanceDto
                {
                    Make = car.Make,
                    Model = car.Model,
                    TraveledDistance = car.TravelledDistance
                })
                .OrderBy(x => x.Make)
                .ThenBy(m => m.Model)
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[]), new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(car => new ExportCarsFromBmw()
                {
                    Id = car.Id,
                    Model = car.Model,
                    TraveledDistance = car.TravelledDistance
                })
                .OrderBy(m => m.Model)
                .ThenByDescending(td => td.TraveledDistance)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsFromBmw[]), new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(s => new ExportLocalSupplierDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportLocalSupplierDto[]), new XmlRootAttribute("suppliers"));

            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), suppliers, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(car => new ExportCarWithPartsDto()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TraveledDistance = car.TravelledDistance,
                    Parts = car.PartCars.Select(part => new ExportPartDto()
                        {
                            Name = part.Part.Name,
                            Price = part.Part.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .OrderByDescending(td => td.TraveledDistance)
                .ThenBy(m => m.Model)
                .Take(5)
                .ToArray();


            var serializer = new XmlSerializer(typeof(ExportCarWithPartsDto[]), new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(customer => new ExportCustomersWithSalesDto()
                {
                    FullName = customer.Name,
                    BoughtCars = customer.Sales.Count,
                    SpentMoney = customer.Sales.Sum(m => m.Car.PartCars.Sum(x => x.Part.Price))
                })
                .OrderByDescending(sm => sm.SpentMoney)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCustomersWithSalesDto[]), new XmlRootAttribute("customers"));

            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), customers, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(sale => new ExportSaleWithDiscountDto()
                {
                    Car = new ExportCarDto()
                    {
                        Make = sale.Car.Make,
                        Model = sale.Car.Model,
                        TraveledDistance = sale.Car.TravelledDistance
                    },
                    Discount = sale.Discount,
                    CustomerName = sale.Customer.Name,
                    Price = sale.Car.PartCars.Sum(x => x.Part.Price),
                    PriceWithDiscount = (sale.Car.PartCars.Sum(x => x.Part.Price)) - (sale.Car.PartCars.Sum(x => x.Part.Price) * sale.Discount / 100)

                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportSaleWithDiscountDto[]), new XmlRootAttribute("sales"));

            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), sales, xmlNamespaces);

            return sb.ToString().TrimEnd();

        }
    }
}