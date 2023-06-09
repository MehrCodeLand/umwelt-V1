﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace umweltV1.Data.Models.Users
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int MyUserId { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string Email { get; set; }
        public string ConfirmCode { get; set; }
        public bool IsConfirmCode { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string Avatar { get; set; } = "helloWorld.jpg";
        public string? DisplayName { get; set; }
        public string Password { get; set; }
        public DateTime DateToConfirmCode { get; set; }


        #region Rel

        public IEnumerable<UserRole>? UserRoles { get; set; }



        #endregion
    }
}
