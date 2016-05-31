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


namespace MyOnlineClinic.Web.Areas.Patients.Controllers
{
    public class RegistrationController : BaseController
    {
        private readonly ITitleService _TitleServices;
        private readonly IUserService _userService;
        public RegistrationController(ITitleService TitleServices, IUserService userService)
        {
            _TitleServices = TitleServices;
            _userService = userService;
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
            Patient profile = null;
            model.RoleId = RoleType.Patient;
            model.OrgId = 1;
            profile = model.GetAdminPatient(model.OrgId);
            var user = model.GetLoginAdmin(profile, model.Email);
            var Password = RandomPassword.Generate(6, 8);
            user.LoginPassword = Password;
            user.SetCreated(user.LoginId);
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
            return model;
        }
    }
}