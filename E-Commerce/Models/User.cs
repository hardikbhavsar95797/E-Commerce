using E_Commerce.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName is required"), MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime Created_At { get; set; }

        public bool Is_Deleted { get; set; }
        public ICollection<ProductReview> ProductReviews { get; set; }
    }
}
