using System;
using System.Collections.Generic;
using System.Text;
using MyApp.Models;

namespace MyApp.Core.ViewModels
{
    public class EmployeeWithManagerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public Employee Manager { get; set; }
    }
}
