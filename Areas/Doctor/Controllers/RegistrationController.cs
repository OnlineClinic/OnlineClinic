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
using System.Collections.ObjectModel;
using System.IO;
using System.Configuration;
using MyOnlineClinic.Emailer;

namespace MyOnlineClinic.Web.Areas.Doctor.Controllers
{
    public class RegistrationController : BaseController
    {
        private readonly ITitleService _TitleServices;
        private readonly IUserService _userService;
        private readonly IProfessionTypeService _professionService;
        public RegistrationController(ITitleService TitleServices, IUserService userService, IProfessionTypeService professionService)
        {
            _TitleServices = TitleServices;
            _userService = userService;
            _professionService = professionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Add()
        {
            RegisterViewModel model = new RegisterViewModel();
            BindDropDown(model);
            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Add(RegisterViewModel model)
        {
            BaseMember profile = null;
            model.RoleId = RoleType.Doctor;
            model.OrgId = 1;
            profile = model.GetDoctorAdmin();
            var user = model.GetLogin(profile, model.Email);

            var Password = RandomPassword.Generate(6, 8);
            user.LoginPassword = Password;
            _userService.Add(user);
            BindDropDown(model);
            string activationToken = user.ActivationToken.ToString();
            string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
            var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@@Password", Password).Replace("@@ActivationLink", link);
            EmailService objEmail = new EmailService();
            objEmail.SendEmail("User Registration", html, model.Email, "tma@myonlineclinic.com.au");
            return View(model);
        }
        public RegisterViewModel BindDropDown(RegisterViewModel model)
        {
            var titlelist = _TitleServices.GetTitleList().Where(t => t.Type == (int)RoleType.Patient).ToList();
            for (int i = 0; i < titlelist.Count; i++)
            {
                titlelist[i].TitleName = StringCipher.Decrypt(titlelist[i].TitleName);
            }
            model.TitleList = titlelist;
            model.ProfessionTypes = _professionService.GetProfessionTypesList();
            model.ProfessionSubType = _professionService.GetProfessionSubTypesList();
            return model;
        }
        public ActionResult GetProfessionSubTypes(int Id)
        {
            var ProfessionSubType = _professionService.GetProfessionSubTypesList().Where(t => t.Id == Id).ToList();
            return Json(ProfessionSubType, JsonRequestBehavior.AllowGet);
        }
    }
}