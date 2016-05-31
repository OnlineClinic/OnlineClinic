using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Models;
using System.IO;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Emailer;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    [AuthorizeRole("Admin")]
    public class DashBoardController : BaseController
    {
        #region "VARIABLES"
        protected readonly IUserService _userService;
        private readonly IOrganizationService _organiztionService;
        private readonly IClinicService _clinicService;
        private readonly ILoginHelper _loginHelper;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ILookUpModuleService _moduleService;
        private readonly IPermissionsService _permissionService;
        private readonly ILookUpUserRolesService _roleService;
        private readonly IPermissionInModuleService _permissionInModuleService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ITitleService _titleService;
        private readonly IGenderService _genderService;
        private readonly IStateService _stateService;
        private readonly IAdminUserService _adminUserService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        #endregion

        #region "CONSTRUCTOR"
        public DashBoardController(IUserService userService, IOrganizationService organiztionService, IClinicService clinicService,
            ILoginHelper loginHelper, ICountryService countryService, ICityService cityService, ILookUpModuleService moduleService,
            IPermissionsService permissionService, ILoginHelper loginService, ILookUpUserRolesService roleService, IAdminUserService adminUserService,
            IPermissionInModuleService permissioninmoduleService, ITimeZoneService timezoneService,
            ITitleService TitleServices, IGenderService genderService, IStateService stateService, IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _userService = userService;
            _organiztionService = organiztionService;
            _clinicService = clinicService;
            _loginHelper = loginHelper;
            _countryService = countryService;
            _cityService = cityService;
            _moduleService = moduleService;
            _permissionService = permissionService;
            _roleService = roleService;
            _permissionInModuleService = permissioninmoduleService;
            _timeZoneService = timezoneService;
            _titleService = TitleServices;
            _genderService = genderService;
            _stateService = stateService;
            _adminUserService = adminUserService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }
        #endregion

        #region "ACTIONS"
        public new ActionResult Profile()
        {
            Login model = _userService.Find(base.loginUserModel.LoginId);
            UserViewModel userModel = new UserViewModel();
            userModel.FirstName = StringCipher.Decrypt(model.Member.FirstName) + " " + StringCipher.Decrypt(model.Member.SurName);
            userModel.LoginName = StringCipher.Decrypt(model.LoginName);
            userModel.EmailAddress = StringCipher.Decrypt(model.Email);
            userModel.FaxNumber = StringCipher.Decrypt(model.Member.FaxNumber);
            userModel.DOB = model.Member.DOB;
            userModel.PostCode = StringCipher.Decrypt(model.Member.PostCode);
            userModel.PhoneNumber = StringCipher.Decrypt(model.Member.PhoneNumber);
            userModel.MobileNumber = StringCipher.Decrypt(model.Member.MobileNumber);
            userModel.Address1 = StringCipher.Decrypt(model.Member.Address1);
            userModel.Address2 = StringCipher.Decrypt(model.Member.Address2);
            userModel.MemberId = model.Member.MemberId;
            userModel.LoginId = model.LoginId;
            userModel.ProfilePic = model.Avatar;
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Profile(UserViewModel model, HttpPostedFileBase ProfilePic)
        {
            _loginHelper.UpdateProfile(model, ProfilePic, base.loginUserModel.LoginId);
            Response.Redirect("Profile");
            return View(model);
        }

        public ActionResult UsersList()
        {
            var users = _adminUserService.GetAllAdminUser().Select(x =>
               new UserViewModel
               {
                   Fullname = StringCipher.Decrypt(_titleService.GetTitleById(x.Title).TitleName) + " " + StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                   EmailAddress = StringCipher.Decrypt(x.Email),
                   PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                   MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                   FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                   ModifiedDate = x.ModifiedDate ?? DateTime.Now,
                   PostCode = StringCipher.Decrypt(x.PostCode),
                   MemberId = x.MemberId,
                   RoleName = x.OrgUsreRole != 0 ? StringCipher.Decrypt(_roleService.GetUserRolesById(x.OrgUsreRole).RoleName) : string.Empty,
                   IsActive = x.IsActive,
                   ProfilePic = _userService.Find(x.LoginId).Avatar,
                   LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                   TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)
               }).ToList();
            return View(users);
        }

        [HttpPost]
        public ActionResult UsersList(SearchParametersViewModel searchModel)
        {
            var users = _adminUserService.GetAllAdminUser().Select(x =>
               new UserViewModel
               {
                   Fullname = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),

                   EmailAddress = StringCipher.Decrypt(x.Email),
                   PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                   MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                   FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                   LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                   TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                   PostCode = StringCipher.Decrypt(x.PostCode),
                   MemberId = x.MemberId,
                   RoleName = x.OrgUsreRole != 0 ? StringCipher.Decrypt(_roleService.GetUserRolesById(x.OrgUsreRole).RoleName) : string.Empty,
                   IsActive = x.IsActive
               }).ToList();

            //RoleList = _roleService.GetUserRolesList().Where(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Moc Admin")).Select(x => new RoleListViewModel
            //    {
            //        Id = x.Id,
            //        RoleName = StringCipher.Decrypt(x.RoleName)
            //    }).ToList()

            users = users.Where(x =>
                 ((searchModel.StaffName != null && x.Fullname.IndexOf(searchModel.StaffName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.StaffName == null)
                  &&

                  ((searchModel.RoleName != null && x.RoleName.IndexOf(searchModel.RoleName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.RoleName == null)
                    &&
                 ((searchModel.Email != null && x.EmailAddress.Contains(searchModel.Email) || searchModel.Email == null))
                &&
                   ((searchModel.ActivationStatus != null && x.IsActive == Convert.ToBoolean(searchModel.ActivationStatus)) ||
                         searchModel.ActivationStatus == null)

               ).ToList();

            return View(users);

        }

        public ActionResult Details(int id)
        {

            var model = _adminUserService.GetUsersById(id).Select(x => new UserViewModel
            {
                FirstName = StringCipher.Decrypt(x.FirstName),
                MiddleName = StringCipher.Decrypt(x.MiddleName),
                SurName = StringCipher.Decrypt(x.SurName),
                OrgUsreRole = x.OrgUsreRole,
                EmailAddress = StringCipher.Decrypt(x.Email),
                CountryId = x.CountryId,
                State = x.State,
                City = x.City,
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                PostCode = StringCipher.Decrypt(x.PostCode),
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                LoginId = x.LoginId,
                MemberId = x.MemberId,
                DobInString = Convert.ToDateTime(x.DOB).ToString("dd-MM-yyyy"),
                TimezoneId = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)
            }).ToList().FirstOrDefault();

            if (model.State != null)
            {
                model.StateName = StringCipher.Decrypt(_stateService.GetStateById(model.State).StateName);
            }
            if (model.CountryId != null)
            {
                model.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.CountryId).CountryName);
            }
            if (model.City != null)
            {
                model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);
            }

            if (model.ProfilePic != null)
            {
                model.ProfilePic = _userService.Find(model.LoginId).Avatar;
            }
            model.ProfilePic = _userService.Find(model.LoginId).Avatar;
            return View(model);
        }

        public ActionResult AddUser(int? id)
        {
            if (id > 0)
            {
                var title = _titleService.GetTitleList().Where(t => t.Type == (int)RoleType.User).ToList();
                for (int i = 0; i < title.Count; i++)
                {
                    title[i].TitleName = StringCipher.Decrypt(title[i].TitleName);
                }
                var cmodel = _adminUserService.GetAdminUserById(Convert.ToInt32(id));

                cmodel.FirstName = StringCipher.Decrypt(cmodel.FirstName);
                cmodel.MiddleName = StringCipher.Decrypt(cmodel.MiddleName);
                cmodel.SurName = StringCipher.Decrypt(cmodel.SurName);
                cmodel.Address1 = StringCipher.Decrypt(cmodel.Address1);
                cmodel.Address2 = StringCipher.Decrypt(cmodel.Address2);
                cmodel.PostCode = StringCipher.Decrypt(cmodel.PostCode);
                cmodel.FaxNumber = StringCipher.Decrypt(cmodel.FaxNumber);
                cmodel.Email = StringCipher.Decrypt(cmodel.Email);
                var userDetails = _userService.Find(cmodel.LoginId);
                var userPermissions = _permissionInModuleService.GetPermissionInModuleByUserId(cmodel.LoginId);
                var userModuleList = userPermissions.Select(w => new { w.ModuleId }).Distinct();
                var userSubModuleList = userPermissions.Select(w => new { w.PermissionId }).Distinct();
                var countrylist = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();
                for (int i = 0; i < countrylist.Count; i++)
                {
                    countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
                }

                var Citylist = _cityService.GetCityList().Where(x => x.IsActive == true).ToList();
                for (int i = 0; i < Citylist.Count; i++)
                {
                    Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
                }

                var Statelist = _stateService.GetStateList().Where(x => x.IsActive == true).ToList();
                for (int i = 0; i < Statelist.Count; i++)
                {
                    Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
                }
                UserViewModel model = new UserViewModel
                {
                    Countries = countrylist,
                    StateList = Statelist,
                    Cities = Citylist,
                    TitleList = title,
                    OrgUsreRole = cmodel.OrgUsreRole,
                    ModuleList =
                        _moduleService.GetmoduleList()
                            .Select(
                                w =>
                                    new ModuleListModel
                                    {
                                        Id = w.Id,
                                        Name = StringCipher.Decrypt(w.ModuleName),
                                        Selected = userModuleList.Any(x => x.ModuleId == w.Id) ? true : false,
                                        SubModuleList =
                                            _permissionService.GetPermissionByModule(w.Id)
                                                .Select(
                                                    x =>
                                                        new SubModuleListModel
                                                        {
                                                            Id = x.Id,
                                                            Name = StringCipher.Decrypt(x.PermissionName),
                                                            Selected = userSubModuleList.Any(y => y.PermissionId == x.Id) ? true : false
                                                        })
                                                .ToList()
                                    })
                            .ToList(),
                    TitleId = cmodel.Title,
                    FirstName = StringCipher.Decrypt(cmodel.FirstName),
                    MiddleName = StringCipher.Decrypt(cmodel.MiddleName),
                    SurName = StringCipher.Decrypt(cmodel.SurName),
                    Address1 = StringCipher.Decrypt(cmodel.Address1),
                    Address2 = StringCipher.Decrypt(cmodel.Address2),
                    PhoneNumber = StringCipher.Decrypt(cmodel.PhoneNumber),
                    FaxNumber = StringCipher.Decrypt(cmodel.FaxNumber),
                    CountryId = cmodel.CountryId,
                    State = cmodel.State,
                    City = cmodel.City,
                    PostCode = StringCipher.Decrypt(cmodel.PostCode),
                    DobInString = Convert.ToDateTime(cmodel.DOB).ToString("dd-MM-yyyy"),
                    TimezoneId = StringCipher.Decrypt(cmodel.TimeZoneString),
                    //OrgUsreRole = userDetails.LookUpRoleId == 1 ? userDetails.LookUpRoleId : cmodel.OrgUsreRole,
                    MemberId = cmodel.MemberId,
                    LoginId = cmodel.LoginId,
                    ProfilePic = userDetails.Avatar,
                    EmailAddress = StringCipher.Decrypt(userDetails.Email),

                    RoleList = _roleService.GetUserRolesList().Where(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Moc Admin")).Select(x => new RoleListViewModel
                 {
                     Id = x.Id,
                     RoleName = StringCipher.Decrypt(x.RoleName)
                 }).ToList()
                };

                ReadOnlyCollection<TimeZoneInfo> tz;
                tz = TimeZoneInfo.GetSystemTimeZones();
                var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
                ViewBag.Timezone = timeZoneList;
                model.TimezoneId = Common.GetTimeZoneStandardIdAndDisplayName(cmodel.TimeZoneString);
                return View(model);
            }
            else
            {
                var titlelist = _titleService.GetTitleList().Where(t => t.Type == (int)RoleType.User).ToList();
                for (int i = 0; i < titlelist.Count; i++)
                {
                    titlelist[i].TitleName = StringCipher.Decrypt(titlelist[i].TitleName);
                }

                var countrylist = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();

                for (int i = 0; i < countrylist.Count; i++)
                {
                    countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
                }

                UserViewModel model = new UserViewModel
                {
                    Countries = countrylist,
                    StateList = new List<LookUpState>(),
                    Cities = new List<LookUpCity>(),
                    TitleList = titlelist,
                    ModuleList =
                        _moduleService.GetmoduleList()
                            .Select(
                                w =>
                                    new ModuleListModel
                                    {
                                        Id = w.Id,
                                        Name = StringCipher.Decrypt(w.ModuleName),
                                        Selected = false,
                                        SubModuleList =
                                            _permissionService.GetPermissionByModule(w.Id)
                                                .Select(
                                                    x =>
                                                        new SubModuleListModel
                                                        {
                                                            Id = x.Id,
                                                            Name = x.PermissionName,
                                                            Selected = false
                                                        })
                                                .ToList()
                                    })
                            .ToList(),
                    RoleList = _roleService.GetUserRolesList().Where(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Moc Admin")).Select(x => new RoleListViewModel
                    {
                        Id = x.Id,
                        RoleName = StringCipher.Decrypt(x.RoleName)
                    }).ToList(),
                };
                ReadOnlyCollection<TimeZoneInfo> tz;
                tz = TimeZoneInfo.GetSystemTimeZones();
                var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
                ViewBag.Timezone = timeZoneList;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult AddUser(UserViewModel model, HttpPostedFileBase ProfilePic)
        {
            UserViewModel cModel = new UserViewModel();
            AdminUser obj = new AdminUser();
            obj.FirstName = StringCipher.Decrypt(model.FirstName);
            obj.SurName = StringCipher.Decrypt(model.SurName);
            obj.Email = StringCipher.Decrypt(model.EmailAddress);
            obj.DOB = Convert.ToDateTime(model.DobInString);
            obj.PhoneNumber = StringCipher.Decrypt(model.PhoneNumber);
            obj.PostCode = StringCipher.Decrypt(model.PostCode);
            obj.CreatedBy = base.loginUserModel.LoginId;
            obj.Address1 = StringCipher.Decrypt(model.Address1);
            obj.OrgUsreRole = model.OrgUsreRole;
            var userName = StringCipher.Decrypt(model.EmailAddress);
            if (model.MemberId == 0)
            {
                if (_userService.CheckDuplicateMail(model.EmailAddress))
                {
                    ModelState.AddModelError("Email", "User with this email id already exists.");
                    ViewBag.ErrorMessage = "This email address already exists.";
                    ReadOnlyCollection<TimeZoneInfo> tz;
                    tz = TimeZoneInfo.GetSystemTimeZones();
                    var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
                    ViewBag.Timezone = timeZoneList;
                    model.TitleList = _titleService.GetTitleList().Where(t => t.Type == (int)RoleType.Patient).ToList();
                    for (int i = 0; i < model.TitleList.Count; i++)
                    {
                        model.TitleList[i].TitleName = StringCipher.Decrypt(model.TitleList[i].TitleName);
                    }
                    var countrylist = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();
                    for (int i = 0; i < countrylist.Count; i++)
                    {
                        countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
                    }
                    model.Countries = countrylist;
                    var Citylist = _cityService.GetCityList().Where(x => x.IsActive == true).ToList();
                    for (int i = 0; i < Citylist.Count; i++)
                    {
                        Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
                    }
                    //model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
                    //model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();          
                    var Statelist = _stateService.GetStateList().Where(x => x.IsActive == true).ToList();
                    for (int i = 0; i < Statelist.Count; i++)
                    {
                        Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
                    }
                    model.ModuleList = _moduleService.GetmoduleList().Select(w => new ModuleListModel { Id = w.Id, Name = StringCipher.Decrypt(w.ModuleName), Selected = false, SubModuleList = _permissionService.GetPermissionByModule(w.Id).Select(x => new SubModuleListModel { Id = x.Id, Name = StringCipher.Decrypt(x.PermissionName), Selected = false }).ToList() }).ToList();
                    model.UserRoleList = _roleService.GetUserRolesList();
                    return View(model);
                }
                //if (UserService.CheckDuplicateUserName(model.EmailAddress))
                //{
                //    ViewBag.ErrorMessage = "This email address already exists.";
                //    return View(model);
                //}
                if (cModel != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                    //UploadImage(model, ProfilePic);
                    BaseMember profile = null;
                    profile = _loginHelper.GetAdminUser(model);

                    var user = _loginHelper.GetAminUserLogin(profile, model.EmailAddress, model, RoleType.ADMIN);
                    var randomPassword = RandomPassword.Generate(6, 8);
                    user.LoginPassword = randomPassword;
                    //user.LookUpRoleId = (int)RoleType.User;

                    _userService.Add(user);

                    foreach (var item in model.ModuleList)
                    {
                        if (item.Selected)
                        {
                            var permission = new PermissionInModule();
                            if (item.SubModuleList != null)
                            {
                                foreach (var inneritem in item.SubModuleList)
                                {
                                    if (inneritem.Selected)
                                    {
                                        permission.LoginId = user.LoginId;
                                        permission.PermissionId = inneritem.Id;
                                        permission.ModuleId = item.Id;
                                        permission.UserRoleId = model.OrgUsreRole;
                                        permission.IsApproved = true;
                                        permission.IsActive = true;
                                        permission.IsDeleted = false;
                                        _permissionInModuleService.Add(permission);
                                    }
                                }
                            }
                        }
                    }

                    string activationToken = user.ActivationToken.ToString();
                    string link = ConfigurationManager.AppSettings["SitePath"] + "Account/Activate/" + activationToken;
                    var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
                    string html = System.IO.File.ReadAllText(fileUrl);
                    html = html.Replace("@@ActivationLink", link).Replace("Your temparory Password is :- @@Password", "");
                    var email = new EmailService();
                    email.SendEmail("User Registration", html, StringCipher.Decrypt(user.Email), Common.fromEmailAddress);
                    return RedirectToAction("UsersList");
                }
            }
            else
            {
                if (model.MemberId > 0)
                {
                    _permissionInModuleService.DeletePermissionForUser(model.LoginId);
                    var avatar = _userService.Find(model.LoginId);
                    if (avatar != null)
                    {
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimezoneId);
                        TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
                        avatar.TimeZoneString = Common.GetTimeZoneOffset(offset);
                    }

                    if (ProfilePic != null && ProfilePic.ContentLength > 0)
                    {
                        //Uploading image if new image uploaded.                         
                       
                        if (avatar != null)
                        {
                            avatar.Avatar = UploadImage(model, ProfilePic).ProfilePic;
                            _userService.Update(avatar);
                        }
                       
                    }
                    else if (avatar != null)
                    {
                        _userService.Update(avatar);
                    }
                    if (avatar.LookUpRoleId == 1)
                    {
                        obj.OrgUsreRole = avatar.LookUpRoleId;
                    }
                    else
                    {
                        var mocAdmin = StringCipher.Encrypt("MOC Admin");
                        var roleName = StringCipher.Encrypt("Admin");

                        var userRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == mocAdmin && x.RoleName == roleName);
                        if (userRole != null)
                        {
                            obj.OrgUsreRole = userRole.Id;
                        }
                        else { obj.OrgUsreRole = (int)RoleType.ADMIN; }
                    }

                    avatar.Member.FirstName = model.FirstName;
                    avatar.Member.SurName = model.SurName;
                    avatar.Member.Email = model.EmailAddress;
                    avatar.LookUpRoleId = obj.OrgUsreRole;
                    avatar.Member.PhoneNumber = model.PhoneNumber;
                    avatar.Member.FaxNumber = model.FaxNumber;
                    avatar.Member.DOB = Convert.ToDateTime(model.DobInString);
                    avatar.Member.CountryId = model.CountryId;
                    avatar.Member.State = model.State;
                    avatar.Member.City = model.City;
                    avatar.Member.Address1 = model.Address1;
                    avatar.Member.Address2 = model.Address2;
                    avatar.Member.ModifiedDate = DateTime.UtcNow;
                    avatar.Member.ModifiedBy = base.loginUserModel.LoginId;
                    avatar.Member.PostCode = model.PostCode;
                  //  avatar.Member.OrgUsreRole = model.OrgUsreRole;
                    avatar.Member.IsActive = true;
                    avatar.Member.IsDeleted = false;
                    avatar.Member.IsApproved = true;

                    TimeZoneInfo tzi1 = TimeZoneInfo.FindSystemTimeZoneById(model.TimezoneId);
                    TimeSpan offset1 = tzi1.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
                    avatar.Member.TimeZoneString = Common.GetTimeZoneOffset(offset1);

                    _userService.UpdateMember(avatar);
                    foreach (var item in model.ModuleList)
                    {
                        if (item.Selected)
                        {
                            var permission = new PermissionInModule();
                            if (item.SubModuleList != null)
                            {
                                foreach (var inneritem in item.SubModuleList)
                                {
                                    if (inneritem.Selected)
                                    {
                                        permission.LoginId = avatar.LoginId;
                                        permission.PermissionId = inneritem.Id;
                                        permission.ModuleId = item.Id;
                                        permission.UserRoleId = model.OrgUsreRole;
                                        _permissionInModuleService.Add(permission);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ReadOnlyCollection<TimeZoneInfo> tzs;
            tzs = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList1 = tzs.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList1;
            model.TitleList = _titleService.GetTitleList().Where(t => t.Type == (int)RoleType.Patient).ToList();
            model.Countries = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();
            model.Cities = _cityService.GetCityList().Where(x => x.IsActive == true).ToList();
            model.StateList = _stateService.GetStateList().Where(x => x.IsActive == true).ToList();
            model.ModuleList = _moduleService.GetmoduleList().Select(w => new ModuleListModel { Id = w.Id, Name = w.ModuleName, Selected = false, SubModuleList = _permissionService.GetPermissionByModule(w.Id).Select(x => new SubModuleListModel { Id = x.Id, Name = x.PermissionName, Selected = false }).ToList() }).ToList();
            model.UserRoleList = _roleService.GetUserRolesList();
            return RedirectToAction("UsersList");
        }

        public ActionResult Index()
        {
            var organizations = _organiztionService.OrganizationCount();
            var clinicCount = _clinicService.ClinicCount();
            var patientCount = _userService.PatientCount();
            var DoctorCount = _doctorService.DoctorCount();
            var AppointmentCount = _appointmentService.AppointmentCount();

            DashboardViewModel model = new DashboardViewModel
            {
                ClinicCount = clinicCount,
                OrganizationCount = organizations,
                PatientCount = patientCount,
                DoctorCount = DoctorCount,
                AppointmentCount = AppointmentCount
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordChange(ChangePasswordViewModel Cmodel)
        {
            if (ModelState.IsValid)
            {
                var loginid = _userService.Find(base.loginUserModel.LoginId);
                var oldpassword = _userService.Find(loginid.LoginName, Cmodel.OldPassword);
                if (oldpassword == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("OldPassword", "Current Password Does not Exsit");
                    return View(new ChangePasswordViewModel());
                }
                ChangePassword(Cmodel);
                ModelState.Clear();
                ModelState.AddModelError("Done", "Password Change Successfully..!!!");
            }

            return View(new ChangePasswordViewModel());
        }

        public JsonResult PasswordReset(string Newpassword, string RetypePassword, Guid UserLoginID)
        {
            var user = _userService.Find(UserLoginID);
            var msg = "N";
            if (Newpassword == RetypePassword && user != null)
            {
                ResetPassword(Newpassword, RetypePassword, UserLoginID);
                msg = "Y";
            }
            else
            {
                msg = "N";
                //return Json(msg, JsonRequestBehavior.AllowGet);

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUser(FormCollection collection)
        {
            var ids = collection[0];
            _adminUserService.Delete(ids);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult AcitvateUser(FormCollection collection)
        {
            var ids = collection[0];
            _adminUserService.Activate(ids);
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion

        #region "USER DEFINED FUNCTIONS"
        public UserViewModel UploadImage(UserViewModel model, HttpPostedFileBase profilePic)
        {
            if (profilePic != null && profilePic.ContentLength > 0)
            {
                var fileName = Path.GetFileName(profilePic.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    string fName = Convert.ToString(DateTime.Now.ToString("ddMMyyyy-HHmmss-fff")) + extension;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/UserProfile/"), fName);
                    profilePic.SaveAs(path);
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

        private UserViewModel BindDropDown(UserViewModel userViewModel)
        {
            return userViewModel;
        }
        #endregion
    }
}