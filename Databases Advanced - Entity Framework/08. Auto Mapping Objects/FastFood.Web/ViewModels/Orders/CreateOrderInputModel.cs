using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Orders
{
    public class CreateOrderInputModel
    {
        [Required]
        public string Customer { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
