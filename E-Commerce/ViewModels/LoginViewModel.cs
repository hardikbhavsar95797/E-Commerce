using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName or Email is required")]
        public string Login { get; set; } // Username or Email

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
