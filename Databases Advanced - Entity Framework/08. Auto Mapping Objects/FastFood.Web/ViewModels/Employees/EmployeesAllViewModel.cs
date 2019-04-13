using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Employees
{
    public class EmployeesAllViewModel
    {
        [Required]
        public string Name { get; set; }

        [Range(15, 65)]
        public int Age { get; set; }

        public string Address { get; set; }

        public string Position { get; set; }
    }
}
