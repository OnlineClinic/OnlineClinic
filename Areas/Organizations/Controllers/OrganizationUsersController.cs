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
namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    //[AuthorizeRole("OrganizationAdmin")]
    public class OrganizationUsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IOrganizationUserService _organizationUserService;
        private readonly IOrganizationService _organizationService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ILoginHelper _loginService;
        private readonly ILookUpUserRolesService _roleService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ILookUpModuleService _moduleService;
        private readonly IPermissionInModuleService _permissioninmoduleService;
        private readonly IPermissionsService _permissionService;
        private readonly IStateService _stateService;
        private readonly ITitleService _TitleServices;

        public OrganizationUsersController(IUserService userService, IOrganizationUserService organizationUserService,
            IOrganizationService organizationService, ICountryService countryService, ICityService cityService,
            ILoginHelper loginService, ILookUpUserRolesService roleService, ITimeZoneService timeZoneService,
            ILookUpModuleService moduleService, IPermissionInModuleService permissioninmoduleService,
            IPermissionsService permissionService, IStateService stateService, ITitleService titleServices)
        {
            _organizationUserService = organizationUserService;
            _userService = userService;
            _organizationService = organizationService;
            _countryService = countryService;
            _cityService = cityService;
            _loginService = loginService;
            _roleService = roleService;
            _timeZoneService = timeZoneService;
            _moduleService = moduleService;
            _permissioninmoduleService = permissioninmoduleService;
            _permissionService = permissionService;
            _stateService = stateService;
            _TitleServices = titleServices;
        }
        //
        // GET: /Organizations/OrganizationUsers/
        public ActionResult Index()
        {
            var users = new List<OrganizationUserViewModel>();

            try
            {
                users = _organizationUserService.GetByOrganization(base.loginUserModel.OrganizationId).Select(x =>
                 new OrganizationUserViewModel
                 {
                     FirstName = StringCipher.Decrypt(x.FirstName),
                     MiddleName = StringCipher.Decrypt(x.MiddleName),
                     SurName = StringCipher.Decrypt(x.SurName),
                     MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                     PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                     FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                     UserId = x.MemberId,
                     OrgId = x.OrgId,
                     OrgUsreRole = x.OrgUsreRole,
                     Address1 = StringCipher.Decrypt(x.Address1),
                     Address2 = StringCipher.Decrypt(x.Address2),
                     RoleName = _roleService.GetUserRolesById(x.OrgUsreRole) != null ? StringCipher.Decrypt(_roleService.GetUserRolesById(x.OrgUsreRole).RoleName) : "",
                     ProfilePic = _userService.Find(x.LoginId).Avatar,
                     Email = StringCipher.Decrypt(x.Email),
                     IsActive = x.IsActive,
                     LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                     TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)

                 }).ToList();
            }
            catch { }
            return View(users);
        }
        public ActionResult Add(int? id)
        {
            OrganizationUserViewModel model = new OrganizationUserViewModel();
            if (id.HasValue)
            {
                var user = _organizationUserService.GetUserById(Convert.ToInt32(id));
                model = new OrganizationUserViewModel(user);
                model.FirstName = StringCipher.Decrypt(user.FirstName);
                model.MiddleName = StringCipher.Decrypt(user.MiddleName);
                model.SurName = StringCipher.Decrypt(user.SurName);
                model.PhoneNumber = StringCipher.Decrypt(user.PhoneNumber);
                model.MobileNumber = StringCipher.Decrypt(user.MobileNumber);
                model.FaxNumber = StringCipher.Decrypt(user.FaxNumber);
                model.PostCode = StringCipher.Decrypt(user.PostCode);
                model.DOB = user.DOB;
                model.OrgUsreRole = user.OrgUsreRole;
                model.State = user.State;
                model.CountryId = user.CountryId;
                model.State = user.State;
                model.OrgId = user.OrgId;
                model.Suburb = user.Suburb;
                model.Email = StringCipher.Decrypt(user.Email);
                model.Address1 = StringCipher.Decrypt(user.Address1);
                model.Address2 = StringCipher.Decrypt(user.Address2);
                model.LoginId = user.LoginId;
                model.TimeZone = Common.GetTimeZoneStandardIdAndDisplayName(user.TimeZoneString);
                model.UserId = user.MemberId;
                BindDropDownLists(model);
                return View(model);
            }
            try
            {
                model.ProfilePic = _userService.Find(model.LoginId).Avatar;
            }
            catch { }


            BindDropDownLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(OrganizationUserViewModel model, HttpPostedFileBase ProfilePic)
        {
            var org = _organizationService.GetOrganizationList().FirstOrDefault(x => x.OrganizationId == base.loginUserModel.OrganizationId);
            var cModel = model.GetEditModel(model);
            if (org == null)
            {
                BindDropDownLists(model);
                ViewBag.ErrorMessage = "Not a valid organization.";
                return View(model);
            }
            else
            {
                cModel.OrgId = base.loginUserModel.OrganizationId;
                model.OrgId = base.loginUserModel.OrganizationId;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZone);
                TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
                cModel.TimeZoneString = Common.GetTimeZoneOffset(offset);

                if (cModel.MemberId > 0)
                {
                    cModel.DOB = DateTime.Now;
                    cModel.ModifiedBy = base.loginUserModel.LoginId;
                    cModel.ModifiedDate = DateTime.UtcNow;
                    _organizationUserService.Update(cModel);
                    return RedirectToAction("Index");
                }
                else
                {
                    var userName = model.Email;
                    if (_userService.CheckDuplicateMail(StringCipher.Encrypt(model.Email)))
                    {
                        BindDropDownLists(model);
                        ViewBag.ErrorMessage = "This email address already exists.";
                        return View(model);
                    }
                    if (_userService.CheckDuplicateUserName(StringCipher.Encrypt(model.Email)))
                    {
                        BindDropDownLists(model);
                        ViewBag.ErrorMessage = "This email address already exists.";
                        return View(model);
                    }
                    if (cModel != null)
                    {
                        _loginService.UploadImage(model, ProfilePic);
                        BaseMember profile = null;
                        profile = _loginService.GetUser(model, cModel.OrgId);
                        var user = _loginService.GetOrganiztionUserLogin(profile, model.Email, model, RoleType.OrgUser);
                        var randomPassword = RandomPassword.Generate(6, 8);
                        user.LoginPassword = randomPassword;
                        _userService.Add(user);

                        string activationToken = user.ActivationToken.ToString();
                        string link = ConfigurationManager.AppSettings["SitePath"] + "Account/Activate/" + activationToken;
                        var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
                        string html = System.IO.File.ReadAllText(fileUrl);
                        html = html.Replace("@@Password", "").Replace("@@ActivationLink", link);
                        EmailService objEmail = new EmailService();
                        objEmail.SendEmail("User Registration", html, userName, "tma@myonlineclinic.com.au");


                        foreach (var item in model.ModuleList)
                        {
                            if (item.Selected)
                            {
                                var permission = new PermissionInModule();
                                foreach (var inneritem in item.SubModuleList)
                                {

                                    if (inneritem.Selected)
                                    {
                                        permission.LoginId = user.LoginId;
                                        permission.PermissionId = inneritem.Id;
                                        permission.ModuleId = item.Id;
                                        permission.UserRoleId = model.OrgUsreRole;
                                        _permissioninmoduleService.Add(permission);
                                    }
                                }
                            }
                        }

                        return RedirectToAction("Index");
                    }
                }
            }
            BindDropDownLists(model);
            return View(model);
        }
        public OrganizationUserViewModel BindDropDownLists(OrganizationUserViewModel model)
        {
            var title = _TitleServices.GetTitleList().Where(x => x.Type == (int)RoleType.ADMIN).ToList();
            for (int i = 0; i < title.Count; i++)
            {
                title[i].TitleName = StringCipher.Decrypt(title[i].TitleName);
            }
            //_TitleServices.GetTitleList()
            model.TitleList = title.Where(x => x.Type == (int)RoleType.ADMIN).ToList();

            var countrylist = _countryService.GetCountryList();
            for (int i = 0; i < countrylist.Count; i++)
            {
                countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
            }
            model.Countries = countrylist;
            var Citylist = _cityService.GetCityList();
            for (int i = 0; i < Citylist.Count; i++)
            {
                Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
            }
            var Statelist = _stateService.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.Cities = model.OrgId > 0 ? Citylist : new List<LookUpCity>();
            model.StateList = model.OrgId > 0 ? Statelist : new List<LookUpState>();
            model.UserRoleList = _roleService.GetUserRolesList().Where(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Organization")).Select(x => new OrganizationRoleDecryptMoel
            {
                RoleName = StringCipher.Decrypt(x.RoleName),
                Id = x.Id,
                UserRoleTypeId = x.UserRoleTypeId
            }).ToList();
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.TimeZoneCollection = timeZoneList;
            model.TimeZonesesList = _timeZoneService.GetTimeZone();

            string[] arrPermission = new string[] { "Add Organisations", "Edit Organisations", "View Organizations", 
                                                    "View Organization Details", "Active/Inactive Organisations" 
                                                  };

            model.ModuleList = _moduleService.GetmoduleList().Where(x => x.ModuleName != "Manage Staff").Select(w =>
                                                              new ModuleListModel
                                                              {
                                                                  Id = w.Id,
                                                                  Name = w.ModuleName,
                                                                  Selected = false,
                                                                  SubModuleList = _permissionService.GetPermissionByModule(w.Id).
                                                                  Select(x => new SubModuleListModel
                                                                  {
                                                                      Id = x.Id,
                                                                      Name = x.PermissionName,
                                                                      Selected = false
                                                                  }).Where(x => !arrPermission.Contains(x.Name)).ToList()
                                                              }).ToList();

            return model;
        }
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection[0];
            _organizationUserService.Delete(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _organizationUserService.Activate(ids);
            return RedirectToAction("Index");
        }
    }
}
