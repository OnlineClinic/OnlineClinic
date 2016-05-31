using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
using System.Data.SqlClient;
using MyOnlineClinic.Migrator;
using System.Globalization;

namespace MyOnlineClinic.Web.Areas.Patients.Controllers
{
    public class ReportProblemController : BaseController
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IReportProblemService _reportProblemService;

        public ReportProblemController (IAdminUserService adminUserService,IReportProblemService reportProblemService)
        {
            _adminUserService = adminUserService;
            _reportProblemService = reportProblemService;
        }
      
        public ActionResult Index(ReportProblemViewModel model)
        {
            var adminLogiid = _adminUserService.FindAdmin((int)RoleType.ADMIN).LoginId;
            var Report = new ReportProblem
            {
                ReportProblemSubject = model.ReportProblemSubject,
                SenderId = base.loginUserModel.LoginId,
                RecevierId = adminLogiid,
                IsRead = false,
                IsCompleted = false,
                RoleType = (int)RoleType.Patient
            };
            if(Report!=null)
            {
                _reportProblemService.Add(Report);
            }
            return View(model);
        }
	}
}