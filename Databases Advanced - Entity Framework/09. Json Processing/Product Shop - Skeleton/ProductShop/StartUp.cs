using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                //string jsonString = File.ReadAllText("../../../Datasets/categories-products.json");

                Console.WriteLine(GetUsersWithProducts(context));

                double number = 2.22;

                Console.WriteLine(number);

            }
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var jsonUsers = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(jsonUsers);
            context.SaveChanges();

            return $"Successfully imported {jsonUsers.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var deserializedProducts = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(deserializedProducts);
            context.SaveChanges();

            return $"Successfully imported {deserializedProducts.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var deserializedCategories = JsonConvert.DeserializeObject<Category[]>(inputJson, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var validCategories = new List<Category>();

            foreach (var deserializedCategory in deserializedCategories)
            {
                if (deserializedCategory.Name == null)
                {
                    continue;
                }
                validCategories.Add(deserializedCategory);
            }

            context.Categories.AddRange(validCategories);

            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var deserializedCategoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
;

            context.CategoryProducts.AddRange(deserializedCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {deserializedCategoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(product => new
                {
                    name = product.Name,
                    price = product.Price,
                    seller = product.Seller.FirstName + " " + product.Seller.LastName ?? product.Seller.LastName
                })
                .ToList();

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);

            return result;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(x => x.Buyer != null))
                .OrderBy(l => l.LastName)
                .ThenBy(f => f.FirstName)
                .Select(user => new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    soldProducts = user.ProductsSold.Select(product => new
                    {
                        name = product.Name,
                        price = product.Price,
                        buyerFirstName = product.Buyer.FirstName,
                        buyerLastName = product.Buyer.LastName
                    }).ToList()
                })
                .ToList();

            string result = JsonConvert.SerializeObject(users, Formatting.Indented);

            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(cat => new
                {
                    category = cat.Name,
                    productsCount = cat.CategoryProducts.Count,
                    averagePrice = $"{cat.CategoryProducts.Average(p => p.Product.Price):F2}",
                    totalRevenue = $"{cat.CategoryProducts.Sum(s => s.Product.Price):F2}"
                })
                .ToList();

            string result = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return result;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = new
            {
                usersCount = context.Users.Count(u => u.ProductsSold.Count > 0 && u.ProductsSold.Any(x => x.BuyerId != null)),
                users = context.Users
                    .Where(u => u.ProductsSold.Count > 0 && u.ProductsSold.Any(x => x.Buyer != null))
                    .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(x => x.Buyer != null),
                        products = u.ProductsSold
                            .Where(x => x.Buyer != null)
                            .Select(p => new
                        {
                            name = p.Name,
                            price = $"{p.Price}"
                        }).ToList()
                    }
                })
                    .OrderByDescending(x => x.soldProducts.count)
                    .ToList()
            };

            string result = JsonConvert.SerializeObject(users, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                
            });

            return result;
        }
    }
}