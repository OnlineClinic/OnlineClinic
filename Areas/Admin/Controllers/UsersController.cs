using MyOnlineClinic.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Models;
using System.IO;
using System.Drawing;
using MyOnlineClinic.Emailer;
using System.Configuration;
using MyOnlineClinic.Web.Helper;
using Omu;
using System.Collections.ObjectModel;

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    [AuthorizeRole("Admin")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        //
        // GET: /Admin/Clinic/
        //public ActionResult Index()
        //{
        //    var userService;// _userService.GetAdminUsers().Select(w=>new AdminUserProfileViewModel{ AdminUserId=w., });

        //    return View(userService);
        //}
    }
}
