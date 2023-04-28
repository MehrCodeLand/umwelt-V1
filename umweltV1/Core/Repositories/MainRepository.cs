using System.Text.RegularExpressions;
using umweltV1.Core.Interfaces;
using umweltV1.Data.Models.Structs;
using umweltV1.Data.Models.Users;
using umweltV1.Data.MyDbContext;
using umweltV1.Data.ViewModels;
using umweltV1.Security.Creator;
using umweltV1.Security.PasswordHelper;

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
        public MessageData SignUpUser(SignUpUserVm signUpUser)
        {
            var result = ValidationSignUpData(signUpUser);
            if(result.ErrorId != 0) { return result; }


            var user = new User()
            {
                MyUserId = CreateRandomId.CreateId(),
                Username = signUpUser.Username.ToLower(),
                Email = signUpUser.Email.ToLower(),
                Password = HashPasswordC.EncodePasswordMd5(signUpUser.Password),
                ConfirmCode = CreateConfirmCodeEmail.ConfirmCode(),
            };

            result = AddUser(user);
            return result;
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
        -1001:Somthings Wrong
         */
        private MessageData ValidationSignUpData(SignUpUserVm signUpUser)
        {
            MessageData message = new MessageData();

            if(signUpUser == null)
            { 
                message.ErrorId = -100;
                message.Message = "Data is NULL!";
                return message ;
            }
            else if(signUpUser.Password == null)
            {
                message.ErrorId = -100;
                message.Message = "Password is NULL!";
                return message;
            }
            else if(signUpUser.RePassword == null )
            {
                message.ErrorId = -100;
                message.Message = "RePassword is NULL!";
                return message;
            }
            else if(signUpUser.Username == null || signUpUser.Username.Length < 3 )
            {
                message.ErrorId = -120;
                message.Message = "your Username is to short!";
                return message ;
            }
            else if(signUpUser.Email == null )
            {
                message.ErrorId = -150;
                message.Message = "your Email is null!";
                return message;
            }
            else if(signUpUser.Password != signUpUser.RePassword )
            {
                message.ErrorId = -130;
                message.Message = "Your PAssword And Repassword Are not match!";
                return message;
            }
            else if(signUpUser.Password.Length < 8 )
            {
                message.ErrorId = -140;
                message.Message = "Your Password is to short!";
                return message;
            }

            else if(ValidateEmailRegex(signUpUser.Email.ToLower()) == -60)
            {
                message.ErrorId =-60;
                message.Message = "Email Adress is not valid";
                return message;    
            }
            else if(IsEmail(signUpUser.Email.ToLower()) == -50)
            {
                message.ErrorId = -30;
                message.Message = "Email Is Exist!";
                return message; 
            }
            else if(IsUsername(signUpUser.Username.ToLower()) == -50)
            {
                message.ErrorId = -50;
                message.Message = "Username Is Exist!";
                return message;    
            }
            else if(signUpUser.Username.Any(char.IsDigit) == true )
            {
                message.ErrorId = -70;
                message.Message = "Username mussent have a number!";
                return message;
            }



            message.SuccessId = 100;
            message.ErrorId = 0;
            return message;
        }
        private int ValidateEmailRegex(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            var result = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);

            if(result != true) { return -60; }
            return 60;
        }
        private MessageData AddUser(User user)
        {
            _db.Users.Add(user);
            Save();

            var IsAdded = IsUserCreate(user.Username);

            MessageData message = new MessageData();
            if (IsAdded != true)
            {
                message.ErrorId = -500;
                message.Message = "We Have Problem,now try again!";
                return message;
            }

            message.SuccessId = 500;
            message.Message = "Be Green :)";
            return message;
        }
        private bool IsUserCreate(string username)
        {
            return _db.Users.Any(u => u.Username == username);
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

        #region CRUD_BASE

        /*
         -500:Faild to Add
         500:We Add User
         */

        private void Save()
        {
            _db.SaveChanges();
        }
        #endregion
    }
}
