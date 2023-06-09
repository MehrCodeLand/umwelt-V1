﻿using System.Text.RegularExpressions;
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

        // new and dear things
        public bool CheackPermissionID(int permissionId, string email)
        {

            int userId = _db.Users.FirstOrDefault(x => x.Email == email.ToLower()).UserId;

            List<int> roleIds = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();


            if (roleIds.Any()) { return false ; }

            foreach(int roleId in roleIds)
            {
                foreach( var rolePermission in _db.RolePermissions.Where(x => x.RoleId == roleId).ToList())
                {
                    if(rolePermission.PermissionId == permissionId) { return true; }
                }
            }

            return false;
        }

        #region IsExist

        /*
         -50:Username Exist
         50:Username Not Found   
         30:Email Is Not Found
         -30:Email IS Exist
         */
        private bool IsUsername(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }
        private bool IsEmail(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }
        #endregion

        #region SignIn

        public MessageData SignInUser(SignInVm signIn)
        {
            var message = new MessageData();
            signIn.Email = signIn.Email.ToLower();

            if (signIn == null)
            {
                message.ErrorId = -120;
                message.Message = "Invalid Data";

                return message;
            }
            else if (signIn.Email == null)
            {
                message.ErrorId = -100;
                message.Message = "Where is Email?";

                return message;
            }
            else if(signIn.Password == null)
            {
                message.ErrorId = -150;
                message.Message = "Where is password?";

                return message;
            }

            signIn.Password = HashPasswordC.EncodePasswordMd5(signIn.Password);

            var result = IsCorrectPassAndEmail(signIn);
            if (!result)
            {
                message.ErrorId = -300;
                message.Message = "Incorrect Email Or password";

                return message;
            }


            //find username
            signIn.Username = GetUsernameByEmail(signIn.Email);

            message.SuccessId = 200;
            message.Message = "Done,Now You are sign in !";
            return message;
        }
        private bool IsCorrectPassAndEmail(SignInVm signIn)
        {
            var password = _db.Users.FirstOrDefault(u => u.Email == signIn.Email).Password;
            if(password != null)
            {
                if(password == signIn.Password)
                {
                    return true;
                }
            }

            return false;
        }
        private string GetUsernameByEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email ).Username.ToLower();
        }
        #endregion

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
                DateToConfirmCode = DateTime.Now,
            };

            result = AddUser(user);
            result.Message = "Your Account Was created,We Sent you confirm-Code\n" +
                "please check your email now";

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
            else if(IsEmail(signUpUser.Email.ToLower()))
            {
                message.ErrorId = -30;
                message.Message = "Email Is Exist!";
                return message; 
            }
            else if(IsUsername(signUpUser.Username.ToLower()))
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

        #region Register Confirm Code

        public ConfirmEmailAcountVm CreateConfirmModel(string email)
        {
            var confirmCodeUser = _db.Users.FirstOrDefault(u => u.Email == email).ConfirmCode;

            var confirmVm = new ConfirmEmailAcountVm()
            {
                ConfirmCode = confirmCodeUser,
            };

            return confirmVm;
        }
        public ConfirmEmailAcountVm ModelForSendAgainEmail(string confirmCode)
        {
            // we should change confirm code again
            var user = _db.Users.FirstOrDefault(u => u.ConfirmCode == confirmCode);

            var confirmModel = new ConfirmEmailAcountVm()
            {
                ConfirmCode = user.ConfirmCode,
                Username = user.Username,
                Email = user.Email,
            };

            return confirmModel;
        }

        /*
         
        -900:fake data
        -2010:thats to late
        -3000:We cant update user
         300:Done
         */
        public MessageData AcceeptUser(string confirmCode)
        {
            var message = new MessageData();

            var user = _db.Users.FirstOrDefault(u => u.ConfirmCode == confirmCode);

            if (user == null)
            {
                message.ErrorId = -900;
                message.Message = "Invalid Data and code!";
                return message ;
            }
            
            
            if( !(user.DateToConfirmCode.AddMinutes(15) > DateTime.Now))
            {
                // we will send him email again
                // we change confirm code again 

                user.ConfirmCode = CreateConfirmCodeEmail.ConfirmCode();
                user.DateToConfirmCode = DateTime.Now;

                UpdateUser(user);

                message.ErrorId = -2010;
                message.Message = "thtas to late,we will send\n" +
                    " you another email\n" +
                    "be quick ;)";

                return message;
            }



            // ready to accept him
            user.IsConfirmCode = true;
            user.ConfirmCode = CreateConfirmCodeEmail.ConfirmCode();


            // time to update user
            var result = UpdateUser(user);
            if(result == false)
            {
                // we need better this latter
                message.ErrorId = -3000;
                message.Message = "We cant do this right now!";

                return message;
            }

            message.SuccessId = 300;
            message.Message = "Dooone!\n" +
                "now your our frinds";
            return message;
        }

        private bool UpdateUser( User user)
        {
            _db.Users.Update(user);
            Save();
            return true;
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
