using MyOnlineClinic.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class DashboardController : BaseController
    {

        ILoginHelper _loginService;
        IUserService _userService;
        private readonly IOrganizationService _organiztionService;
        private readonly IClinicService _clinicService;
        private readonly IPermissionInModuleService _permissioninmoduleService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        public DashboardController(ILoginHelper loginService, IUserService userService, IOrganizationService organiztionService,
           IClinicService clinicService, IPermissionInModuleService permissioninmoduleService, IDoctorService doctorService, IAppointmentService appointmentService)
        {
            _loginService = loginService;
            _userService = userService;
            _organiztionService = organiztionService;
            _clinicService = clinicService;
            _permissioninmoduleService = permissioninmoduleService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }
        public ActionResult Profile()
        {
            Login model = _userService.Find(base.loginUserModel.LoginId);
            UserViewModel userModel = new UserViewModel(model);
            userModel.FirstName = StringCipher.Decrypt(userModel.FirstName);
            userModel.MiddleName = StringCipher.Decrypt(userModel.MiddleName);
            userModel.SurName = StringCipher.Decrypt(userModel.SurName);
            userModel.Address1 = StringCipher.Decrypt(userModel.Address2);
            userModel.PostCode = StringCipher.Decrypt(userModel.PostCode);
            userModel.FaxNumber = StringCipher.Decrypt(userModel.FaxNumber);
            userModel.EmailAddress = StringCipher.Decrypt(userModel.EmailAddress);
            userModel.Address2 = StringCipher.Decrypt(userModel.Address2);
            userModel.LoginName = StringCipher.Decrypt(userModel.LoginName);
            userModel.PhoneNumber = StringCipher.Decrypt(userModel.PhoneNumber);
            userModel.MobileNumber = StringCipher.Decrypt(userModel.MobileNumber);
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
            DashboardViewModel model = new DashboardViewModel();
            var moduleList = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).ToList();
            var listDistinct = moduleList.GroupBy(i => i.ModuleId, (key, group) => group.First()).ToList();
            var org = _organiztionService.GetOrganizationById(base.loginUserModel.OrganizationId);
            var orgId = org.OrganizationId;
            var clinicCount = _clinicService.ClinicOfOrganizationCount(orgId);
            var patientCount = _userService.PatientOfOrganizationCount(orgId);
            var doctorCount = _doctorService.DoctorOfOrganizationCount(orgId);
            var Patient = _userService.GetPatientList().Where(x => x.OrgId == orgId).FirstOrDefault();
            model.AppointmentCount = 0;

            if (Patient != null)
            {
                var appointmentCount = _appointmentService.AppointmentofOrganizationCount(Patient.LoginId);
                model.AppointmentCount = appointmentCount;
            }

            model.DoctorCount = doctorCount;
            model.ClinicCount = clinicCount;
            model.PatientCount = patientCount;

            model.ModuleList = listDistinct;
            return View(model);
        }

        // GET: /Admin/Login/
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
