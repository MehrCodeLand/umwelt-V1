﻿using umweltV1.Data.Models.Structs;
using umweltV1.Data.ViewModels;

namespace umweltV1.Core.Interfaces
{
    public interface IAdminService
    {
        MessageData CreatateRole( CreateRoleVm createRole );
    }
}
