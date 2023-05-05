using System.ComponentModel.DataAnnotations;

namespace umweltV1.Data.ViewModels
{
    public class SignInVm
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Username { get; set; }


    }
}
