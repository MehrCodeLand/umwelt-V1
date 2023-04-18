using System.ComponentModel.DataAnnotations;

namespace umweltV1.Data.Models.Users
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }


        #region Rel

        public IEnumerable<UserRole> UserRoles { get; set; }

        #endregion
    }
}
