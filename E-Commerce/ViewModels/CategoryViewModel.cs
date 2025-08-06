using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class CategoryViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
