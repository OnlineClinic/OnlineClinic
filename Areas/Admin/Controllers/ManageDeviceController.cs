using MyOnlineClinic.Emailer;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
using System.Globalization;
using System.Net;
using PayPal.Api;

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    public class ManageDeviceController : BaseController
    {
        private readonly IPatientService _patientServcie;
        private readonly ILoginHelper _loginHelper;
        private readonly IUserService _userService;
        public ManageDeviceController(IPatientService patientService, ILoginHelper loginHelper, IUserService userService)
        {
            _patientServcie = patientService;
            _loginHelper = loginHelper;
            _userService = userService;
        }
        //By dheeraj
        // GET: /Admin/ManageDevice/
        public ActionResult Index()
        {
            var timeZoneString = _userService.Find(base.loginUserModel.LoginId).TimeZoneString;
            var Devices = _patientServcie.GetDeviceList().Select(x => new DeviceViewModel
            {
                DeviceName = StringCipher.Decrypt(x.DeviceName),
                DeviceNumber = StringCipher.Decrypt(x.DeviceNumber),
                DeviceDetails = StringCipher.Decrypt(x.DeviceDetails),
                ProfilePic = x.DeviceImage == null ? "" : x.DeviceImage,
                PatientDeviceId = x.PatientDeviceId,
                CreatedDateInString = x.CreatedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.CreatedDate), timeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(timeZoneString, true)
                //PatientName = StringCipher.Decrypt(_userService.GetPatientById(_patientServcie.GetAssigenDeviceList().Where(y => x.PatientDeviceId == x.PatientDeviceId).FirstOrDefault().PatientId).FirstOrDefault().FirstName),
                //AssigenDate = (DateTime)_userService.GetPatientById(_patientServcie.GetAssigenDeviceList().Where(y => x.PatientDeviceId == x.PatientDeviceId).FirstOrDefault().PatientId).FirstOrDefault().CreatedDate


            }).ToList();



            return View(Devices);
        }
        [HttpPost]
        public ActionResult Index(SearchParametersViewModel searchModel)
        {
            var timeZoneString = _userService.Find(base.loginUserModel.LoginId).TimeZoneString;

            var deviced = _patientServcie.GetDeviceList();
            var Devices = _patientServcie.GetDeviceList().Select(x => new DeviceViewModel
            {
                DeviceName = StringCipher.Decrypt(x.DeviceName),
                DeviceNumber = StringCipher.Decrypt(x.DeviceNumber),
                DeviceDetails = StringCipher.Decrypt(x.DeviceDetails),
                ProfilePic = x.DeviceImage,
                PatientDeviceId = x.PatientDeviceId,
                CreatedDate = x.CreatedDate,
                PatientName = StringCipher.Decrypt(_userService.GetPatientById(_patientServcie.GetAssigenDeviceList().Where(y => x.PatientDeviceId == x.PatientDeviceId).FirstOrDefault().PatientId).FirstOrDefault().FirstName),
                // AssigenDate = (DateTime)_userService.GetPatientById(_patientServcie.GetAssigenDeviceList().Where(y => x.PatientDeviceId == x.PatientDeviceId).FirstOrDefault().PatientId).FirstOrDefault().CreatedDate,
                CreatedDateInString = x.CreatedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.CreatedDate), timeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(timeZoneString, true)
            }).ToList();

            Devices = Devices.Where(x =>
                  ((searchModel.DeviceName != null && x.DeviceName.IndexOf(searchModel.DeviceName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.DeviceName == null)
                   &&
                  ((searchModel.DeviceNumber != null && x.DeviceNumber.Contains(searchModel.DeviceNumber) || searchModel.DeviceNumber == null))
                  &&
                  ((searchModel.PatientName != null && x.PatientName.Contains(searchModel.PatientName) || searchModel.PatientName == null))
                 &&
                  ((searchModel.AssignDate != null && x.AssigenDate.Date == Convert.ToDateTime(searchModel.AssignDate).Date) || searchModel.AssignDate == null)

                ).ToList();
            return View(Devices);
        }

        [HttpGet]
        public ActionResult AssignDevice()
        {
            DeviceViewModel model = new DeviceViewModel();
            var AssigenDevices = _patientServcie.GetAssigenDeviceList().Select(x => new DeviceViewModel
            {
                AssignDeviceId = x.DeviceId,
                AssignPatientId = x.PatientId,
                AssigenDate = x.AssigenDate,
                PatientName = StringCipher.Decrypt(_userService.GetPatById(x.PatientId).FirstName),
                PatientSurName = StringCipher.Decrypt(_userService.GetPatById(x.PatientId).SurName),
                PatientEmail = StringCipher.Decrypt(_userService.GetPatById(x.PatientId).Email),
                DeviceName = StringCipher.Decrypt(_patientServcie.GetAssigendDeviceById(x.DeviceId).DeviceName),
                AssgienddeviceNumber = StringCipher.Decrypt(_patientServcie.GetAssigendDeviceById(x.DeviceId).DeviceNumber),
                deviceLogo = _patientServcie.GetAssigendDeviceById(x.DeviceId).DeviceImage
            }).ToList();


            return View(AssigenDevices);
        }
        // GET: /Admin/ManageDevice/Add
        [HttpGet]
        public ActionResult Add(int? id)
        {
            DeviceViewModel model = new DeviceViewModel();
            if (id.HasValue)
            {
                var Cmodel = _patientServcie.GetDeviceById(Convert.ToInt32(id)).Select(x => new DeviceViewModel
                {
                    DeviceName = StringCipher.Decrypt(x.DeviceName),
                    DeviceNumber = StringCipher.Decrypt(x.DeviceNumber),
                    DeviceDetails = StringCipher.Decrypt(x.DeviceDetails),
                    ProfilePic = x.DeviceImage,
                    PatientDeviceId = x.PatientDeviceId

                }).FirstOrDefault();

                return View(Cmodel);
            }
            else
            {
                return View(model);
            }

        }

        // Post: /Admin/ManageDevice/Add
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Add(DeviceViewModel model, HttpPostedFileBase ProfilePic)
        {
            PatientDevice UpdateDevice = new PatientDevice();
            var NumberExists = _patientServcie.CheckDuplicateDeviceNumber(StringCipher.Encrypt(model.DeviceNumber));
            if (NumberExists)
            {
                ModelState.AddModelError("DeviceNumber", "Device Number already exists.");
                DeviceViewModel models = new DeviceViewModel();
                return View(models);
            }
            UpdateDevice = model.EditDeviceData(model);
            if (model.PatientDeviceId > 0)
            {
                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                }
                UpdateDevice.ModifiedBy = base.loginUserModel.LoginId;
                if (!string.IsNullOrEmpty(model.ProfilePic))
                {
                    UpdateDevice.DeviceImage = model.ProfilePic;
                }
                _patientServcie.UpdateDevice(UpdateDevice);

                return RedirectToAction("Index");
            }
            else
            {
                PatientDevice device = new PatientDevice();

                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                }
                var Device = model.AddDeviceData(device);
                device.CreatedBy = base.loginUserModel.LoginId;
                _patientServcie.AddDevice(Device);

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Assign()
        {
            DeviceViewModel model = new DeviceViewModel();
            List<Patient> Patient = _userService.GetPatientList().ToList();

            model.PatientList = Patient;

            for (int i = 0; i < model.PatientList.Count; i++)
            {
                model.PatientList[i].FirstName = StringCipher.Decrypt(model.PatientList[i].FirstName);
                model.PatientList[i].MiddleName = StringCipher.Decrypt(model.PatientList[i].MiddleName);
                model.PatientList[i].SurName = StringCipher.Decrypt(model.PatientList[i].SurName);
                model.PatientList[i].Address1 = StringCipher.Decrypt(model.PatientList[i].Address1);
                model.PatientList[i].Address2 = StringCipher.Decrypt(model.PatientList[i].Address2);

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Assign(DeviceViewModel model)
        {
            AssigenDevice Device = new AssigenDevice();
            Device.PatientId = Convert.ToInt32(model.PatientId);
            Device.DeviceId = Convert.ToInt32(model.DeviceId);
            Device.AssigenDate = model.AssigenDate;
            _patientServcie.AddAssigneDevice(Device);

            return View(model);
        }

        [HttpGet]
        public JsonResult GetPatientList(string query = "")
        {
            query = query.Trim();
            List<Patient> PatientList = string.IsNullOrEmpty(query) ? _userService.GetPatientList() : _userService.GetPatientList().Where(x => x.FirstName.Contains(query)
               || x.SurName.Contains(query) || x.FaxNumber.Contains(query)).ToList();
            for (int i = 0; i < PatientList.Count; i++)
            {
                PatientList[i].FirstName = StringCipher.Decrypt(PatientList[i].FirstName);
                PatientList[i].Email = StringCipher.Decrypt(PatientList[i].Email);
            }
            return Json(PatientList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDeviceList(string query = "")
        {
            query = query.Trim();
            List<PatientDevice> DeviceList = string.IsNullOrEmpty(query) ? _patientServcie.GetDeviceList() : _patientServcie.GetDeviceList()
                 .Where(x => x.DeviceName.Contains(query)).ToList();
            for (int i = 0; i < DeviceList.Count; i++)
            {
                DeviceList[i].DeviceNumber = StringCipher.Decrypt(DeviceList[i].DeviceNumber);
                DeviceList[i].DeviceName = StringCipher.Decrypt(DeviceList[i].DeviceName);
            }
            return Json(DeviceList, JsonRequestBehavior.AllowGet);
        }
    }
}