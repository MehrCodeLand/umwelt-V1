using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using umweltV1.Core.Interfaces;

namespace umweltV1.Data.Models.Attributes
{
    public class PermissionCheckerAttribute : Attribute, IAuthorizationFilter
    {
        private IMainService _main;
        private readonly int _permissionId;

        public PermissionCheckerAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // if he is loggin 
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                _main = (IMainService)context.HttpContext.RequestServices.GetService(typeof(IMainService));
                string email = context.HttpContext.User.Identity.Name;

                if (!_main.CheackPermissionID(_permissionId, email))
                {
                    context.Result = new RedirectResult("/profile");
                }
            }

            context.Result = new RedirectResult("/login");
        }
    }
}
