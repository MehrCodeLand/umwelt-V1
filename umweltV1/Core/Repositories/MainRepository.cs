using System.Text.RegularExpressions;
using umweltV1.Core.Interfaces;
using umweltV1.Data.Models.Users;
using umweltV1.Data.MyDbContext;
using umweltV1.Data.ViewModels;

namespace umweltV1.Core.Repositories
{
    public class MainRepository : IMainService
    {
        private readonly MyDb _db;
        public MainRepository(MyDb db)
        {
            _db = db;
        }


        #region SignUp User
        public int SignUpUser(SignUpUserVm signUpUser)
        {
            var result = ValidationSignUpData(signUpUser);
            if(result < 0 ) return result;


            return 0; 
        }

        /*
         * Validation Code
        -100:NULL
        -120:Username is to short
        -130:Compare Password
        -140:Password To Short
        -150:Email is Null
        -50:Username Is Exist 
        -30:Email Is Exist
        -60:Email Structure is Not Correct
        -70:Username Has a inCorecct Structure
        100:All  IS ok
         */
        private int ValidationSignUpData(SignUpUserVm signUpUser)
        {
            if(signUpUser == null) { return -100 ; }
            else if(signUpUser.Password == null) { return -100 ; }
            else if(signUpUser.RePassword == null ) { return -100 ; }
            else if(signUpUser.Username == null || signUpUser.Username.Length < 3 ) { return -120 ; }
            else if(signUpUser.Email == null ) { return -150;  }
            else if(signUpUser.Password != signUpUser.RePassword ) { return -130;  }
            else if(signUpUser.Password.Length < 8 ) { return -140; }

            else if(ValidateEmailRegex(signUpUser.Email.ToLower()) == -60) { return -60; }
            else if(IsEmail(signUpUser.Email.ToLower()) == -50) { return -30; }
            else if(IsUsername(signUpUser.Username.ToLower()) == -50) { return -50; }
            else if(signUpUser.Username.Any(char.IsDigit) == true ) { return -70; }

            return 100 ;
        }
        private int ValidateEmailRegex(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            var result = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);

            if(result != true) { return -60; }
            return 60;
        }

        #endregion

        #region IsExist

        /*
         -50:Username Exist
         50:Username Not Found   
         30:Email Is Not Found
         -30:Email IS Exist
         */
        private int IsUsername(string username)
        {
            var result = _db.Users.Any(u => u.Username == username);
            if (result) { return -50;  }

            return 50;
        }
        private int IsEmail(string email)
        {
            bool result = _db.Users.Any(u => u.Email == email);
            if (result) { return -30; }

            return 30;
        }
        #endregion
    }
}
