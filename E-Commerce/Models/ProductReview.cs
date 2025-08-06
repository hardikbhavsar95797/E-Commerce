using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("ProductReview")]
    public class ProductReview
    {
        [Key]
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int UserId { get; set; }
        [Range(1, 5, ErrorMessage = "Apply Valid Rating")]
        public int Rating { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ReviewText { get; set; }
        public bool Is_Deleted { get; set; }
        public DateTime Created_At { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
