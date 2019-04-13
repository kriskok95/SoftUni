using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Data
{
    public class MyAppContext : DbContext
    {
        //public MyAppContext()
        //{
            
        //}

        public MyAppContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
