using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore.Storage;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string xmlString = File.ReadAllText("../../../Datasets/categories-products.xml");

            using (ProductShopContext context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));

            var usersDto = (ImportUserDto[])serializer.Deserialize(new StringReader(inputXml));

            List<User> users = new List<User>();

            foreach (var userDto in usersDto)
            {
                User user = new User()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Age = userDto.Age
                };
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productsDto = (ImportProductDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Product> products = new List<Product>();

            foreach (var productDto in productsDto)
            {
                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId
                };
                products.Add(product);
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategorieDto[]), new XmlRootAttribute("Categories"));
            var categoriesDto = (ImportCategorieDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Category> categories = new List<Category>();

            foreach (var categoryDto in categoriesDto)
            {
                if (!IsValid(categoryDto))
                {
                    continue;
                }

                Category category = new Category
                {
                    Name = categoryDto.Name
                };

                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoriesAndProductsDto[]), new XmlRootAttribute("CategoryProducts"));
            var categoryProductsDto = (ImportCategoriesAndProductsDto[])serializer.Deserialize(new StringReader(inputXml));

            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDto)
            {
                if (context.Categories.Select(x => x.Id).Contains(categoryProductDto.CategoryId) &&
                    context.Products.Select(x => x.Id).Contains(categoryProductDto.ProductId))
                {
                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = categoryProductDto.CategoryId,
                        ProductId = categoryProductDto.ProductId
                    };
                    categoryProducts.Add(categoryProduct);
                }
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ExportProductsDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportProductsDto[]), new XmlRootAttribute("Products"));
            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), products, xmlNamespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .Select(x => new ExportSoldProductsDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(p => new SoldProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToArray()
                })
                .OrderBy(x => x.LastName)
                .ThenBy(f => f.FirstName)
                .Take(5)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportSoldProductsDto[]), new XmlRootAttribute("Users"));
            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new ExportCategoriesDto()
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(x => x.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(p => p.Count)
                .ThenBy(t => t.TotalRevenue)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCategoriesDto[]), new XmlRootAttribute("Categories"));
            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = new ExportUsersWithProductsDto()
            {
                Count = context.Users.Count(x => x.ProductsSold.Any()),
                Users = context.Users
                    .Where(x => x.ProductsSold.Any())
                    .Select(u => new ExportUserDto()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new ExportSoldProductsWithCountDto()
                        {
                            Count = u.ProductsSold.Count,
                            Products = u.ProductsSold.Select(x => new SoldProductDto()
                                {
                                    Name = x.Name,
                                    Price = x.Price
                                })
                                .OrderByDescending(x => x.Price)
                                .ToArray()
                        }
                    })
                    .OrderByDescending(y => y.SoldProducts.Count)
                    .Take(10)
                    .ToArray()
            };

            var serializer = new XmlSerializer(typeof(ExportUsersWithProductsDto), new XmlRootAttribute("Users"));
            var sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}