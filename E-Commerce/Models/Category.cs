using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Is_Deleted { get; set; }
        public DateTime Created_At { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
