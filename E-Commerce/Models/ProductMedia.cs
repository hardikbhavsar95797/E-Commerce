using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("ProductMedia")]
    public class ProductMedia
    {
        [Key]
        public long Id { get; set; }
        public string ImagePath { get; set; }
        public long ProductId { get; set; }
        public bool Is_Deleted { get; set; }
        public DateTime Created_At { get; set; }
        public Product Product { get; set; }
    }
}
