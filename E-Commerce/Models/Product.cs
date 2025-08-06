using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
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

        public bool Is_Deleted { get; set; }

        public DateTime Created_At { get; set; }

        public Category Category { get; set; }
        public ICollection<ProductMedia> ProductMedias { get; set; }

    }
}
