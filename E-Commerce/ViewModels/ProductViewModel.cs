using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace E_Commerce.ViewModels
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000, ErrorMessage = "Enter valid price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public long Quantity { get; set; }

    }
}
