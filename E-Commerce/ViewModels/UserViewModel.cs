using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
