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

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class ClinicUsersController : BaseController
    {
        private readonly IClinicService _clinicService;
        private readonly IClinicUserService _clinicUserService;
        private readonly IUserService _userService;
        private readonly ILoginHelper _loginService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IStateService _stateService;
        private readonly ILookUpUserRolesService _roleService;
        private readonly IOrganizationService _organizationService;
        private readonly ITitleService _titleservice;
        private readonly ILookUpModuleService _moduleService;
        private readonly IPermissionsService _permissionService;
        public ClinicUsersController(IClinicService clinicService, IClinicUserService clinicUserService,
            IUserService userService, ILoginHelper loginService, ICountryService countryService,
            ICityService cityService, ILookUpUserRolesService roleService, IOrganizationService organizationService,
            IStateService stateService, ITitleService titleservice, ILookUpModuleService moduleService,
            IPermissionsService permissionService
            )
        {
            _clinicService = clinicService; _clinicUserService = clinicUserService;
            _loginService = loginService; _userService = userService;
            _countryService = countryService; _cityService = cityService;
            _roleService = roleService;
            _organizationService = organizationService;
            _stateService = stateService;
            _titleservice = titleservice;
            _moduleService = moduleService;
            _permissionService = permissionService;
        }
        //
        // GET: /Organizations/ClinicUsers/
        public ActionResult Index(int id)
        {
            var users = new List<ClinicUserViewModel>();
            var cliniUsers = _clinicUserService.GetUsersByClinic(id).Select(x => new ClinicUserViewModel
            {
                FirstName = StringCipher.Decrypt(x.FirstName),
                SurName = StringCipher.Decrypt(x.SurName),
                Email = StringCipher.Decrypt(x.Email),
                ClinicName = StringCipher.Decrypt(_clinicService.GetClinicById(id).ClinicName),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                UserId = x.MemberId,
                ProfilePic = _userService.Find(x.LoginId).Avatar,
                //ClinicLogo = _clinicService.GetClinicById(id).ClinicLogo,
                //OrganizationName = x.Clinic.organization.OrganizationName,
                //IsApproved = x.IsActive,
                OrgId = x.OrgId,
                LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                RoleNameId = x.ClinicUsreRole,
                IsActive = x.IsActive

            }).ToList();
            for (int i = 0; i < cliniUsers.Count; i++)
            {
                var organizationName = string.Empty;
                var rolename = string.Empty;
                try
                {
                    if (_organizationService.GetOrganizationById(cliniUsers[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(cliniUsers[i].OrgId).OrganizationName);
                    }
                    if (_roleService.GetUserRolesById(cliniUsers[i].RoleNameId).RoleName != null)
                    {
                        rolename = StringCipher.Decrypt(_roleService.GetUserRolesById(cliniUsers[i].RoleNameId).RoleName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    cliniUsers[i].OrganizationName = organizationName;
                }
                if (!string.IsNullOrEmpty(rolename))
                {
                    cliniUsers[i].RoleName = rolename;
                }
            }
            return View(cliniUsers);
        }
        [HttpPost]
        public ActionResult Index(int id, SearchParametersViewModel searchModel)
        {
            var cliniUsers = _clinicUserService.GetUsersByClinic(id).Select(x => new ClinicUserViewModel
            {
                FirstName = StringCipher.Decrypt(x.FirstName),
                SurName = StringCipher.Decrypt(x.SurName),
                Email = StringCipher.Decrypt(x.Email),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                IsApproved = x.IsActive,
                UserId = x.MemberId,
                ClinicId = x.ClinicId,
                OrgId = x.OrgId,
                LoginId = x.LoginId,
                ModifiedDate = x.ModifiedDate,
                RoleNameId = x.ClinicUsreRole,
                LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)
            }).ToList();
            for (int i = 0; i < cliniUsers.Count; i++)
            {
                var ClinicLogo = string.Empty;
                var OrganizationName = string.Empty;
                var ClinicName = string.Empty;
                var ProfilePic = string.Empty;
                var rolename = string.Empty;
                try
                {
                    //cliniUsers[i].Address2 = StringCipher.Decrypt(_roleService.GetUserRolesList().Where(x => x.UserRoleTypeId == cliniUsers[i].OrgUsreRole).FirstOrDefault().RoleName);
                    cliniUsers[i].FirstName = cliniUsers[i].MiddleName != null ? StringCipher.Decrypt(cliniUsers[i].FirstName) + " " + StringCipher.Decrypt(cliniUsers[i].MiddleName) + " " + StringCipher.Decrypt(cliniUsers[i].SurName) : StringCipher.Decrypt(cliniUsers[i].FirstName) + " " + StringCipher.Decrypt(cliniUsers[i].SurName);
                    if (_clinicService.GetClinicById(cliniUsers[i].ClinicId).ClinicLogo != null)
                    {
                        ClinicLogo = _clinicService.GetClinicById(cliniUsers[i].ClinicId).ClinicLogo;
                    }
                    if (_clinicService.GetClinicById(cliniUsers[i].ClinicId).ClinicName != null)
                    {
                        ClinicName = StringCipher.Decrypt(_clinicService.GetClinicById(cliniUsers[i].ClinicId).ClinicName);
                    }
                    if (_organizationService.GetOrganizationById(cliniUsers[i].OrgId).OrganizationName != null)
                    {
                        OrganizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(cliniUsers[i].OrgId).OrganizationName);
                    }
                    if (_userService.Find(cliniUsers[i].LoginId).Avatar != null)
                    {
                        ProfilePic = _userService.Find(cliniUsers[i].LoginId).Avatar;
                    }
                    if (_roleService.GetUserRolesById(cliniUsers[i].RoleNameId).RoleName != null)
                    {
                        rolename = StringCipher.Decrypt(_roleService.GetUserRolesById(cliniUsers[i].RoleNameId).RoleName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(ClinicLogo))
                {
                    cliniUsers[i].ClinicLogo = ClinicLogo;
                }
                if (!string.IsNullOrEmpty(ClinicName))
                {
                    cliniUsers[i].ClinicName = ClinicName;
                }
                if (!string.IsNullOrEmpty(OrganizationName))
                {
                    cliniUsers[i].OrganizationName = OrganizationName;
                }
                if (!string.IsNullOrEmpty(ProfilePic))
                {
                    cliniUsers[i].ProfilePic = ProfilePic;
                }
                if (!string.IsNullOrEmpty(rolename))
                {
                    cliniUsers[i].RoleName = rolename;
                }

            }
            cliniUsers = cliniUsers.Where(x =>
                (
                 (
                 searchModel.OrgName != null && x.OrganizationName.IndexOf(searchModel.OrgName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.OrgName == null)
                 &&
                    ((searchModel.ClinicName != null && x.ClinicName.IndexOf(searchModel.ClinicName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.ClinicName == null)
                     &&
                    ((searchModel.StaffName != null && x.FirstName.IndexOf(searchModel.StaffName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.StaffName == null)
                    &&
                    ((searchModel.Email != null && x.Email.IndexOf(searchModel.Email, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.Email == null)
                     &&
                    ((searchModel.RoleName != null && x.RoleName.IndexOf(searchModel.RoleName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.RoleName == null)
                    //&&
                    //((searchModel.Role != 0 && x.OrgUsreRole == searchModel.Role) || searchModel.Role == 0)
                    //&& 
                    //((objFilter.OrgAdmin != 0 && x.FaxNumber == Convert.ToString(objFilter.OrgAdmin)) || objFilter.OrgAdmin == 0)
                   &&
                   ((searchModel.ActivationStatus != null && x.IsActive == Convert.ToBoolean(searchModel.ActivationStatus)) ||
                         searchModel.ActivationStatus == null)
                  ).ToList();
            return View(cliniUsers);
        }
        public ActionResult Add(int? id)
        {
            ClinicUserViewModel model = new ClinicUserViewModel();
            string[] arrPermission = new string[] { "Add Clinics", "Edit Clinics", "View Clinics", 
                                                    "View Clinic Details", "Active/Inactive Clinics" 
                                                  };
            model.ModuleList = _moduleService.GetmoduleList().Where(x => x.Id != (int)ModuleNames.ManageOrganizations &&
                                                                    x.Id != (int)ModuleNames.ManageStaff).
                                                                    Select(w => new ModuleListModel
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

            if (id.HasValue)
            {
                var user = _clinicUserService.GetUserById(Convert.ToInt32(id));
                user.FirstName = StringCipher.Decrypt(user.FirstName);
                user.MiddleName = StringCipher.Decrypt(user.MiddleName);
                user.SurName = StringCipher.Decrypt(user.SurName);
                user.Address1 = StringCipher.Decrypt(user.Address1);
                user.Address2 = StringCipher.Decrypt(user.Address2);
                user.PostCode = StringCipher.Decrypt(user.PostCode);
                user.FaxNumber = StringCipher.Decrypt(user.FaxNumber);
                user.Email = StringCipher.Decrypt(user.Email);
                model = new ClinicUserViewModel(user);
                BindDropDownLists(model);
                return View(model);
            }
            BindDropDownLists(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(ClinicUserViewModel model, HttpPostedFileBase ProfilePic)
        {
            ClinicUsers cModel = new ClinicUsers();
            var organization = _organizationService.GetOrganizationByLoginId(base.loginUserModel.LoginId);
            if (organization != null)
            {
                model.OrgId = organization.OrganizationId;
            }
            cModel = model.GetEditModel(model);
            if (model.OrgId == 0)
            {
                BindDropDownLists(model);
                ViewBag.ErrorMessage = "Not a valid organization.";
                return View(model);
            }
            else
            {

                if (cModel.MemberId > 0)
                {
                    cModel.DOB = DateTime.Now;
                    cModel.ModifiedBy = base.loginUserModel.LoginId;
                    cModel.ModifiedDate = DateTime.Now;
                    _clinicUserService.Update(cModel);
                    ViewBag.Message = "Message Sent";
                }
                else
                {
                    var userName = model.Email;
                    if (_userService.CheckDuplicateMail(model.Email))
                    {
                        BindDropDownLists(model);
                        ViewBag.ErrorMessage = "This email address already exists.";
                        return View(model);
                    }
                    if (_userService.CheckDuplicateUserName(model.Email))
                    {
                        BindDropDownLists(model);
                        ViewBag.ErrorMessage = "This email address already exists.";
                        return View(model);
                    }
                    if (cModel != null)
                    {
                        UploadImage(model, ProfilePic);
                        BaseMember profile = null;
                        profile = _loginService.GetClinicUser(model, model.ClinicId, cModel.OrgId);
                        var user = _loginService.GetClinicUserLogin(profile, model.Email, model, RoleType.ClinicUser);
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

                    }
                }
            }
            BindDropDownLists(model);
            return View(model);
        }

        public ClinicUserViewModel BindDropDownLists(ClinicUserViewModel model)
        {
            var countrylist = _countryService.GetCountryList();
            for (int i = 0; i < countrylist.Count; i++)
            {
                countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
            }

            var Citylist = _cityService.GetCityList();
            for (int i = 0; i < Citylist.Count; i++)
            {
                Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
            }
            //model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            //model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();          
            var Statelist = _stateService.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.Countries = countrylist;
            model.Cities = model.ClinicId > 0 ? Citylist : new List<LookUpCity>();
            model.States = model.ClinicId > 0 ? Statelist : new List<LookUpState>();
            model.UserRoleList = _roleService.GetUserRolesList().Where(x => x.UserRoleTypeId == 3).ToList();
            model.TitleList = _titleservice.GetTitleList().Where(x => x.Type == (int)RoleType.User).ToList();
            for (int i = 0; i < model.UserRoleList.Count; i++)
            {
                model.UserRoleList[i].RoleName = StringCipher.Decrypt(model.UserRoleList[i].RoleName);
            }
            model.ClinicsList = _clinicService.GetAllClinic().Where(x => x.IsActive).ToList();
            for (int i = 0; i < model.ClinicsList.Count; i++)
            {
                model.ClinicsList[i].ClinicName = StringCipher.Decrypt(model.ClinicsList[i].ClinicName);
            }
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.TimeZones = timeZoneList;
            return model;
        }
        public ClinicUserViewModel UploadImage(ClinicUserViewModel model, HttpPostedFileBase profilePic)
        {
            if (profilePic != null && profilePic.ContentLength > 0)
            {
                var fileName = Path.GetFileName(profilePic.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    if (!string.IsNullOrEmpty(model.ProfilePic))
                    {
                        //Delete exising File
                        var file = Server.MapPath(model.ProfilePic);
                        imageHelper.Delete(file);
                    }
                    string fName = Convert.ToString(DateTime.Now.ToString("ddMMyyyy-HHmmss-fff")) + extension;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/UserProfile/"), fName);
                    var bitmap = new Bitmap(profilePic.InputStream);
                    imageHelper.Save(bitmap, 150, 150, 250, path);
                    model.ProfilePic = "/UploadedFiles/UserProfile/" + fName;
                }
                else
                {
                    ModelState.AddModelError("Error", "Not a valid file");
                    ViewBag.CurrentMenu = "Publicidade";
                    ViewBag.isError = 1;
                    return model;
                }
            }
            else
            {
                if (model.ProfilePic != null)
                {
                    model.ProfilePic = model.ProfilePic;
                }

            }
            return model;
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection[0];
            _clinicUserService.Delete(ids);
            return Redirect(Request.UrlReferrer.ToString()); // return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _clinicUserService.Activate(ids);
            return Redirect(Request.UrlReferrer.ToString()); //return RedirectToAction("Index");
        }
    }
}
