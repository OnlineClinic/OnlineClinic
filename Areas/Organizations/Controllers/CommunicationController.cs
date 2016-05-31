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

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class CommunicationController : BaseController
    {
        private readonly ICommunication _Communication;
        private readonly IReportProblemService _reportProblemService;
        private readonly IDoctorService _doctorService;
        private readonly ILoginHelper _loginHelper;
        private readonly IUserService _userService;
        public CommunicationController(ICommunication Communication, IReportProblemService reportproeblemService, IDoctorService doctorService, ILoginHelper loginHelper, IUserService userService)
        {
            _Communication = Communication;
            _reportProblemService = reportproeblemService;
            _doctorService = doctorService;
            _loginHelper = loginHelper;
            _userService = userService;
        }
        //
        // GET: /Admin/Communication/
        public ActionResult Index(FormCollection Collection)
        {
            CommunicationViewModel model = new CommunicationViewModel();
            model.Source = Collection.Get("All");
            var type = Collection.Get("Type");
            if (type == "All")
            {
                var Equipment = _Communication.GetEquipmentList().Select(x => new CommunicationViewModel
                {
                    EquipmentName = StringCipher.Decrypt(x.EquipmentName),
                    UserName = StringCipher.Decrypt(x.UserName),
                    Email = StringCipher.Decrypt(x.Email),
                    Message = StringCipher.Decrypt(x.Message),
                    CreatedDate = x.CreatedDate,
                    UserType = StringCipher.Decrypt(x.UserType)
                }).ToList();

                var Contact = _Communication.GetContactusList().Select(x => new CommunicationViewModel
                {
                    ContactUsName = StringCipher.Decrypt(x.Name),
                    ContactUsEmail = StringCipher.Decrypt(x.Email),
                    ContactUsMessage = StringCipher.Decrypt(x.Message),
                    ContactUsCreatedDate = x.CreatedDate,
                    // UserType = StringCipher.Decrypt(x.UserType)
                }).ToList();

                model.EquipmentList = Equipment;
                model.ContactList = Contact;
                model.AllList = Equipment.Union(Contact).ToList();
            }

            if (type == "Doctor")
            {
                var Equipment = _Communication.GetEquipmentList().Where(y => y.UserType == "Doctor").Select(x => new CommunicationViewModel
                {
                    EquipmentName = StringCipher.Decrypt(x.EquipmentName),
                    UserName = StringCipher.Decrypt(x.UserName),
                    Email = StringCipher.Decrypt(x.Email),
                    Message = StringCipher.Decrypt(x.Message),
                    CreatedDate = x.CreatedDate,
                    UserType = StringCipher.Decrypt(x.UserType)
                }).ToList();

                var Contact = _Communication.GetContactusList().Where(x => x.RoleType == (int)RoleType.Doctor).Select(x => new CommunicationViewModel
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


                var Report = _reportProblemService.GetRportsProblem().Where(x => x.RoleType == (int)RoleType.Doctor).Select(x => new ReportProblemViewModel
                {
                    ReportProblemSubject = x.ReportProblemSubject,
                    SenderId = x.SenderId,
                    RecevierId = x.RecevierId,
                    ReportProblemId = x.ReportProblemId,
                    IsRead = x.IsRead,
                    IsCompleted = x.IsCompleted,
                    UserName = _doctorService.GetDoctorByLoginId(x.SenderId).FirstName + " " + _doctorService.GetDoctorByLoginId(x.SenderId).SurName,
                    CreatedDate = x.CreatedDate

                }).ToList();

                model.EquipmentList = Equipment;
                model.ContactList = Contact;
                model.Problemlist = Report;
            }
            if (type == "Patient")
            {
                var Equipment = _Communication.GetEquipmentList().Where(y => y.UserType == "Patient").Select(x => new CommunicationViewModel
                {
                    EquipmentName = StringCipher.Decrypt(x.EquipmentName),
                    UserName = StringCipher.Decrypt(x.UserName),
                    Email = StringCipher.Decrypt(x.Email),
                    Message = StringCipher.Decrypt(x.Message),
                    CreatedDate = x.CreatedDate,
                    UserType = StringCipher.Decrypt(x.UserType)
                }).ToList();
                var Contact = _Communication.GetContactusList().Where(x => x.RoleType == (int)RoleType.Patient).Select(x => new CommunicationViewModel
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


                var Report = _reportProblemService.GetRportsProblem().Where(x => x.RoleType == (int)RoleType.Patient).Select(x => new ReportProblemViewModel
                {
                    ReportProblemSubject = x.ReportProblemSubject,
                    SenderId = x.SenderId,
                    RecevierId = x.RecevierId,
                    ReportProblemId = x.ReportProblemId,
                    IsRead = x.IsRead,
                    IsCompleted = x.IsCompleted,
                    UserName = _userService.GetPatientByLoginId(x.SenderId).FirstName + " " + _userService.GetPatientByLoginId(x.SenderId).SurName,
                    CreatedDate = x.CreatedDate

                }).ToList();

                model.EquipmentList = Equipment;
                model.ContactList = Contact;
                model.Problemlist = Report;
            }
            return View(model);
        }

        //Report Problem by Dheeraj
        public JsonResult Reply(string reply, int id)
        {
            var ProblemReply = new ReportProblemReply()
            {
                Message = reply,
                ReplyById = base.loginUserModel.LoginId,
                RoleType = (int)RoleType.ADMIN,
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

        //contact us by Dheeraj

        public JsonResult ContactReply(string reply, int id)
        {
            var ContactReply = new ContactReply()
            {
                Message = reply,
                ReplyById = base.loginUserModel.LoginId,
                RoleType = (int)RoleType.ADMIN,
                ContactUsId = id,
                IsRead = false,
                IsCompleted = false

            };
            if (ContactReply != null)
            {
                _Communication.Add(ContactReply);
            }
            return Json(ContactReply, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContactReply(int Replyid)
        {
            var reply = _Communication.GetContactusReplyList().Select(x => new CommunicationViewModel
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
            var reply = _Communication.GetContactusReplyList().Select(x => new CommunicationViewModel
            {
                Message = x.Message,
                ContactUsId = x.ContactUsId,
                ContactReplyId = x.ContactReplyId,
                ReplyById = x.ReplyById,
                // ReplyById = x.ReplyById,
                RoleType = x.RoleType,
                CreatedDate = x.CreatedDate

            }).Where(x => x.ContactUsId == problemid).ToList();

            return Json(reply, JsonRequestBehavior.AllowGet);
        }
	}
}