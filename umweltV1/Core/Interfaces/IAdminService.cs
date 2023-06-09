﻿using umweltV1.Data.Models.Structs;
using umweltV1.Data.ViewModels;

namespace umweltV1.Core.Interfaces
{
    public interface IAdminService
    {
        MessageData CreateRole( CreateRoleVm createRole );
        MessageData CreatePermission( CreatePermisionVm permisionVm );
        IList<string> GetAllPermissionTitle();
    }
}
