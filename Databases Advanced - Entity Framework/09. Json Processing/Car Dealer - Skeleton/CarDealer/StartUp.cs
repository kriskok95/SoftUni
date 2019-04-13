using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Migrations;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                string jsonString = File.ReadAllText("../../../Datasets/sales.json");

                Console.WriteLine(GetSalesWithAppliedDiscount(context));

            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var jsonParts = JsonConvert.DeserializeObject<Part[]>(inputJson);

            List<Part> result = new List<Part>();
            foreach (var jsonPart in jsonParts)
            {
                if (!context.Suppliers.Any(x => x.Id == jsonPart.SupplierId))
                {
                    continue;
                }

                result.Add(jsonPart);
            }

            context.Parts.AddRange(result);
            context.SaveChanges();

            return $"Successfully imported {result.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDtos = JsonConvert.DeserializeObject<List<CarPartsDto>>(inputJson);
            List<CarPartsDto> validCars = new List<CarPartsDto>();

            HashSet<int> partIds = context.Parts.Select(x => x.Id).ToHashSet();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CarPartsDto, Car>();

            });

            var mapper = config.CreateMapper();

            foreach (var car in carDtos)
            {
                bool isValid = false;

                foreach (var partId in car.PartsId.Distinct())
                {
                    if (partIds.Contains(partId))
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }

                if (isValid == true)
                {
                    validCars.Add(car);
                }
            }

            for (int i = 0; i < validCars.Count; i++)
            {
                var carTemp = mapper.Map<Car>(validCars[i]);
                foreach (var partId in validCars[i].PartsId.Distinct())
                {
                    carTemp.PartCars.Add(new PartCar(){CarId = i + 1, PartId = partId});
                }

                context.Add(carTemp);
            }

            int result = validCars.Count;
            context.SaveChanges();

            return $"Successfully imported {result}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(b => b.BirthDate)
                .ThenBy(y => y.IsYoungDriver)
                .Select(customer => new
                {
                    Name = customer.Name,
                    BirthDate = customer.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = customer.IsYoungDriver
                })
                .ToArray();

            string serializedCustomers = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return serializedCustomers;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(d => d.TravelledDistance)
                .Select(car => new
                {
                    Id = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                })
                .ToList();

            string result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;

        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            string result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {

            var cars = context.Cars.Select(c => new
            {
                car = new
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,                   
                },
                parts = c.PartCars.Select(p => new
                {
                    Name = p.Part.Name,
                    Price = $"{p.Part.Price:F2}"
                })
            }).ToList();

            string result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {

            var customers = context.Customers.Where(p => p.Sales.Any(y => y.Car != null))
                .OrderByDescending(x => x.Sales.Sum(y => y.Car.PartCars.Sum(s => s.Part.Price)))
                .ThenByDescending(x => x.Sales.Count)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Sum(x => x.Car.PartCars.Sum(y => y.Part.Price))
                }).ToList();        
              
            string result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var cars = context.Sales.Select(c => new
                {
                    car = new
                    {
                        Make = c.Car.Make,
                        Model = c.Car.Model,
                        TravelledDistance = c.Car.TravelledDistance
                    },
                    customerName = c.Customer.Name,
                    Discount = c.Discount.ToString("F2"),
                    price = c.Car.PartCars.Sum(x => x.Part.Price).ToString("F2"),
                    priceWithDiscount = (c.Car.PartCars.Sum(x => x.Part.Price) -
                                        (c.Car.PartCars.Sum(x => x.Part.Price) * (c.Discount / 100))).ToString("F2")
                })
                .ToList()
                .Take(10);

            string result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }
    }
}