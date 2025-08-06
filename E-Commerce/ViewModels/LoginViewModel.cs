using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; } // Username or Email

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
