using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Categories
{
    public class CategoryAllViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
