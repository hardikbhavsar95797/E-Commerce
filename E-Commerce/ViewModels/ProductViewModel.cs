using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required]
        public long CategoryId { get; set; }

        public long Quantity { get; set; }

    }
}
