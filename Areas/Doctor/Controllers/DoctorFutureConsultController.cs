using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Controllers;
using System.IO;
using System.Drawing;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Emailer;
using System.Configuration;
using System.Collections.ObjectModel;

namespace MyOnlineClinic.Web.Areas.Doctor.Controllers
{
    public class DoctorFutureConsultController : BaseController
    {
        private readonly IAppointmentService _AppointmentService;
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly IClinicalService _clinicalService;

        public DoctorFutureConsultController(IAppointmentService AppointmentService, IUserService userService, ICountryService countryService, IClinicalService clinicalService)
        {
            _AppointmentService = AppointmentService;
            _userService = userService;
            _countryService = countryService;
            _clinicalService = clinicalService;
         
        }
        // GET: /Doctor/DoctorFutureConsult/
        public ActionResult Index()
        {
            var commonUtilities = new CommonUtilities();

            var FutureConsult = _AppointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
            {
                AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                AppointmentId = x.AppointmentId,
                AppointmentTypeText = (x.AppointmentType == (int)AppointmentType.VideoConsult ?
                                      commonUtilities.GetDisplayName(AppointmentType.VideoConsult) :
                                      x.AppointmentType == (int)AppointmentType.ClinicVisit ?
                                      commonUtilities.GetDisplayName(AppointmentType.ClinicVisit) :
                                      commonUtilities.GetDisplayName(AppointmentType.HomeVisit)),
                PatientName = StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.FirstName) + " " + StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.SurName),
                AppointmentDateUtc = x.AppointmentDateUtc,
                AppointmentStatus = x.AppointmentStatus,
                Address1 = StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.Address1),
                Address2 = StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.Address2),
                Country = _userService.Find(x.PatientLoginId).Member.CountryId,
                CountryName = StringCipher.Decrypt(_countryService.GetCountryById(_userService.Find(x.PatientLoginId).Member.CountryId).CountryName),
                AppointmentStatusName = (x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Pending ?
                                                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Pending) :
                                                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Approved ?
                                                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Approved) :
                                                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Completed)),
                MemberId = _userService.Find(x.PatientLoginId).Member.MemberId,
                IsSoapNotesAdded = _AppointmentService.CheckSoapNotes(x.AppointmentId) ? true : false,
                DoctorLoginId=x.DoctorLoginId

            }).Where(x=>  x.DoctorLoginId==base.loginUserModel.LoginId && x.AppointmentStatus == (int)AppointmentStatus.Approved  || (x.IsSoapNotesAdded==false && x.AppointmentStatus == (int)AppointmentStatus.Completed)).ToList(); // x => x.DoctorLoginId==base.loginUserModel.LoginId &&


            return PartialView("_DoctorFutureConsult", FutureConsult);

        }
        [HttpPost]
        public JsonResult UpdateStatus(int StatusId, int appId)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            
            var Appointment = _AppointmentService.GetAppointmentById(Convert.ToInt32(appId));
            Appointment.AppointmentId = appId;
            Appointment.AppointmentStatus = StatusId;
            _AppointmentService.Update(Appointment);
            jsonObject.Add("success", true);
            jsonObject.Add("message", "Status Update Successfully");
            return Json(jsonObject, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveNewConsult(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            NewConsultation SaveConsult = new NewConsultation();
            SaveConsult.PatientId = model.PatientId;
            SaveConsult.DoctorId = base.loginUserModel.LoginId;
            SaveConsult.ReasonForContact = model.ReasonForContact;
            SaveConsult.Plans = model.Plans;
            SaveConsult.Objective = model.Objective;
            SaveConsult.Assessment = model.Assessment;
            SaveConsult.Subjective = model.Subjective;
            SaveConsult.DiagnosisId = 1;
            SaveConsult.AppointmentId =model.AppointmentId;
            _clinicalService.AddNewConsult(SaveConsult);
            jsonObject.Add("success", true);
            jsonObject.Add("message", "Soap Notes Added Successfully");
            return Json(jsonObject, JsonRequestBehavior.AllowGet);
     
        }
    }
}