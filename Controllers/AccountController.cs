using Omu.Encrypto;
using MyOnlineClinic.Emailer;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Models;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyOnlineClinic.Web.Helper;

namespace MyOnlineClinic.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        protected IFormsAuthentication _formsAuth;

        protected ICountryService _countryService;

        protected ICityService _cityservice;

        protected IUserService _userService;
        private readonly IAdminUserService _adminUserService;
        private readonly IOrganizationUserService _organizationUserService;
        private readonly IClinicUserService _clinicUserService;

        private IHasher hasher = new Hasher();


        public AccountController(IFormsAuthentication formsAuth, ICountryService countryService,
            ICityService cityService, IUserService userService, IAdminUserService adminUserService,
            IOrganizationUserService organizationUserService, IClinicUserService clinicUserService)
        {
            _formsAuth = formsAuth;
            _countryService = countryService;
            _cityservice = cityService;
            _userService = userService;
            _adminUserService = adminUserService;
            _organizationUserService = organizationUserService;
            _clinicUserService = clinicUserService;
            hasher.SaltSize = 10;
        }

        [AllowAnonymous]
        public ActionResult ValidateEmail(string Email)
        {
            bool ifEmailExist = false;
            try
            {
                //ifEmailExist = _userService.CheckDuplicateMail(Email) ? true : false;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult ValidateEmail2(string Email2)
        {
            bool ifEmailExist = false;
            try
            {
                //ifEmailExist = _userService.CheckDuplicateMail(Email2) ? true : false;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult ValidateUser(string UserName)
        {
            bool ifEmailExist = false;
            try
            {
                //ifEmailExist = _userService.CheckDuplicateUserName(UserName) ? true : false;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
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
                        int clinicId = 0;
                        string clinicName = string.Empty;

                        var user = _userService.Find(model.Username, model.Password);
                        if (user != null)
                        {
                            if (!user.IsAdmin())
                            {
                                if (!user.IsEmailVerified)
                                {
                                    TempData["ErrorMessage"] = "Your email is not verified, Please verify your email.";
                                    return View(model);
                                }
                                if (!user.IsApproved)
                                {
                                    TempData["ErrorMessage"] = "Your account is in the approval process.";
                                    return View(model);
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

                            if (user.LookUpRoleId == (int)RoleType.OrganizationAdmin)
                            {
                                isAdmin = true;
                                var staff = _organizationUserService.GetOrganizationList().Where(x => x.LoginId == user.LoginId && x.MemberId > 2).FirstOrDefault();
                                if (staff != null)
                                {
                                    isAdmin = false;
                                }
                            }

                            if (user.LookUpRoleId == (int)RoleType.ClinicAdmin || user.IsClinicUser())
                            {
                                isAdmin = true;
                                var staff = _clinicUserService.GetAllClinicUsers().Where(x => x.LoginId == user.LoginId && x.MemberId > 2).FirstOrDefault();

                                clinicId = staff.ClinicId;
                                clinicName = staff.Clinic.ClinicName;
                                organizationId = staff.OrgId;

                                if (staff != null)
                                {
                                    isAdmin = false;
                                }
                            }

                            if (user.IsOrgUser())
                            {
                                var OrgId = _organizationUserService.GetOrganizationList().Where(x => x.LoginId == user.LoginId).FirstOrDefault().OrgId;
                                organizationId = OrgId;
                            }

                            _formsAuth.SignIn(StringCipher.Decrypt(memberFullName), false, user.LoginRole.LookUpRoleName, user.LoginId.ToString(), avatar, logo, organizationName, isAdmin, clinicName, clinicId, organizationId);

                            if (user.IsAdmin())
                            {
                                return RedirectToAction("index", "Dashboard", new { @area = "admin" });
                            }
                            else if (user.IsOragnizatinAdmin())
                            {
                                return RedirectToAction("index", "Dashboard", new { @area = "Organizations" });
                            }
                            else if (user.IsDoctor())
                            {
                                //return RedirectToAction("index", "Doctor", new { @area = "Doctor" });
                                return RedirectToAction("index", "Dashboard", new { @area = "Doctor" });
                                //return Content("<script language='javascript' type='text/javascript'>alert('Correct Doctor');</script>");
                            }
                            else if (user.IsPatient())
                            {
                                return RedirectToAction("PatientDashboard", "Home");
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
                            var count = _userService.FailureCount(model.Username);
                            if (count >= 3)
                            {
                                _userService.LockUser(model.Username);
                            }
                            //return Content("<script language='javascript' type='text/javascript'>alert('Invalid email or password, please try again!');</script>");
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Your account has been locked please reset your password again to unlock your account.";
                        //return Content("<script language='javascript' type='text/javascript'>alert('Invalid email or password, please try again!');</script>");
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (CryptographicException cex)
            {
                FormsAuthentication.SignOut();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/RegisterPatient---Create By dheeraj
        [AllowAnonymous]
        public ActionResult RegisterPatient()
        {
            RegisterViewModel model = new RegisterViewModel();
            //model = BindDropDown(model);
            return View(model);
        }

        //POST: /Account/RegisterPatient....Create by dheeraj
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult RegisterPatient(RegisterViewModel model)
        {
            var userExists = _userService.CheckDuplicateUserName(model.UserName);
            if (userExists)
            {
                ModelState.AddModelError("UserName", "Username already exists.");
            }
            var userEmailExists = _userService.CheckDuplicateMail(model.Email);
            if (userEmailExists)
            {
                ModelState.AddModelError("Email", "User with this email id already exists.");
            }
            //model = BindDropDown(model);
            //if (ModelState.IsValid)
            //{
            BaseMember profile = null;
            model.RoleId = RoleType.Patient;
            profile = model.GetPatient();
            var user = model.GetLogin(profile, model.Email);
            _userService.Add(user);
            string activationToken = user.ActivationToken.ToString();
            string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
            var fileUrl = Server.MapPath("~/EmailTemplates/PatientRegister.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@@ActivationLink", link);
            EmailService objEmail = new EmailService();

            Task.Factory.StartNew(() => { 
                objEmail.SendEmail("User Registration", html, model.Email, "tma@myonlineclinic.com.au");
            });
            

            return RedirectToAction("Login");
            // }
            //}
            //}

            return View(model);
        }

        //GET: /Account/RegisterDoctor---Create By dheeraj
        [AllowAnonymous]
        public ActionResult RegisterDoctor()
        {
            RegisterViewModel model = new RegisterViewModel();
            //model = BindDropDown(model);
            return View(model);
        }


        //POST: /Account/RegisterDoctor....Create by dheeraj
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult RegisterDoctor(RegisterViewModel model)
        {
            var userExists = _userService.CheckDuplicateUserName(model.UserName);
            if (userExists)
            {
                ModelState.AddModelError("UserName", "Username already exists.");
            }
            var userEmailExists = _userService.CheckDuplicateMail(model.Email);
            if (userEmailExists)
            {
                ModelState.AddModelError("Email", "User with this email id already exists.");
            }
            
            BaseMember profile = null;
            model.RoleId = RoleType.Doctor;
            profile = model.GetDoctor();
            var user = model.GetLogin(profile, model.Email);
            _userService.Add(user);
            string activationToken = user.ActivationToken.ToString();
            string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
            var fileUrl = Server.MapPath("~/EmailTemplates/PatientRegister.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@@ActivationLink", link);
            EmailService objEmail = new EmailService();

            //To-Do on all places where registration is taking place
            Task.Factory.StartNew(() =>
            {
                objEmail.SendEmail("User Registration", html, model.Email, "tma@myonlineclinic.com.au");
            });
            return RedirectToAction("Login"); 
            //return View(model);
        }


        [AllowAnonymous]
        public ActionResult VerifyEmail(string id)
        {

            _userService.VerifyEmail(id);

            return RedirectToAction("Login", "Account", new { returnUrl = "" });
        }

        [AllowAnonymous]
        public ActionResult VerifyDocEmail(string id)
        {

            _userService.VerifyDoctorEmail(id);

            return RedirectToAction("Login", "Account", new { returnUrl = "" });
        }

        [AllowAnonymous]
        public ActionResult Activate(string id)
        {
            var loginUser = _userService.ActivateUserEmail(id);
            if (loginUser != null)
            {
                loginUser.IsEmailVerified = true;
                var password = RandomPassword.Generate(6, 8);
                loginUser.LoginPassword = hasher.Encrypt(password);
                loginUser.IsApproved = true;
                _userService.Update(loginUser);

                var fileUrl = Server.MapPath("~/EmailTemplates/ActivationTemplate.html");
                string html = System.IO.File.ReadAllText(fileUrl);
                html = html.Replace("@@Password", password);
                EmailService objEmail = new EmailService();

                Task.Factory.StartNew(() =>
                {
                    objEmail.SendEmail("Your Password", html, StringCipher.Decrypt(loginUser.Email), Common.fromEmailAddress);
                });
                return RedirectToAction("Login", "Account", new { returnUrl = "" });
            }
            return View();
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

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public JsonResult doesUserNameExist(string Email)
        {
            var user = _userService.CheckDuplicateMail(StringCipher.Encrypt(Email));

            return Json(user == false);

        }

        //[HttpPost]
        //[AllowAnonymous]
        //private ActionResult EncryptionAndDecryption(string value, string type)
        //{
        //    var newvalue=string.Empty;
        //    if (type == "E")
        //    {
        //        newvalue = StringCipher.Encrypt(value);
        //    }
        //    else
        //    {
        //        newvalue = StringCipher.Decrypt(value);            
        //    }
        //    Response.Write(newvalue);
        //    return View();
        //}

    }
}