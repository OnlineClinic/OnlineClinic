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
    public class DoctorMessageController : BaseController
    {
        private readonly IAppointmentService _AppointmentService;
        private readonly IUserService _userService;
        private readonly IReportProblemService _reportProblemService;
        private readonly ICommunication _communication;
        public DoctorMessageController(IAppointmentService AppointmentService, IUserService userService, IReportProblemService reportProblemService, ICommunication communication)
        {
            _AppointmentService = AppointmentService;
            _userService = userService;
            _reportProblemService = reportProblemService;
            _communication = communication;
        }
        //
        // GET: /Doctor/DoctorMessage/
        public ActionResult Index()
        {
            var commonUtilities = new CommonUtilities();
            AppointmentViewModel model = new AppointmentViewModel();
            var Message = _AppointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
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
                DoctorLoginId=x.DoctorLoginId

            }).Where(x => x.AppointmentStatus == (int)AppointmentStatus.Pending && x.DoctorLoginId == base.loginUserModel.LoginId).ToList();



            var Report = _reportProblemService.GetRportsProblem().Where(x => x.SenderId == base.loginUserModel.LoginId).Select(x => new ReportProblemViewModel
            {
                ReportProblemSubject = x.ReportProblemSubject,
                SenderId = x.SenderId,
                RecevierId = x.RecevierId,
                ReportProblemId = x.ReportProblemId,
                IsRead = x.IsRead,
                IsCompleted = x.IsCompleted

            }).ToList();

            var Contact = _communication.GetContactusList().Where(x=>x.SenderId==base.loginUserModel.LoginId).Select(x => new CommunicationViewModel
            {
                ContactUsName = x.Name,
                ContactUsEmail = x.Email,
                ContactUsMessage = x.Message,
                ContactUsCreatedDate = x.CreatedDate,
                Subject = x.Subject,
                RoleType = x.RoleType,
                contactid = x.ContectUsId,
                SenderId = x.SenderId,
                RecevierId = x.RecevierId

                // UserType = StringCipher.Decrypt(x.UserType)
            }).ToList();



            model.AppointmentList = Message;
            model.ReportProblem = Report;
            model.ContactusList = Contact;

            return View(model);
        }

        //
        [HttpPost]
        public ActionResult Accept(AppointmentViewModel model)
        {
            AppointmentTracking Track = new AppointmentTracking();
            var id = model.hdnAppId;
            var Appointment = _AppointmentService.GetAppointmentById(Convert.ToInt32(id));
            Track.AppointmentId = Convert.ToInt32(id);
            Track.AppointmentStatus = Appointment.AppointmentStatus;
            Track.AppointmentType = Appointment.AppointmentType;
            _AppointmentService.Add(Track);
            Appointment UpdateStatus = new Appointment();
            Appointment.AppointmentId = id;
            Appointment.AppointmentStatus = (int)AppointmentStatus.Approved;
            _AppointmentService.Update(Appointment);
            ModelState.AddModelError("Status", "You accept the cosult.Check the status of consult in my future consults in your dashboard");
            return Redirect(Request.UrlReferrer.ToString());
        }

        //
        [HttpPost]
        public ActionResult Reject(AppointmentViewModel model)
        {
            AppointmentTracking Track = new AppointmentTracking();
            var id = model.AppId;
            var Appointment = _AppointmentService.GetAppointmentById(Convert.ToInt32(id));
            Track.AppointmentId = Convert.ToInt32(id);
            Track.AppointmentStatus = Appointment.AppointmentStatus;
            Track.AppointmentType = Appointment.AppointmentType;
            _AppointmentService.Add(Track);
            Appointment UpdateStatus = new Appointment();
            Appointment.AppointmentId = id;
            Appointment.AppointmentStatus = (int)AppointmentStatus.Rejected;
            _AppointmentService.Update(Appointment);
            ModelState.AddModelError("Reject", "You reject the cosult.");
            return Redirect(Request.UrlReferrer.ToString());

        }


        public JsonResult Reply(string reply, int id)
        {
            var ProblemReply = new ReportProblemReply()
            {
                Message = reply,
                ReplyById = base.loginUserModel.LoginId,
                RoleType = (int)RoleType.Doctor,
                ReportProblemId = id,
                IsRead = false,
                IsCompleted = false

            };
            if (ProblemReply != null)
            {
                _reportProblemService.Add(ProblemReply);
            }
            return Json(ProblemReply, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReply(int Replyid)
        {
            var reply = _reportProblemService.GetRportsProblemReply().Select(x => new ReportProblemViewModel
                {
                    Message = x.Message,
                    ReportProblemReplyId = x.ReportProblemReplyId,
                    ReplyById = x.ReplyById,
                    RoleType = x.RoleType

                }).Where(x => x.ReportProblemReplyId == Replyid).ToList();

            return Json(reply, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllReply(int problemid)
        {
            var reply = _reportProblemService.GetRportsProblemReply().Select(x => new ReportProblemViewModel
            {
                Message = x.Message,
                ReportProblemReplyId = x.ReportProblemReplyId,
                ReplyById = x.ReplyById,
                RoleType = x.RoleType,
                ReportProblemId = x.ReportProblemId


            }).Where(x => x.ReportProblemId == problemid).ToList();

            return Json(reply, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContactReply(string reply, int id)
        {
            var ContactReply = new ContactReply()
            {
                Message = reply,
                ReplyById = base.loginUserModel.LoginId,
                RoleType = (int)RoleType.Doctor,
                ContactUsId = id,
                IsRead = false,
                IsCompleted = false

            };
            if (ContactReply != null)
            {
                _communication.Add(ContactReply);
            }
            return Json(ContactReply, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContactReply(int Replyid)
        {
            var reply = _communication.GetContactusReplyList().Select(x => new CommunicationViewModel
            {
                Message = x.Message,
                ContactUsId = x.ContactUsId,
                ContactReplyId = x.ContactReplyId,
                ReplyById = x.ReplyById,
                // ReplyById = x.ReplyById,
                RoleType = x.RoleType


            }).Where(x => x.ContactReplyId == Replyid).ToList();

            return Json(reply, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContactAllReply(int problemid)
        {
            var reply = _communication.GetContactusReplyList().Select(x => new CommunicationViewModel
            {
                Message = x.Message,
                ContactUsId = x.ContactUsId,
                ContactReplyId = x.ContactReplyId,
                ReplyById = x.ReplyById,
                // ReplyById = x.ReplyById,
                RoleType = x.RoleType


            }).Where(x => x.ContactUsId == problemid).ToList();

            return Json(reply, JsonRequestBehavior.AllowGet);
        }
    }
}
