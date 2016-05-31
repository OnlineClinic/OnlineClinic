using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Helper
{
    public static class PermissionHelper
    {
        static IPermissionInModuleService _permissionInModuleService;
        static IUserService _userService;

        static PermissionHelper()
        {
            _permissionInModuleService = new PermissionInModuleService();
            _userService = new UserService();
        }

        public static bool CheckPermission(string permissionName, int moduleId, Guid loginId)
        {
            var checkPermission = false;
            var loginName = StringCipher.Decrypt(_userService.Find(loginId).LoginName);

            if (loginName == "admin")
            {
                checkPermission = true;
                return checkPermission;
            }

            var permissionList = _permissionInModuleService.GetPermissionInModuleByUserId(loginId).Where
                                                                    (
                                                                        x => x.Permisions.PermissionName == permissionName && x.ModuleId == moduleId 
                                                                        && x.IsActive == true &&
                                                                       x.IsApproved == true
                                                                    ).ToList();
            

            if (permissionList != null && permissionList.Count > 0)
                checkPermission = true;

            return checkPermission;
        }
    }
}