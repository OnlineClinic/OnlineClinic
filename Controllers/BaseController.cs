using Omu.Encrypto;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Migrator;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using MyOnlineClinic.Web.Helper;

namespace MyOnlineClinic.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IEntitiesDB entitiesDb = new EntitiesDB();
        // protected IUserService _userService;
        //protected IRoleService _roleService;
        //protected IUserSubscriptionService _subscriptionService;
        private readonly IPermissionInModuleService _permissioninmoduleService;
        protected LoginUserViewModel loginUserModel;
        private readonly IUserService _userService;
        private IHasher hasher = new Hasher();
        public BaseController()
        {
            _permissioninmoduleService = new PermissionInModuleService();
            _userService = new UserService();
            //_userService = new UserService();
            //_roleService = new RoleService();
            //_subscriptionService = new UserSubscriptionService();
            hasher.SaltSize = 10;
        }



        [RequireHttps]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {

                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket;
                if (authCookie != null)
                {
                    ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        string[] userInfo = ticket.UserData.Split(':');
                        loginUserModel = new LoginUserViewModel
                        {
                            FullName = userInfo[0],
                            UserRoleName = userInfo[1],
                            LoginId = new Guid(userInfo[2]),
                            //MemberId = Convert.ToInt32(userInfo[3]),
                            Avatar = userInfo[3],
                            Logo = userInfo[4],
                            OrganizationName = userInfo[5],
                            IsAdmin = Convert.ToBoolean(userInfo[6]),
                            ClinicName = userInfo.Length > 7 ? userInfo[7] : "",
                            ClinicId = userInfo.Length > 8 ? Convert.ToInt32(userInfo[8]) : 0,
                            OrganizationId = userInfo.Length > 9 ? Convert.ToInt32(userInfo[9]) : 0
                            // SubscriptionList = _subscriptionService.GetSubscriptionByUser(new Guid(userInfo[2]))                            
                        };

                        // here i need to create a string list that will contain all the service name that are subscribed and approved by admin.                      
                        string permisssions = string.Empty;
                        //foreach (var item in loginUserModel.SubscriptionList)
                        //{
                        //    permisssions += item.ServicePrice.service.ServiceName + ",";
                        //}

                        var moduleList = string.Join(",", _permissioninmoduleService.GetPermissionInModuleByUserId(new Guid(userInfo[2])).Select(x => x.Modules.ModuleName));

                        var permissionList = string.Join(",", _permissioninmoduleService.GetPermissionInModuleByUserId(new Guid(userInfo[2])).Select(x => x.Permisions.PermissionName));


                        loginUserModel.ModuleNames = moduleList.TrimEnd(',');
                        loginUserModel.Permission = permissionList.TrimEnd(',');
                        loginUserModel.FullName = loginUserModel.FullName;
                        ViewBag.LoginUserModel = loginUserModel;
                        base.OnActionExecuting(filterContext);
                    }
                }
                else
                {
                    FormsAuthentication.SignOut();
                    RedirectToAction("Login", "Account", new { @area = "" });
                }
            }
            catch (Exception ex)
            {
                FormsAuthentication.SignOut();
            }
        }
        protected void ChangePassword(ChangePasswordViewModel model)
        {
            var user = _userService.Find(this.loginUserModel.LoginId);
            if (user == null) return;
            user.LoginPassword = hasher.Encrypt(model.Password);
            _userService.Update(user);
        }
        protected void ResetPassword(string Newpassword, string RetypePassword, Guid UserLoginID)
        {
            var user = _userService.Find(UserLoginID);
            if (user == null) return;
            user.LoginPassword = hasher.Encrypt(Newpassword);
            _userService.Update(user);
        }
        [AllowAnonymous]
        public ActionResult ValidateUser(string UserName)
        {
            bool ifEmailExist = false;
            try
            {
                ifEmailExist = _userService.CheckDuplicateUserName(UserName) ? true : false;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult ValidateEmail(string Email)
        {
            bool ifEmailExist = false;
            try
            {
                ifEmailExist = _userService.CheckDuplicateMail(Email) ? true : false;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}