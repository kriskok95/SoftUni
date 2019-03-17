using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
            
        }

        public CreditCard(decimal limit, decimal moneyOwned, DateTime date)
            
        {
            this.Limit = limit;
            this.MoneyOwned = moneyOwned;
            this.ExpirationDate = date;
        }

        [Key]
        public int CreditCardId { get; set; }

        [Required]
        public decimal Limit { get; private set; }

        [Required]
        public decimal MoneyOwned { get; private set; }

        [NotMapped]
        public decimal LimitLeft => this.Limit - this.MoneyOwned;

        [Required]
        public DateTime ExpirationDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal amount)
        {
            if (MoneyOwned - amount >= 0)
            {
                this.MoneyOwned -= amount;
            }
            else
            {
                Console.WriteLine($"Insufficient funds!");
            }
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this.MoneyOwned += amount;
            }
        }
    }
}
