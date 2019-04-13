using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyApp.Models
{
    public class Employee
    {
        public Employee()
        {
            this.ManagedEmployees = new List<Employee>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Range(typeof(decimal), "0.0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        public DateTime? Birthday { get; set; }

        public string Address { get; set; }

        //[ForeignKey("Manager")]
        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }

        public List<Employee> ManagedEmployees { get; set; }
    }
}
