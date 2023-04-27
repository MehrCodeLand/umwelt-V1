using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace umweltV1.Data.Models.Users
{
    public class Permission
    {
        [Key]
        public int PermissionID { get; set; }
        public int MyPermissionId { get; set; }
        public string Title { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Permission? Parent { get; set; }

        public IEnumerable<RolePermission> RolePermissions { get; set; }

    }
}
