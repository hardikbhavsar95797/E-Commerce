using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, 10000)]
        public decimal Price { get; set; }

        public long CategoryId { get; set; }

        public long Quantity { get; set; }

        public bool Is_Deleted { get; set; }

        public DateTime Created_At { get; set; }

        public Category Category { get; set; }
        public ICollection<ProductMedia> ProductMedias { get; set; }

    }
}
