﻿using umweltV1.Data.Models.Structs;
using umweltV1.Data.ViewModels;

namespace umweltV1.Core.Interfaces
{
    public interface IMainService
    {
        MessageData SignUpUser(SignUpUserVm signUpUser);
        bool CheackPermissionID(int permissionId, string email);
        ConfirmEmailAcountVm CreateConfirmModel(string email);
        MessageData AcceeptUser(string confirmCode);
        ConfirmEmailAcountVm ModelForSendAgainEmail(string confirmCode);
        MessageData SignInUser(SignInVm signIn);
    }
}
