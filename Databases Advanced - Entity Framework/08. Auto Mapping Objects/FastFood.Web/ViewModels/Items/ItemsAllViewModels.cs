using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Items
{
    public class ItemsAllViewModels
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }
    }
}
