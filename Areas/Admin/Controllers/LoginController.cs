using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using CaptchaMvc.HtmlHelpers;
using PagedList;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using MyOnlineClinic.Web.Helper;
using System.Configuration;
using MyOnlineClinic.Emailer;
using System.Security.Cryptography;
using System.Web.Security;
using MyOnlineClinic.Web.Controllers;


namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    public class LoginController : BaseController
    {
        protected IFormsAuthentication _formsAuth;
        ILoginHelper _loginService;
        IUserService _userService;
        IClinicService _clinicService;
        IClinicUserService _clinicUserService;
        private readonly IOrganizationUserService _organizationUserService;

        private readonly IAdminUserService _adminUserService;
        public LoginController(ILoginHelper loginService, IUserService userService, IFormsAuthentication formsAuth, IAdminUserService adminUserService,
            IClinicService clinicService, IClinicUserService clinicUserService,IOrganizationUserService organizationUserService)
        {
            _loginService = loginService;
            _userService = userService;
            _formsAuth = formsAuth;
            _adminUserService = adminUserService;
            _clinicService = clinicService;
            _clinicUserService = clinicUserService;
            _organizationUserService = organizationUserService;
        }
        //
        // GET: /Admin/Login/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.SuccessMsg = TempData["SuccessMsg"];
            try
            {
                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    if (!string.IsNullOrEmpty(authCookie.Value))
                    {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                        if (ticket.IsPersistent)
                        {
                            if (string.IsNullOrEmpty(returnUrl))
                                return RedirectToAction("Index", "Home");
                            else
                                return Redirect(returnUrl);
                        }
                    }
                }
            }
            catch (CryptographicException cex)
            {
                FormsAuthentication.SignOut();
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Username = StringCipher.Encrypt(model.Username);
                    if (_userService.IsUserLocked(model.Username) == false)
                    {
                        string logo = string.Empty;
                        string organizationName = string.Empty;
                        int organizationId = 0;
                        var user = _userService.Find(model.Username, model.Password);
                        if (user != null)
                        {
                            if (!user.IsAdmin())
                            {
                                if (!user.IsEmailVerified)
                                {
                                    TempData["ErrorMessage"] = "Your email is not verified, Please verify your email.";                                    
                                    return Redirect(Request.UrlReferrer.ToString());
                                }
                                if (!user.IsApproved)
                                {
                                    TempData["ErrorMessage"] = "Your account is in the approval process.";                                 
                                    return Redirect(Request.UrlReferrer.ToString());
                                }
                            }

                            if (user.IsOragnizatinAdmin())
                            {
                                Organization org = new Organization();
                                if (user.Member != null)
                                {
                                    org = ((OrganizationUsers)user.Member).organization;
                                    logo = org.OrganizationLogo;
                                    organizationName = org.OrganizationName;
                                    organizationId = org.OrganizationId;
                                }
                            }
                            else if (user.IsOrgUser())
                            {
                                var OrgId = _organizationUserService.GetOrganizationList().Where(x => x.LoginId == user.LoginId).FirstOrDefault().OrgId;
                                organizationId = OrgId;
                            }

                            string avatar = string.Empty;
                            bool isAdmin = false;
                            avatar = user.Avatar;
                            string memberFullName = string.Empty;
                            memberFullName = user.Member != null ? StringCipher.Decrypt(user.Member.FirstName) + " " + StringCipher.Decrypt(user.Member.SurName) : StringCipher.Decrypt(user.LoginName);
                            if (user.LookUpRoleId == (int)RoleType.ADMIN)
                            {
                                isAdmin = true;
                                var staff = _adminUserService.GetAllAdminUser().Where(x => x.LoginId == user.LoginId && x.MemberId > 1).FirstOrDefault();
                                if (staff != null)
                                {
                                    isAdmin = false;
                                }
                            }

                            string clinicName = string.Empty;
                            var clinicId = 0;

                            if (user.IsClinicAdmin() || user.IsClinicUser())
                            {
                                var clinicUser = _clinicUserService.GetAllClinicUsers().ToList().Where(t => t.LoginId == user.LoginId);

                                if (clinicUser != null && clinicUser.ToList().Count > 0)
                                {
                                    clinicId = clinicUser.FirstOrDefault().ClinicId;

                                    var clinic = _clinicService.GetClinicById(clinicId);

                                    if (clinic != null)
                                    {
                                        clinicName = clinic.ClinicName;
                                        organizationId = clinic.OrganizationId;
                                    }
                                }
                            }
                          

                            _formsAuth.SignIn(memberFullName, false, StringCipher.Decrypt(user.LoginRole.LookUpRoleName), user.LoginId.ToString(), avatar, logo, organizationName, isAdmin, clinicName, clinicId, organizationId);
                            

                            if (user.IsAdmin())
                            {
                                return RedirectToAction("index", "Dashboard", new { @area = "admin" });
                            }
                            else if (user.IsOragnizatinAdmin())
                            {
                                return RedirectToAction("index", "Dashboard", new { @area = "Organizations" });
                            }

                            else if (user.IsClinicAdmin() || user.IsClinicUser())
                            {
                                return RedirectToAction("Index", "Dashboard", new { @area = "Clinic" });
                            }
                            else if (user.IsOrgUser())
                            {

                                return RedirectToAction("index", "Dashboard", new { @area = "Organizations" });
                            }
                          
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid email or password, please try again.";
                            //var count = _userService.FailureCount(StringCipher.Decrypt(model.Username));
                            //if (count >= 3)
                            //{
                            //    _userService.LockUser(StringCipher.Decrypt(model.Username));
                            //}
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Your account has been locked please reset your password again to unlock your account.";                       
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
            }
            catch (CryptographicException cex)
            {
                FormsAuthentication.SignOut();
            }

            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            _formsAuth.SignOut();
            return RedirectToAction("", "Login", new { @area = "Admin" });
        }

        public void SendPassword(string Email, string Password, string UserName)
        {

            var fileUrl = Server.MapPath("~/EmailTemplates/EmailConfirmation.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@UserName", UserName);
            html = html.Replace("@Email", Email);
            html = html.Replace("@Password", Password);
            EmailService objEmail = new EmailService();
            objEmail.SendEmail("Login Details", html, Email, "info@myonlineclinic.com.au");

        }

    }
}