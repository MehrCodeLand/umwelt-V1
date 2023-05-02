using System.ComponentModel.DataAnnotations;

namespace umweltV1.Data.ViewModels
{
    public class SignUpUserVm
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(15 , ErrorMessage = "Max:20")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(20)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [MaxLength(20)]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string RePassword { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string ConfirmCode { get; set; }
    }
}
