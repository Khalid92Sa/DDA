using DDA.Business.Interfaces;
using DDA.ViewModels.Enum;
using SSSFramework;
using SSSFramework.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace DDA.Business.Security
{
    public class RolePermissionAttribute : RolePermissionBaseAttribute
    {
        public RolePermissionAttribute(params UserPermission[] permission)
                : base(Array.ConvertAll(permission, value => (int)value))
        {
        }

        protected override void DoAuthorization(AuthorizationContext filterContext)
        {
            var userService = IoC.Resolve<IUserService>();

            if (HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException();
            }

            var authenticationResult =
                userService.CheckCurrentUserPermissions(HttpContext.Current.User.Identity.Name,
                    _requiredPermission);

            if (!authenticationResult)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {

                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }
    }
}
