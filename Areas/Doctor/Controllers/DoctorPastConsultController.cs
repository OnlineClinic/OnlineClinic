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
    public class DoctorPastConsultController : BaseController
    {
         private readonly IAppointmentService _AppointmentService;
        private readonly IUserService _userService;
        public DoctorPastConsultController(IAppointmentService AppointmentService, IUserService userService)
        {
            _AppointmentService = AppointmentService;
            _userService = userService;
        }
        //
        // GET: /Doctor/DoctorPastConsult/
        public ActionResult Index()
        {
            var commonUtilities = new CommonUtilities();

            var Pastconsult = _AppointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
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
                IsSoapNotesAdded = _AppointmentService.CheckSoapNotes(x.AppointmentId) ? true : false

            }).Where(x => x.AppointmentStatus != (int)AppointmentStatus.Pending && x.AppointmentStatus != (int)AppointmentStatus.Approved || (x.IsSoapNotesAdded == true && x.AppointmentStatus == (int)AppointmentStatus.Completed)).ToList();
            // x.DoctorLoginId == base.loginUserModel.LoginId &&
            var a = (int)AppointmentStatus.Pending;


            return PartialView("_doctorPastConsult", Pastconsult);
        }
        public JsonResult SosapNotes(int AppointmentId)
        {
            var SOap = _AppointmentService.GetSoapId(AppointmentId);
            return Json(SOap, JsonRequestBehavior.AllowGet);
        }
	}
}