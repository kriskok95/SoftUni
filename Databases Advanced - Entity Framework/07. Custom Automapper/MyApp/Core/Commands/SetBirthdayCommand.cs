using System;
using System.Globalization;
using AutoMapper;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;
using ICommand = MyApp.Core.Commands.Contracts.ICommand;

namespace MyApp.Core.Commands
{
    class SetBirthdayCommand  : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetBirthdayCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandParams)
        {
            int employeeId = int.Parse(commandParams[0]);

            DateTime birthday = DateTime.ParseExact(commandParams[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Employee employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new InvalidOperationException("User with this ID doesn't exist!");
            }

            employee.Birthday = birthday;
            context.SaveChanges();

            var setBirthdayDto = this.mapper.CreateMappedObject<SetBirthdayDto>(employee);

            string result = $"{setBirthdayDto.FirstName} {setBirthdayDto.LastName} is born in {setBirthdayDto.Birthday:dd-MM-yyyy}";

            return result;
        }
    }
}
