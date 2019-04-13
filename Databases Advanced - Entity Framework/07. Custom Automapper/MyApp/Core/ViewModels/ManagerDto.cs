using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using MyApp.Models;

namespace MyApp.Core.ViewModels
{
    public class ManagerDto
    {
        public ManagerDto()
        {
            this.ManagedEmployees = new List<Employee>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Employee> ManagedEmployees { get; set; }
    }
}
