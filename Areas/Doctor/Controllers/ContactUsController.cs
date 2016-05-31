using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System.Web.Mvc;
using MyOnlineClinic.Emailer;

namespace MyOnlineClinic.Web.Areas.Doctor.Controllers
{
    public class ContactUsController : BaseController
    {
        private readonly ICommunication _Communication;
        private readonly IReportProblemService _reportProblemService;
        private readonly IDoctorService _doctorService;
        private readonly IAdminUserService _adminUserService;
        public ContactUsController(ICommunication Communication, IReportProblemService reportproeblemService, IDoctorService doctorService, IAdminUserService adminUserService)
        {
            _Communication = Communication;
            _reportProblemService = reportproeblemService;
            _doctorService = doctorService;
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Index(CommunicationViewModel model)
        {
            var adminLogiid = _adminUserService.FindAdmin((int)RoleType.ADMIN).LoginId;
            var Contact = new ContectUs
            {
                Name = model.ContactUsName,
                Email = model.ContactUsEmail,
                Message = model.ContactUsMessage,
                SenderId = base.loginUserModel.LoginId,
                RecevierId = adminLogiid,
                IsRead = false,
                IsCompleted = false,
                RoleType = (int)RoleType.Doctor,
                Subject=model.Subject
            };
            if (Contact != null)
            {
                _Communication.Add(Contact);
                var fileUrl = Server.MapPath("~/EmailTemplates/Communication.html");
                string html = System.IO.File.ReadAllText(fileUrl);
                html = html.Replace("@UserName", model.ContactUsName);
                html = html.Replace("@Email", model.ContactUsEmail);
                html = html.Replace("@Message", model.ContactUsMessage);
                EmailService objEmail = new EmailService();
                objEmail.SendEmail("Contact us", html, "mocbookings@doctortoyou.com.au", "tma@myonlineclinic.com.au");
                return Redirect(Request.UrlReferrer.ToString());
            }
            return View(model);
        }

    }
}