using System.Text.RegularExpressions;
using umweltV1.Core.Interfaces;
using umweltV1.Data.Models.Structs;
using umweltV1.Data.Models.Users;
using umweltV1.Data.MyDbContext;
using umweltV1.Data.ViewModels;
using umweltV1.Security.Creator;

namespace umweltV1.Core.Repositories
{
    public class AdminRepository : IAdminService
    {
        private readonly MyDb _db;
        public AdminRepository(MyDb db)
        {
            _db = db;
        }



        #region Permission

        public MessageData CreatePermission(CreatePermisionVm permisionVm)
        {
            MessageData message = new MessageData();

            message = ValidatePermission(permisionVm);
            if(message.ErrorId < 0)
            {
                return message;
            }


            // parent id 
            Permission permission = new Permission()
            {
                MyPermissionId = CreateRandomId.CreateId(),
                Title = permisionVm.Title.ToLower(),
            };

            message = AddPermission(permission);
            return message;
        }

        private MessageData AddPermission(Permission permission)
        {
            MessageData message = new MessageData();

            _db.Permissions.Add(permission);
            Save();

            var result = IsPermissionAdedd(permission.Title);
            if (!result)
            {
                message.SuccessId = 100;
                message.Message = "somthings wrong!";
                return message;
            }

            message.ErrorId = -100;
            message.Message = "Done.";
            return message;
        }

        private bool IsPermissionAdedd(string title)
        {
            return _db.Permissions.Any(u => u.Title == title);
        }



        /*
        100: Done
        -50: title is to short
        -30: title is to long
        -70: title has number
        -10: title is exist
        -120: special char error 
         */
        private MessageData ValidatePermission(CreatePermisionVm permisionVm)
        {
            MessageData message = new MessageData();

            if(permisionVm.Title == null || permisionVm.Title.Length < 3)
            {
                message.ErrorId = -50;
                message.Message = "title is to short";

                return message;
            }
            else if(permisionVm.Title.Length > 15)
            {
                message.ErrorId = -30;
                message.Message = "title is to long";

                return message;
            }
            else if(permisionVm.Title.Any(char.IsDigit))
            {
                message.ErrorId = -70;
                message.Message = "title has number!";

                return message;
            }
            if(PermissionIsExist(permisionVm.Title.ToLower()) == true)
            {
                message.ErrorId = -10;
                message.Message = "This Title is exist!";

                return message;
            }
            else if (RegexIsSpecialCharacters(permisionVm.Title.ToLower()))
            {
                message.ErrorId = -120;
                message.Message = "Spacial Characters!";

                return message;
            }


            return message; 
        }


        private bool PermissionIsExist( string title)
        {
            return _db.Permissions.Any(u => u.Title == title);
        }

        private bool RegexIsSpecialCharacters(string title)
        {
            var regexItem = new Regex("^[a-zA-Z]*$");
            if (!regexItem.IsMatch(title)) { return false; }

            return true;
        }

        #endregion

        #region Role

        /*
         -100: Title to short
        -50:incorrect Structure
        -150:title is to long
        100:Done
         */
        public MessageData CreateRole(CreateRoleVm createRole)
        {

            // validate first 
            MessageData message= new MessageData();
            message = ValidateRole(createRole.Tiltle);
            if (message.ErrorId < 0) { return message;  }

            Role role = new Role()
            {
                MyRoleId = CreateRandomId.CreateId(),
                Title = createRole.Tiltle.ToLower(),
                IsDeleted = false,
            };


            message = AddRole(role);
            return message;
        }
        private MessageData ValidateRole(string title)
        {
            MessageData message = new MessageData();

            if (title == null || title.Length < 3)
            {
                message.Message = "Title is to short!";
                message.ErrorId = -100;

                return message;
            }
            else if (title.Any(char.IsDigit))
            {
                message.ErrorId = -50;
                message.Message = "this structure for Title is not correct!";

                return message;
            }
            else if(title.Length > 15)
            {
                message.ErrorId = -150;
                message.Message = "title is to long!";

                return message;
            }


            message.Message = "Done.";
            message.SuccessId = 100;
            return message;
        }
        private MessageData AddRole(Role role)
        {
            _db.Roles.Add(role);
            Save();

            MessageData message = new MessageData();

            // IS adedd?
            var result = IsRoleAdded(role.Title);
            if(result == false)
            {
                message.ErrorId = -200;
                message.Message = "Somthings Wrong ";

                return message;
            }

            message.SuccessId = 100;
            message.Message = "Done.";

            return message;
        }
        private bool IsRoleAdded(string title)
        {
            return _db.Roles.Any(u => u.Title == title);
        }
        private void Save()
        {
            _db.SaveChanges();
        }




        #endregion

    }
}
