using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using Remotion.Linq.Parsing;
using Type = P01_BillsPaymentSystem.Data.Models.Type;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (BillsPaymentSystemContext dbContext = new BillsPaymentSystemContext())
            {
                //dbContext.Database.EnsureDeleted();
                //dbContext.Database.EnsureCreated();
                //Seed(dbContext);


                //PrintUserDetails(dbContext);

                //PayBills(dbContext);

            }
        }

        private static void PayBills(BillsPaymentSystemContext dbContext)
        {

            try
            {
                Console.Write("Enter user ID: ");
                int userId = int.Parse(Console.ReadLine());
                Console.Write("Enter amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                var bankAccounts = dbContext.PaymentMethods
                    .Where(x => x.UserId == userId && x.Type == Type.BankAccount)
                    .Select(pm => pm.BankAccount)
                    .ToList();

                var creditCards = dbContext.PaymentMethods
                    .Where(x => x.UserId == userId && x.Type == Type.CreditCard)
                    .Select(cc => cc.CreditCard)
                    .ToList();

                var availableMoney = creditCards.Sum(m => m.LimitLeft) + bankAccounts.Sum(m => m.Balance);

                if (availableMoney < amount)
                {
                    throw new InvalidOperationException("The given user doesn't have enough money!");
                }

                foreach (var account in bankAccounts)
                {
                    if (account.Balance == 0 || amount == 0)
                    {
                        continue;
                    }

                    if (account.Balance < amount)
                    {
                        amount -= account.Balance;
                        account.Withdraw(account.Balance);
                    }
                    else
                    {
                        account.Withdraw(amount);
                        amount = 0;
                    }
                }

                foreach (var creditCard in creditCards)
                {
                    if (creditCard.LimitLeft == 0 || amount == 0)
                    {
                        continue;
                    }

                    if (creditCard.LimitLeft < amount)
                    {
                        amount -= creditCard.LimitLeft;
                        creditCard.Withdraw(creditCard.LimitLeft);
                    }
                    else
                    {
                        creditCard.Withdraw(amount);
                        amount = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            dbContext.SaveChanges();
        }


        private static void PrintUserDetails(BillsPaymentSystemContext dbContext)
        {
            int userId = int.Parse(Console.ReadLine());

            var user = dbContext.Users
                .Where(x => x.UserId == userId)
                .Select(u => new
                {
                    Name = u.FirstName + " " + u.LastName,
                    BankAccounts = u.PaymentMethods
                        .Where(pm => pm.Type == Type.BankAccount)
                        .Select(pm => pm.BankAccount)
                        .ToList(),
                    CreditCars = u.PaymentMethods
                        .Where(pm => pm.Type == Type.CreditCard)
                        .Select(x => x.CreditCard)
                        .ToList()
                })
                .FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine("User with this ID doesn't exist!");
                return;
            }

            Console.WriteLine($"User: {user.Name}");
            Console.WriteLine("Bank Accounts:");
            foreach (var bankAccount in user.BankAccounts)
            {
                Console.WriteLine($"-- ID: {bankAccount.BankAccountId}");
                Console.WriteLine($"--- Balance: {bankAccount.Balance}");
                Console.WriteLine($"--- Bank: {bankAccount.BankName}");
                Console.WriteLine($"--- SWIFT: {bankAccount.Swift}");
            }

            Console.WriteLine($"Credit cards:");
            foreach (var creditCard in user.CreditCars)
            {
                Console.WriteLine($"-- ID: {creditCard.CreditCardId}");
                Console.WriteLine($"--- Limit: {creditCard.Limit}");
                Console.WriteLine($"--- Money Owed {creditCard.MoneyOwned}");
                Console.WriteLine($"--- Money Left: {creditCard.LimitLeft}");
                Console.WriteLine($"--- Expiration Date: {creditCard.ExpirationDate.ToString("yyyy/MM")}");
            }
        }

        static void Seed(BillsPaymentSystemContext context)
        {
            var users = new[]
            {
                new User
                {
                    FirstName = "Rumen",
                    LastName = "Radev",
                    Email = "Rumen@abv.bg",
                    Password = "123452"
                },

                new User
                {
                    FirstName = "Gosho",
                    LastName = "Petrov",
                    Email = "gosho@gmail.com",
                    Password = "234"
                },

                new User
                {
                    FirstName = "Rejep",
                    LastName = "Ivedic",
                    Email = "Baklavata@gmail.com",
                    Password = "535353"
                },

                new User
                {
                    FirstName = "Pavle",
                    LastName = "Vladimirov",
                    Email = "Pavel_88@abv.bg",
                    Password = "44455"
                }
            };

            context.Users.AddRange(users);

            var creditCards = new[]
            {
                new CreditCard(500m, 250m, new DateTime(1995, 02, 12)),
                new CreditCard(1200, 300m, new DateTime(2002, 02, 25)),
                new CreditCard(2500m, 450m, new DateTime(2012, 06, 12)),
                new CreditCard(10000m, 3000m, new DateTime(2025, 05, 25))
            };

            context.CreditCards.AddRange(creditCards);

            var bankAccounts = new[]
            {
                new BankAccount(2000m, "MyAccount", "swift1"),
                new BankAccount(13000m, "Security", "swift2"),
                new BankAccount(250m, "RareAccount", "swift3"),
                new BankAccount(2300m, "TheLastOne", "swift4")
            };

            context.BankAccounts.AddRange(bankAccounts);

            var paymentMethods = new[]
            {
                new PaymentMethod
                {
                    User = users[0],
                    Type = Type.BankAccount,
                    BankAccount = bankAccounts[0]
                },

                new PaymentMethod
                {
                    User = users[0],
                    Type = Type.BankAccount,
                    BankAccount = bankAccounts[1]
                },

                new PaymentMethod
                {
                    User = users[0],
                    Type = Type.CreditCard,
                    CreditCard = creditCards[0]
                },

                new PaymentMethod
                {
                    User = users[1],
                    Type = Type.CreditCard,
                    CreditCard = creditCards[1]
                },

                new PaymentMethod
                {
                    User = users[2],
                    Type = Type.BankAccount,
                    BankAccount = bankAccounts[2]
                },

                new PaymentMethod
                {
                    User = users[2],
                    Type = Type.CreditCard,
                    CreditCard = creditCards[2]
                },

                new PaymentMethod
                {
                    User = users[2],
                    Type = Type.CreditCard,
                    CreditCard = creditCards[3]
                },

                new PaymentMethod
                {
                    User = users[3],
                    Type = Type.BankAccount,
                    BankAccount = bankAccounts[3]
                }
            };

            context.PaymentMethods.AddRange(paymentMethods);

            context.SaveChanges();
        }
    }
}
