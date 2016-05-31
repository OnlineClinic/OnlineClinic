using MyOnlineClinic.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Web.Models;

namespace MyOnlineClinic.Web.Areas.Clinic.Controllers
{
    public class DashboardController : BaseController
    {

        ILoginHelper _loginService;
        IUserService _userService;
        private readonly IOrganizationService _organiztionService;
        private readonly IClinicService _clinicService;
        private readonly IPermissionInModuleService _permissioninmoduleService;
        private readonly IDoctorService _doctorService;

        public DashboardController(ILoginHelper loginService, IUserService userService, IOrganizationService organiztionService,
           IClinicService clinicService, IPermissionInModuleService permissioninmoduleService,IDoctorService doctorService)
        {
            _loginService = loginService;
            _userService = userService;
            _organiztionService = organiztionService;
            _clinicService = clinicService;
            _permissioninmoduleService = permissioninmoduleService;
            _doctorService = doctorService;
        }
        public ActionResult Profile()
        {
            Login model = _userService.Find(base.loginUserModel.LoginId);
            UserViewModel userModel = new UserViewModel(model);
            userModel.FirstName = StringCipher.Decrypt(userModel.FirstName);
            userModel.MiddleName = StringCipher.Decrypt(userModel.MiddleName);
            userModel.SurName = StringCipher.Decrypt(userModel.SurName);
            userModel.Address1 = StringCipher.Decrypt(userModel.Address1);
            userModel.Address2 = StringCipher.Decrypt(userModel.Address2);
            userModel.LoginName = StringCipher.Decrypt(userModel.LoginName);
            userModel.EmailAddress = StringCipher.Decrypt(userModel.EmailAddress);
            userModel.PhoneNumber = StringCipher.Decrypt(userModel.PhoneNumber);
            userModel.MobileNumber = StringCipher.Decrypt(userModel.MobileNumber);
            userModel.FaxNumber = StringCipher.Decrypt(userModel.FaxNumber);
            userModel.PostCode = StringCipher.Decrypt(userModel.PostCode);
            return View(userModel);
        }
        [HttpPost]
        public ActionResult Profile(UserViewModel model, HttpPostedFileBase ProfilePic)
        {
            _loginService.UpdateProfile(model, ProfilePic, base.loginUserModel.LoginId);
            return View(model);
        }
        public ActionResult Index()
        {
            string[] arr = new string[2] { "Manage Organizations", "Manage Clinics" };

            DashboardViewModel model = new DashboardViewModel();
            var moduleList = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).ToList();
            var listDistinct = moduleList.GroupBy(i => i.ModuleId, (key, group) => group.First()).ToList().Where(t => !arr.Contains(t.Modules.ModuleName));
            model.ModuleList = listDistinct.ToList();
            //var ClinicId = _clinicService.GetClinicByLoginId(base.loginUserModel.LoginId).ClinicId;
            var DoctorCount = _doctorService.DoctorOfClinicCount(base.loginUserModel.ClinicId);
            model.DoctorCount = DoctorCount;
            model.PatientCount = _doctorService.PatientOfClinicCount(base.loginUserModel.ClinicId);
            
            return View(model);
        }

        [HttpGet]
        public ActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordChange(ChangePasswordViewModel Cmodel)
        {
            if (ModelState.IsValid)
            {
                var loginid = _userService.Find(base.loginUserModel.LoginId);
                var oldpassword = _userService.Find(loginid.LoginName, Cmodel.OldPassword);
                if (oldpassword == null)
                {
                    ModelState.AddModelError("OldPassword", "Current Password Does not Exsit");
                    return View(Cmodel);
                }
                ChangePassword(Cmodel);
                ModelState.AddModelError("Done", "Password Change Successfully..!!!");
            }

            return View(Cmodel);
        }
    }
}
