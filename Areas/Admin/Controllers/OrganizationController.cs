using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Emailer;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Web.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    [AuthorizeRole("Admin")]
    public class OrganizationController : BaseController
    {
        readonly IOrganizationService _organizationService;
        readonly ICountryService _countryService;
        readonly ICityService _cityService;
        readonly IUserService _userService;
        readonly IOrganizationUserService _orgUserService;
        readonly IClinicService _clinicService;
        readonly ILoginHelper _loginService;
        readonly IOrganizationTypeService _organizationTypeService;
        readonly ILookUpModuleService _moduleService;
        readonly IPermissionInModuleService _permissioninmoduleService;
        readonly IPermissionsService _permissionService;
        readonly ILookUpUserRolesService _roleService;
        readonly ITitleService _TitleServices;
        readonly IGenderService _GenderService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IStateService _stateservice;
        public OrganizationController(IOrganizationService organizationService, ICountryService countryService, ICityService cityService,
            IUserService userService, IOrganizationUserService orgUserService, ILoginHelper loginService, IClinicService clinicService,
            IOrganizationTypeService organizationTypeService, ITimeZoneService timeZoneService, ILookUpUserRolesService roleService,
             ILookUpModuleService moduleService, IPermissionInModuleService permissioninmoduleService, IPermissionsService permissionService, IStateService stateservice, ITitleService TitleServices, IGenderService GenderService
           )
        {
            _organizationService = organizationService;
            _countryService = countryService;
            _stateservice = stateservice;
            _cityService = cityService;
            _userService = userService;
            _orgUserService = orgUserService;
            _loginService = loginService;
            _clinicService = clinicService;
            _organizationTypeService = organizationTypeService;
            _moduleService = moduleService;
            _permissioninmoduleService = permissioninmoduleService;
            _permissionService = permissionService;
            _roleService = roleService;
            _timeZoneService = timeZoneService;
            _TitleServices = TitleServices;
            _GenderService = GenderService;
        }
        //
        // GET: /Admin/Organization/
        public ActionResult Index()
        {
            OrganizationViewModel model = new OrganizationViewModel();
            var organizations = _organizationService.GetOrganizationList().Select(x =>
                new OrganizationViewModel
                {
                    OrganizationName = StringCipher.Decrypt(x.OrganizationName) == null ? string.Empty : StringCipher.Decrypt(x.OrganizationName),
                    OrganizationId = x.OrganizationId,
                    OrganizationType = x.OrganizationType,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1) == null ? string.Empty : StringCipher.Decrypt(x.Address1),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber) == null ? string.Empty : StringCipher.Decrypt(x.FaxNumber),
                    PostCode = StringCipher.Decrypt(x.PostCode) == null ? string.Empty : StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2) == null ? string.Empty : StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber) == null ? string.Empty : StringCipher.Decrypt(x.PhoneNumber),
                    OrganizationLogo = x.OrganizationLogo,
                  
                    // OrgAdminName = _orgUserService.GetByOrganization(x.OrganizationId) == null ? "" : StringCipher.Decrypt(_orgUserService.GetByOrganization(x.OrganizationId).FirstOrDefault().FirstName.ToString()),
                     LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    ClinicList = _clinicService.GetClinicInOrganization(x.OrganizationId).ToList(),
                    IsActive = x.IsActive,
                    
                    //OrganizationTypeName =x.OrganizationType==null?"":StringCipher.Decrypt(_organizationTypeService.GetOrganizarionTypesById(x.OrganizationType).OrganizationTypeName),
                }).ToList();

            for (int i = 0; i < organizations.Count; i++)
            {
                for (int j = 0; j < organizations[i].ClinicList.Count; j++)
                {
                    organizations[i].ClinicList[j].ClinicName = StringCipher.Decrypt(organizations[i].ClinicList[j].ClinicName);
                }
                if (organizations[i].OrganizationType > 0)
                {
                    organizations[i].OrganizationTypeName = StringCipher.Decrypt(_organizationTypeService.GetOrganizarionTypesById(organizations[i].OrganizationType).OrganizationTypeName);
                }
                if (_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault() != null)
                {
                    var titles = _orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().Title;
                    try
                    {
                        if (_TitleServices.GetTitleList().Where(x => x.TitleId == titles && x.Type == (int)RoleType.ADMIN).FirstOrDefault() != null)
                        {
                            string titlename = StringCipher.Decrypt(_TitleServices.GetTitleList().Where(x => x.TitleId == titles && x.Type == (int)RoleType.ADMIN).FirstOrDefault().TitleName);

                            organizations[i].OrgAdminName = _orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().SurName == null ? titlename + " " + StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().FirstName.ToString()) + " " + StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().SurName.ToString()) : titlename + " " + StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().FirstName.ToString()) + " " +StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().MiddleName.ToString()) +" "+ StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().SurName.ToString());
                        }
                        //if (!string.IsNullOrEmpty(titlename))
                        //{
                        //    organizations[i].OrgAdminName = titlename + " " + StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().FirstName.ToString());
                        //}
                        else
                        {
                            organizations[i].OrgAdminName = StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().FirstName.ToString()) + " " + StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().SurName.ToString());
                        }
                    }
                    catch { }
                }
            }


            //for (int i = 0; i < organizations.count; i++)
            //{
            //    var cliniclist = _clinicservice.getclinicinorganization(organizations[i].organizationid).tolist();
            //    if (cliniclist != null)
            //    {
            //        for (int j = 0; j < cliniclist.count; j++)
            //        {
            //            cliniclist[j].clinicname = stringcipher.decrypt(cliniclist[j].clinicname);
            //        }
            //    }
            //    model.cliniclist = cliniclist;
            //}


            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId);
            return View(organizations);
        }
        public OrganizationViewModel getcliniclist(int orgid)
        {
            OrganizationViewModel org = new OrganizationViewModel();
            org.ClinicList = _clinicService.GetClinicInOrganization(orgid).ToList();
            for (int i = 0; i < org.ClinicList.Count; i++)
            {
                org.ClinicList[i].ClinicName = StringCipher.Decrypt(org.ClinicList[i].ClinicName);
            }
            return org;
        }
        [HttpPost]
        public ActionResult Index(SearchParametersViewModel searchModel)
        {
            var query = _organizationService.GetOrganizationList();
            for (int i = 0; i < query.Count; i++)
            {
                query[i].OrganizationName = StringCipher.Decrypt(query[i].OrganizationName);
                query[i].PostCode = StringCipher.Decrypt(query[i].PostCode);

                if (query[i].LoginId != new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    query[i].FaxNumber = Convert.ToString(_userService.Find(query[i].LoginId).LookUpRoleId);
                }
                //query[i].FaxNumber = Convert.ToString(_userService.Find(query[i].LoginId).LookUpRoleId);
                if (_orgUserService.GetByOrganization(query[i].OrganizationId) != null)
                {
                    query[i].Address2 = StringCipher.Decrypt(_orgUserService.GetByOrganization(query[i].OrganizationId).FirstOrDefault().FirstName) + " " + StringCipher.Decrypt(_orgUserService.GetByOrganization(query[i].OrganizationId).FirstOrDefault().SurName);
                }
            }
            var ObjFilter = new FilterParameters
            {
                OrganizationName = searchModel.OrgName,
                OrgType = searchModel.OrgType,
                PostCode = searchModel.PostCode,
                CountryId = searchModel.CountryId,
                StateId = searchModel.StateId,
                OrgAdmin = searchModel.OrgAdmin,
                ActivationStatus = searchModel.ActivationStatus,
                AdminName = searchModel.AdminName

            };
            query = query.FilterByOrganization(ObjFilter);
            //I am using Fax Number to get RoleType for searching purpose         
            var organizations = query.Select(x =>
                new OrganizationViewModel
                {
                    OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                    OrganizationId = x.OrganizationId,
                    OrganizationType = x.OrganizationType,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = x.FaxNumber,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = x.PhoneNumber,
                    OrganizationLogo = x.OrganizationLogo,
                    // OrgAdminName = _orgUserService.GetByOrganization(x.OrganizationId).FirstOrDefault().FirstName.ToString(),
                    ModifiedDate = x.ModifiedDate,
                    ClinicList = _clinicService.GetClinicInOrganization(x.OrganizationId).ToList(),
                    IsActive = x.IsActive,
                    OrganizationTypeName = _organizationTypeService.GetOrganizarionTypesById(x.OrganizationType).OrganizationTypeName,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)
                }).ToList();
            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId);
            for (int i = 0; i < organizations.Count; i++)
            {
                for (int j = 0; j < organizations[i].ClinicList.Count; j++)
                {
                    organizations[i].ClinicList[j].ClinicName = StringCipher.Decrypt(organizations[i].ClinicList[j].ClinicName);
                }
                if (organizations[i].OrganizationType > 0)
                {
                    organizations[i].OrganizationTypeName = StringCipher.Decrypt(_organizationTypeService.GetOrganizarionTypesById(organizations[i].OrganizationType).OrganizationTypeName);
                }
                if (_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault() != null)
                {
                    if (_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().FirstName != null)
                    {
                        organizations[i].OrgAdminName = StringCipher.Decrypt(_orgUserService.GetByOrganization(organizations[i].OrganizationId).FirstOrDefault().FirstName.ToString());
                    }
                }
            }
            return View(organizations);
        }
        public ActionResult OrganizationUserList(int id)
        {
           // var titles= 
            var orgUsers = _orgUserService.GetByOrganization(id).Select(x => new OrganizationUserViewModel
            {
                //FirstName = StringCipher.Decrypt(x.FirstName),
                FirstName = x.MiddleName != null && _TitleServices.GetTitleList().Where(y => y.TitleId == id && y.Type == (int)RoleType.ADMIN).FirstOrDefault() != null ? StringCipher.Decrypt(_TitleServices.GetTitleList().Where(y => y.TitleId == id && y.Type == (int)RoleType.ADMIN).FirstOrDefault().TitleName) + " " + StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.MiddleName) + " " + StringCipher.Decrypt(x.SurName) : StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                // SurName = StringCipher.Decrypt(x.SurName),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                MiddleName = StringCipher.Decrypt(x.MiddleName),
                PostCode = StringCipher.Decrypt(x.PostCode),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                TimeZone = StringCipher.Decrypt(x.TimeZoneString),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                CountryId = x.CountryId,
                City = x.City,
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                UserId = x.MemberId,
                IsActive = x.IsActive,
                ProfilePic = _userService.Find(x.LoginId).Avatar,
                RoleName = StringCipher.Decrypt(_roleService.GetUserRolesById(x.OrgUsreRole).RoleName),
                LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)
            }).ToList();
            return View(orgUsers);
        }
        [HttpPost]
        public ActionResult OrganizationUserList(int id, SearchParametersViewModel searchModel)
        {

            var query = _orgUserService.GetByOrganization(id).ToList();
            //string organizationname = "";
            for (int i = 0; i < query.Count; i++)
            {

                query[i].Address1 = StringCipher.Decrypt(_organizationService.GetOrganizationById(query[i].OrgId).OrganizationName);
                query[i].PostCode = StringCipher.Decrypt(query[i].PostCode);
                query[i].FaxNumber = Convert.ToString(_userService.Find(query[i].LoginId).LookUpRoleId);
                query[i].Email = StringCipher.Decrypt(query[i].Email);
                query[i].FirstName = query[i].MiddleName != null ? StringCipher.Decrypt(query[i].FirstName) + " " + StringCipher.Decrypt(query[i].MiddleName) + " " + StringCipher.Decrypt(query[i].SurName) : StringCipher.Decrypt(query[i].FirstName) + " " + StringCipher.Decrypt(query[i].SurName);
                //address 2for user role 
                query[i].Address2 = StringCipher.Decrypt(_roleService.GetUserRolesList().Where(x => x.UserRoleTypeId == x.UserRoleTypeId).FirstOrDefault().RoleName);
                // query[i].SurName =  StringCipher.Decrypt(query[i].SurName);
                // query[i].fi
                // _userService.Find(query[i].LoginId).LookUpRoleId;
            }

            var ObjFilter = new FilterParameters
            {
                OrganizationName = searchModel.OrgName,
                OrgType = searchModel.OrgType,
                PostCode = searchModel.PostCode,
                CountryId = searchModel.CountryId,
                StateId = searchModel.StateId,
                OrgAdmin = searchModel.OrgAdmin,
                ActivationStatus = searchModel.ActivationStatus,
                staffName = searchModel.StaffName,
                Email = searchModel.Email,
                Role = searchModel.Role
            };

            query = query.Where(x =>
                (
                 (
                 searchModel.OrgName != null && x.Address1.IndexOf(searchModel.OrgName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.OrgName == null)
                 &&
                    ((searchModel.StaffName != null && x.FirstName.IndexOf(searchModel.StaffName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.StaffName == null)
                    &&
                    ((searchModel.Email != null && x.Email.IndexOf(searchModel.Email, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.Email == null)
                     &&
                    ((searchModel.RoleName != null && x.Address2.IndexOf(searchModel.RoleName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.RoleName == null)
                   &&
                   ((searchModel.Role != 0 && x.OrgUsreRole == searchModel.Role) || searchModel.Role == 0)
                   &&
                   ((searchModel.ActivationStatus != null && x.IsActive == Convert.ToBoolean(searchModel.ActivationStatus)) ||
                         searchModel.ActivationStatus == null)
                  ).ToList();
            var orgUsers = query.Select(x => new OrganizationUserViewModel
            {
                FirstName = StringCipher.Decrypt(x.FirstName),
                SurName = StringCipher.Decrypt(x.SurName),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                MiddleName = StringCipher.Decrypt(x.MiddleName),
                PostCode = StringCipher.Decrypt(x.PostCode),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                TimeZone = StringCipher.Decrypt(x.TimeZoneString),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                CountryId = x.CountryId,
                City = x.City,
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                UserId = x.MemberId,
                IsActive = x.IsActive,
                ProfilePic = _userService.Find(x.LoginId).Avatar,
                RoleName = StringCipher.Decrypt(_roleService.GetUserRolesById(x.OrgUsreRole).RoleName),
                LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)
            }).ToList();

            return View(orgUsers);
        }
        public ActionResult UserDetails(int id)
        {
            var userDetails = _orgUserService.GetUserById(id);
            var usermodel = new OrganizationUserViewModel();
            usermodel.FirstName = StringCipher.Decrypt(userDetails.FirstName);
            usermodel.SurName = StringCipher.Decrypt(userDetails.SurName);
            usermodel.Email = StringCipher.Decrypt(userDetails.Email);
            usermodel.DOB = userDetails.DOB;
            usermodel.Address1 = StringCipher.Decrypt(userDetails.Address1);
            usermodel.Address2 = StringCipher.Decrypt(userDetails.Address2);
            usermodel.PhoneNumber = StringCipher.Decrypt(userDetails.PhoneNumber);
            usermodel.PostCode = StringCipher.Decrypt(userDetails.PostCode);
            usermodel.FaxNumber = StringCipher.Decrypt(userDetails.FaxNumber);
            usermodel.MobileNumber = StringCipher.Decrypt(userDetails.MobileNumber);
            usermodel.ModifiedDate = userDetails.ModifiedDate ?? DateTime.Now;
            usermodel.ProfilePic = _userService.Find(userDetails.LoginId).Avatar;
            usermodel.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(userDetails.CountryId).CountryName);
            usermodel.StateName = StringCipher.Decrypt(_stateservice.GetStateById(userDetails.State).StateName);
            usermodel.CityName = StringCipher.Decrypt(_cityService.GetCityById(userDetails.City).LookUpCityName);
            usermodel.Gender =userDetails.Gender !=null? StringCipher.Decrypt(_GenderService.GetGenderById(Convert.ToInt32(userDetails.Gender)).GenderName):string.Empty;
            return View(usermodel);
        }
        //public int organizationclinic(int organizationid)
        //{
        //    var clinics = _clinicService.GetClinicInOrganization(x.OrganizationId).ToList();
        //    for(int i=0;i<clinics.Count;i++)
        //    {

        //    }
        //}
        public ActionResult Add(int? id)
        {
            OrganizationViewModel cModel = new OrganizationViewModel();
            OrganizationUserViewModel userModel = new OrganizationUserViewModel();
            cModel.OrganizationUser = userModel;
            if (id.HasValue)
            {
                var model = _organizationService.GetOrganizationById(Convert.ToInt32(id));
                cModel = new OrganizationViewModel(model)
                {
                    OrganizationUser = _orgUserService.GetByOrganization(Convert.ToInt32(id)) == null ? new OrganizationUserViewModel() :
                        _orgUserService.GetByOrganization(Convert.ToInt32(id)).Select(x => new OrganizationUserViewModel
                        {
                            Titleid = x.Title,
                            FirstName = StringCipher.Decrypt(x.FirstName),
                            MiddleName = StringCipher.Decrypt(x.MiddleName),
                            SurName = StringCipher.Decrypt(x.SurName),
                            State = x.State,
                            CountryId = x.CountryId,
                            City = x.CountryId,
                            Address1 = StringCipher.Decrypt(x.Address1),
                            Address2 = StringCipher.Decrypt(x.Address2),
                            PostCode = StringCipher.Decrypt(x.PostCode),
                            FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                            PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                            Email = StringCipher.Decrypt(x.Email),
                            DOB = x.DOB,
                            UserId = x.MemberId,
                            OrgId = x.OrgId,
                            ProfilePic = _userService.Find(x.LoginId).Avatar,
                            LoginId = x.LoginId,
                            State1 = x.State,
                            City1 = x.City
                        }).FirstOrDefault()
                };
                if (cModel.OrganizationUser == null)
                {
                    OrganizationUserViewModel uModel = new OrganizationUserViewModel();
                    cModel.OrganizationUser = uModel;
                }
                cModel.PhoneNumber = StringCipher.Decrypt(cModel.PhoneNumber);
                cModel.FaxNumber = StringCipher.Decrypt(cModel.FaxNumber);
                cModel.TimeZoneId = Common.GetTimeZoneStandardIdAndDisplayName(model.TimeZoneString);
                BindDropDownLists(cModel);
                return View(cModel);
            }
            else
            {
                BindDropDownLists(cModel);
                return View(cModel);
            }
        }
        //
        // POST: /Admin/Country/Create
        [HttpPost]
        public ActionResult Add(OrganizationViewModel model, HttpPostedFileBase ProfilePic, HttpPostedFileBase OrganizationLogo)
        {
            try
            {
                var orgName = StringCipher.Encrypt("Organization");
                var adminName = StringCipher.Encrypt("Admin");
                string randomPassword = "";
                Organization cModel = new Organization();
                cModel = model.GetEditModel(model);

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneId);
                TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
                cModel.TimeZoneString = Common.GetTimeZoneOffset(offset);

                if (cModel.OrganizationId > 0)
                {
                    //Created object of organization user
                    if (_orgUserService.GetByOrganization(Convert.ToInt32(cModel.OrganizationId)).FirstOrDefault() == null)
                    {


                        var userRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == orgName && x.RoleName == adminName);
                        if (userRole != null)
                        {
                            model.OrganizationUser.OrgUsreRole = userRole.Id;
                        }
                        else { model.OrganizationUser.OrgUsreRole = (int)RoleType.OrganizationAdmin; }
                        var userName = model.OrganizationUser.Email;
                        var userEmailExists = _userService.CheckDuplicateMail(userName);
                        if (userEmailExists)
                        {
                            ModelState.AddModelError("Email", "User with this email id already exists.");
                            ViewBag.ErrorMessage = "This email address already exists.";
                            BindDropDownLists(model);
                            return View(model);
                        }
                        if (model.OrganizationUser != null)
                        {

                            cModel.CreatedBy = base.loginUserModel.LoginId;
                            UploadLogo(model, OrganizationLogo);
                            cModel.OrganizationLogo = model.OrganizationLogo;
                            cModel.ModifiedDate = DateTime.UtcNow;
                            _organizationService.Update(cModel);

                            UploadImage(model.OrganizationUser, ProfilePic);
                            BaseMember profile = null;

                            profile = _loginService.GetOrganizationAdmin(model, cModel.OrganizationId);
                            var user = _loginService.GetOrganizationAdminLogin(profile, model.OrganizationUser.Email, model);
                            randomPassword = RandomPassword.Generate(6, 8);
                            user.LoginPassword = randomPassword;
                            _userService.Add(user);
                            var org = _organizationService.GetOrganizationById(cModel.OrganizationId);
                            if (org != null)
                            {
                                int userRoleId = 0;
                                org.LoginId = user.LoginId;
                                _organizationService.Update(org);
                                var clinicAdminRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == orgName && x.RoleName == adminName);
                                if (clinicAdminRole != null)
                                {
                                    userRoleId = clinicAdminRole.Id;
                                }
                                var modulePermissions = _permissionService.GetPermissionList().Where(x => x.ModuleId != 3);
                                var permission = new PermissionInModule();

                                foreach (var item in modulePermissions)
                                {
                                    if (item.PermissionName == "Add Organisations" || item.PermissionName == "Edit Organisations" ||
                                        item.PermissionName == "View Organizations" || item.PermissionName == "View Organization Details" ||
                                        item.PermissionName == "Active/Inactive Organisations")
                                    {
                                        continue;
                                    }

                                    permission.LoginId = user.LoginId;
                                    permission.PermissionId = item.Id;
                                    permission.ModuleId = item.ModuleId;
                                    permission.UserRoleId = userRoleId;
                                    permission.IsApproved = true;
                                    permission.IsActive = true;
                                    permission.IsDeleted = true;
                                    _permissioninmoduleService.Add(permission);
                                }
                            }
                            string activationToken = user.ActivationToken.ToString();
                            string link = ConfigurationManager.AppSettings["SitePath"] + "Account/Activate/" + activationToken;
                            var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
                            string html = System.IO.File.ReadAllText(fileUrl);
                            //html = html.Replace("@@Password", randomPassword).Replace("@@ActivationLink", link);
                            html = html.Replace("@@Password", "").Replace("@@ActivationLink", link);
                            EmailService objEmail = new EmailService();
                            objEmail.SendEmail("User Registration", html, userName, Common.fromEmailAddress);

                        }
                    }
                    else
                    {
                        OrganizationUsers oModel = new OrganizationUsers();
                        var omodels = _orgUserService.GetByOrganization(model.OrganizationId).FirstOrDefault();
                        omodels = model.OrganizationUser.GetEditModel(model.OrganizationUser);
                        omodels.DOB = DateTime.Now;
                        omodels.OrgId = model.OrganizationId;
                        omodels.OrgUsreRole = (int)RoleType.OrganizationAdmin;

                        if (ProfilePic != null && ProfilePic.ContentLength > 0)
                        {
                            //Uploading image if new image uploaded.                         
                            var avatar = _userService.Find(model.LoginId);
                            if (avatar != null)
                            {
                                avatar.Avatar = UploadImage(model.OrganizationUser, ProfilePic).ProfilePic;
                                _userService.Update(avatar);
                            }
                        }
                        var userRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == orgName && x.RoleName == adminName);
                        if (userRole != null)
                        {
                            omodels.OrgUsreRole = userRole.Id;
                        }
                        else { omodels.OrgUsreRole = (int)RoleType.OrganizationAdmin; }
                        try
                        {
                            _orgUserService.Update(omodels);
                        }
                        catch { }

                        if (OrganizationLogo != null && OrganizationLogo.ContentLength > 0)
                        {
                            if (OrganizationLogo != null && OrganizationLogo.ContentLength > 0)
                            {
                                //Uploading image if new image uploaded.
                                UploadLogo(model, OrganizationLogo);
                                cModel.OrganizationLogo = model.OrganizationLogo;
                            }
                        }
                    }
                    //updating organization
                    _organizationService.Update(cModel);
                }
                else
                {

                    var isDuplicate = false;
                    var orgNameList = _organizationService.GetOrganizationList();

                    if (orgNameList != null && orgNameList.Count > 0)
                    {
                        var org = orgNameList.Where(t => t.OrganizationName == StringCipher.Encrypt(model.OrganizationName)).ToList();

                        if (org != null && org.Count > 0)
                        {
                            isDuplicate = true;
                        }
                    }

                    if (isDuplicate)
                    {
                        ModelState.AddModelError("Email", "Same Organisation name can't be added");
                        ViewBag.ErrorMessage = "Same Organisation name can't be added";
                        BindDropDownLists(model);
                        return View(model);
                    }

                    var userRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == orgName && x.RoleName == adminName);
                    if (userRole != null)
                    {
                        model.OrganizationUser.OrgUsreRole = userRole.Id;
                    }
                    else { model.OrganizationUser.OrgUsreRole = (int)RoleType.OrganizationAdmin; }
                    var userName = model.OrganizationUser.Email;
                    var userEmailExists = _userService.CheckDuplicateMail(StringCipher.Encrypt(userName));
                    if (userEmailExists)
                    {
                        ModelState.AddModelError("Email", "User with this email id already exists.");
                        ViewBag.ErrorMessage = "This email address already exists.";
                        BindDropDownLists(model);
                        return View(model);
                    }

                    if (model.OrganizationUser != null)
                    {

                        cModel.CreatedBy = base.loginUserModel.LoginId;
                        UploadLogo(model, OrganizationLogo);
                        cModel.OrganizationLogo = model.OrganizationLogo;
                        _organizationService.Add(cModel);

                        UploadImage(model.OrganizationUser, ProfilePic);
                        BaseMember profile = null;

                        profile = _loginService.GetOrganizationAdmin(model, cModel.OrganizationId);
                        var user = _loginService.GetOrganizationAdminLogin(profile, model.OrganizationUser.Email, model);
                        randomPassword = RandomPassword.Generate(6, 8);
                        user.LoginPassword = randomPassword;
                        user.Member.City = model.City;
                        user.Member.State = model.State;
                        _userService.Add(user);
                        var org = _organizationService.GetOrganizationById(cModel.OrganizationId);
                        if (org != null)
                        {
                            int userRoleId = 0;
                            org.LoginId = user.LoginId;
                            _organizationService.Update(org);
                            var clinicAdminRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == orgName && x.RoleName == adminName);
                            if (clinicAdminRole != null)
                            {
                                userRoleId = clinicAdminRole.Id;
                            }

                            string[] arrPermission = new string[] { "Add Organisations", "Edit Organisations", "View Organizations", 
                                                    "View Organization Details", "Active/Inactive Organisations" 
                                                  };

                            var modulePermissions = _permissionService.GetPermissionList().Where(x => x.ModuleId != (int)ModuleNames.ManageStaff);
                            var permission = new PermissionInModule();
                            foreach (var item in modulePermissions)
                            {

                                if (arrPermission.Contains(item.PermissionName))
                                {
                                    continue;
                                }
                                permission.LoginId = user.LoginId;
                                permission.PermissionId = item.Id;
                                permission.ModuleId = item.ModuleId;
                                permission.UserRoleId = userRoleId;
                                permission.IsApproved = true;
                                permission.IsActive = true;
                                permission.IsDeleted = true;
                                _permissioninmoduleService.Add(permission);
                            }
                        }
                        string activationToken = user.ActivationToken.ToString();
                        string link = ConfigurationManager.AppSettings["SitePath"] + "Account/Activate/" + activationToken;
                        var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
                        string html = System.IO.File.ReadAllText(fileUrl);
                        html = html.Replace("@@ActivationLink", link).Replace("@@Password", randomPassword);
                        EmailService objEmail = new EmailService();
                        objEmail.SendEmail("User Registration", html, userName, "noreply@myonlineclinic.com.au");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                BindDropDownLists(model);
                return View(model);
            }
        }
        private string FillNewOrgUser(OrganizationViewModel model, HttpPostedFileBase ProfilePic, string randomPassword, Organization cModel, string userName)
        {
            UploadImage(model.OrganizationUser, ProfilePic);
            BaseMember profile = null;

            profile = _loginService.GetOrganizationAdmin(model, cModel.OrganizationId);
            var user = _loginService.GetOrganizationAdminLogin(profile, model.OrganizationUser.Email, model);
            randomPassword = RandomPassword.Generate(6, 8);
            user.LoginPassword = randomPassword;
            _userService.Add(user);
            var org = _organizationService.GetOrganizationById(cModel.OrganizationId);
            if (org != null)
            {
                int userRoleId = 0;
                org.LoginId = user.LoginId;
                _organizationService.Update(cModel);
                var clinicAdminRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Organization") && x.RoleName == StringCipher.Encrypt("Admin"));
                if (clinicAdminRole != null)
                {
                    userRoleId = clinicAdminRole.Id;
                }
                var modulePermissions = _permissionService.GetPermissionList().Where(x => x.ModuleId != 1 && x.ModuleId != 13);
                var permission = new PermissionInModule();
                foreach (var item in modulePermissions)
                {
                    permission.LoginId = user.LoginId;
                    permission.PermissionId = item.Id;
                    permission.ModuleId = item.ModuleId;
                    permission.UserRoleId = userRoleId;
                    _permissioninmoduleService.Add(permission);
                }
            }
            string activationToken = user.ActivationToken.ToString();
            string link = ConfigurationManager.AppSettings["SitePath"] + "Account/Activate/" + activationToken;
            var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@@Password", randomPassword).Replace("@@ActivationLink", link);

            EmailService objEmail = new EmailService();
            objEmail.SendEmail("User Registration", html, userName, "tma@myonlineclinic.com.au");
            return randomPassword;
        }
        public ActionResult AddOrganizationUser()
        {
            OrganizationUserViewModel model = BindDropdownList();
            return View(model);
        }

        private OrganizationUserViewModel BindDropdownList()
        {
            var title = _TitleServices.GetTitleList().Where(x => x.Type == (int)RoleType.User).ToList();
            for (int i = 0; i < title.Count; i++)
            {
                title[i].TitleName = StringCipher.Decrypt(title[i].TitleName);
            }
            //_TitleServices.GetTitleList()

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
            var Statelist = _stateservice.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            OrganizationUserViewModel model = new OrganizationUserViewModel();
            model.TitleList = title;
            model.Countries = countrylist;
            model.StateList = model.UserId > 0 ? Statelist : new List<LookUpState>();
            model.Cities = model.UserId > 0 ? Citylist : new List<LookUpCity>(); _cityService.GetCityList();
            model.OrgDropDown = _organizationService.GetOrganizationList().Select(x => new DropDownModel { Text = StringCipher.Decrypt(x.OrganizationName), Value = x.OrganizationId }).ToList();
            model.UserRoleList = _roleService.GetUserRolesList().Where(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Organization")).Select(x => new OrganizationRoleDecryptMoel
            {
                RoleName = StringCipher.Decrypt(x.RoleName),
                Id = x.Id,
                UserRoleTypeId = x.UserRoleTypeId
            }).ToList();


            string[] arrPermission = new string[] { "Add Organisations", "Edit Organisations", "View Organisations", 
                                                    "View Organisations Details", "Active/Inactive Organisations"                                                     
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
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            return model;
        }
        [HttpPost]
        public ActionResult AddOrganizationUser(OrganizationUserViewModel model, HttpPostedFileBase ProfilePic)
        {
            model.TitleList = _TitleServices.GetTitleList().Where(x => x.Type == (int)RoleType.ADMIN).ToList();
            model.Countries = _countryService.GetCountryList();
            model.StateList = _stateservice.GetStateList();
            model.Cities = _cityService.GetCityList();
            model.OrgDropDown = _organizationService.GetOrganizationList().Select(x => new DropDownModel { Text = StringCipher.Decrypt(x.OrganizationName), Value = x.OrganizationId }).ToList();
            model.UserRoleList = _roleService.GetUserRolesList().Where(x => x.RoleType.RoleTypeName == "Organization").Select(x => new OrganizationRoleDecryptMoel
                {
                    RoleName = StringCipher.Decrypt(x.RoleName),
                    Id = x.Id,
                    UserRoleTypeId = x.UserRoleTypeId
                }).ToList();
            //model.ModuleList = _moduleService.GetmoduleList().Select(w => new ModuleListModel { Id = w.Id, Name = w.ModuleName, SubModuleList = _permissionService.GetPermissionByModule(w.Id).Select(x => new SubModuleListModel { Id = x.Id, Name = x.PermissionName, Selected = false }).ToList() }).ToList();
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            var cModel = model.GetEditModel(model);

            var userName = model.Email;
            if (_userService.CheckDuplicateMail(model.Email))
            {
                ModelState.AddModelError("Email", "User with this email id already exists.");
                ViewBag.ErrorMessage = "This email address already exists.";
                return View(model);
            }
            if (_userService.CheckDuplicateUserName(model.Email))
            {
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
                objEmail.SendEmail("User Registration", html, userName, Common.fromEmailAddress);

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
                                permission.IsDeleted = false;
                                permission.IsApproved = true;
                                permission.IsActive = true;
                                _permissioninmoduleService.Add(permission);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public OrganizationViewModel BindDropDownLists(OrganizationViewModel model)
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
            //model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();         

            var Citylist = _cityService.GetCityList();
            for (int i = 0; i < Citylist.Count; i++)
            {
                Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
            }
            //model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();

            var Statelist = _stateservice.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            var organzationlist = _organizationTypeService.GetOrganizarionTypesList();

            for (int i = 0; i < organzationlist.Count; i++)
            {
                organzationlist[i].OrganizationTypeName = StringCipher.Decrypt(organzationlist[i].OrganizationTypeName);
            }
            model.TitleList = title;
            model.Countries = countrylist;
            model.Cities = model.OrganizationId > 0 ? Citylist : new List<LookUpCity>();
            model.Statelist = model.OrganizationId > 0 ? Statelist : new List<LookUpState>();
            model.OrganizationTypeList = organzationlist;
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            return model;
        }
        public ActionResult ListOrganization()
        {
            var organizations = _organizationService.GetOrganizationList().Select(x =>
                    new OrganizationViewModel
                    {
                        OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                        OrganizationId = x.OrganizationId,
                        OrganizationType = x.OrganizationType,
                        LoginId = x.LoginId,
                        Address1 = StringCipher.Decrypt(x.Address1),
                        FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                        Address2 = StringCipher.Decrypt(x.Address2),
                        PhoneNumber = StringCipher.Decrypt(x.PhoneNumber)
                    }).ToList();
            return View(organizations);
        }
        //organization details
        public ActionResult Details(int id)
        {
            var model = _organizationService.GetOrganizationById(Convert.ToInt32(id));
            model.PhoneNumber = StringCipher.Decrypt(model.PhoneNumber);
            model.FaxNumber = StringCipher.Decrypt(model.FaxNumber);

            var cModel = new OrganizationViewModel(model)
            {
                OrganizationUser =
                    _orgUserService.GetByOrganization(Convert.ToInt32(id)).Select(x => new OrganizationUserViewModel
                    {

                        FirstName = StringCipher.Decrypt(x.FirstName),
                        MiddleName = StringCipher.Decrypt(x.MiddleName),
                        SurName = StringCipher.Decrypt(x.SurName),
                        State = x.State,
                        PostCode = StringCipher.Decrypt(x.PostCode),
                        FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                        CountryId = x.CountryId,
                        City = x.CountryId,
                        Address1 = StringCipher.Decrypt(x.Address1),
                        Address2 = StringCipher.Decrypt(x.Address2),
                        PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                        Email = StringCipher.Decrypt(x.Email),
                        DOB = x.DOB,
                        UserId = x.MemberId,
                        OrgId = x.OrgId,
                        ProfilePic = _userService.Find(x.LoginId).Avatar,
                        //OrganizationLogo=
                        LoginId = x.LoginId,
                        Gender = StringCipher.Decrypt(_countryService.GetCountryById(Convert.ToInt32(x.CountryId)).CountryName)
                    }).FirstOrDefault()
            };


            if (cModel != null)
            {
                cModel.OrgCountryName = _countryService.GetCountryById(cModel.Country).CountryName;
                cModel.OrgStateName = _stateservice.GetStateById(cModel.State).StateName;
                cModel.OrgCityName = _cityService.GetCityById(cModel.City).LookUpCityName;

                cModel.OrgCountryName = StringCipher.Decrypt(cModel.OrgCountryName);
                cModel.OrgStateName = StringCipher.Decrypt(cModel.OrgStateName);
                cModel.OrgCityName = StringCipher.Decrypt(cModel.OrgCityName);
                cModel.OrganizationLogo = cModel.OrganizationLogo;

                if (cModel.OrganizationType == 1)
                {
                    cModel.OrganizationTypeName = "Fully Open";
                }
                else if (cModel.OrganizationType == 2)
                {
                    cModel.OrganizationTypeName = "Fully Restricted";
                }
                else if (cModel.OrganizationType == 3)
                {
                    cModel.OrganizationTypeName = "Partial Open Doctor";
                }
                else if (cModel.OrganizationType == 4)
                {
                    cModel.OrganizationTypeName = "Partial Open Patient";
                }
            }

            return View(cModel);
        }
        public OrganizationUserViewModel BindDropDown(OrganizationUserViewModel model)
        {

            var title = _TitleServices.GetTitleList().Where(x => x.Type == (int)RoleType.User).ToList();
            for (int i = 0; i < title.Count; i++)
            {
                title[i].TitleName = StringCipher.Decrypt(title[i].TitleName);
            }
            //_TitleServices.GetTitleList()

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
            var Statelist = _stateservice.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
           // OrganizationUserViewModel model = new OrganizationUserViewModel();
            model.TitleList = title;
            model.Countries = countrylist;
            model.StateList = model.UserId > 0 ? Statelist : new List<LookUpState>();
            model.Cities = model.UserId > 0 ? Citylist : new List<LookUpCity>(); _cityService.GetCityList();
            model.OrgDropDown = _organizationService.GetOrganizationList().Select(x => new DropDownModel { Text = StringCipher.Decrypt(x.OrganizationName), Value = x.OrganizationId }).ToList();
            model.UserRoleList = _roleService.GetUserRolesList().Where(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Organization")).Select(x => new OrganizationRoleDecryptMoel
            {
                RoleName = StringCipher.Decrypt(x.RoleName),
                Id = x.Id,
                UserRoleTypeId = x.UserRoleTypeId
            }).ToList();
            string[] arrPermission = new string[] { "Add Organisations", "Edit Organisations", "View Organisations", 
                                                    "View Organisations Details", "Active/Inactive Organisations"                                                     
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
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone1 = timeZoneList;

            //model.Countries = _countryService.GetCountryList().Select(x => new LookUpCountry { CountryId = x.CountryId, CountryName = StringCipher.Decrypt(x.CountryName) }).ToList();
            //model.Cities = _cityService.GetCityList().Select(x => new LookUpCity { LookUpCityId = x.LookUpCityId, CountryId = x.CountryId, LookUpCityName = StringCipher.Decrypt(x.LookUpCityName) }).ToList();
            return model;
        }
        public ActionResult EditOrganizarionUser(int id)
        {

            var model = _orgUserService.GetUserById(id);
            OrganizationUserViewModel orgModel = new OrganizationUserViewModel();
            orgModel.FirstName = StringCipher.Decrypt(model.FirstName);
            orgModel.MiddleName = StringCipher.Decrypt(model.MiddleName);
            orgModel.SurName = StringCipher.Decrypt(model.SurName);
            orgModel.PhoneNumber = StringCipher.Decrypt(model.PhoneNumber);
            orgModel.MobileNumber = StringCipher.Decrypt(model.MobileNumber);
            orgModel.FaxNumber = StringCipher.Decrypt(model.FaxNumber);
            orgModel.PostCode = StringCipher.Decrypt(model.PostCode);
            orgModel.DOB = model.DOB;
            orgModel.OrgUsreRole = model.OrgUsreRole;
            orgModel.State = model.State;
            orgModel.CountryId = model.CountryId;
            orgModel.State = model.State;
            orgModel.OrgId = model.OrgId;
            orgModel.Suburb = model.Suburb;
            orgModel.Email = StringCipher.Decrypt(model.Email);
            orgModel.Address1 = StringCipher.Decrypt(model.Address1);
            orgModel.Address2 = StringCipher.Decrypt(model.Address2);
            orgModel.LoginId = model.LoginId;

            orgModel.TimeZone = Common.GetTimeZoneStandardIdAndDisplayName(model.TimeZoneString);
            orgModel.UserId = model.MemberId;

            orgModel.ProfilePic = _userService.Find(model.LoginId).Avatar;

            BindDropDown(orgModel);
            return View("Edit", orgModel);
        }
        [HttpPost]
        public ActionResult EditOrganizarionUser(OrganizationUserViewModel model, HttpPostedFileBase ProfilePic)
        {

            var updateModel = _userService.Find(model.LoginId);
            //  updateModel.Member = new OrganizationUserViewModel();
            //updateModel.Member = new OrganizationUserViewModel();
            updateModel.Member.FirstName = StringCipher.Encrypt(model.FirstName);
            updateModel.Member.MiddleName = StringCipher.Encrypt(model.MiddleName);
            updateModel.Member.SurName = StringCipher.Encrypt(model.SurName);
            updateModel.Member.Suburb = model.Suburb;
            updateModel.Member.Title = model.Titleid;
            updateModel.Member.State = model.State;
            updateModel.Member.CountryId = model.CountryId;
            updateModel.Member.City = model.City;
            updateModel.Member.Address1 = StringCipher.Encrypt(model.Address1);
            updateModel.Member.Address2 = StringCipher.Encrypt(model.Address2);
            updateModel.Member.DOB = model.DOB;
            updateModel.Member.PhoneNumber = StringCipher.Encrypt(model.PhoneNumber);
            updateModel.Member.MobileNumber = StringCipher.Encrypt(model.MobileNumber);
            updateModel.Member.FaxNumber = StringCipher.Encrypt(model.FaxNumber);
            updateModel.Member.PostCode = StringCipher.Encrypt(model.PostCode);
            updateModel.Member.OrgId = model.OrgId;

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZone);
            TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
            updateModel.TimeZoneString = Common.GetTimeZoneOffset(offset);

            updateModel.Member.TimeZoneString = Common.GetTimeZoneOffset(offset);
            updateModel.ModifiedDate = DateTime.UtcNow;
            updateModel.Member.ModifiedDate =  DateTime.UtcNow;
            UploadImage(model, ProfilePic);
            if (model.UserId > 0)
            {
                _userService.Update(updateModel);
                _userService.UpdateMember(updateModel);
                if (!string.IsNullOrEmpty(model.ProfilePic))
                {
                    updateModel.Avatar = model.ProfilePic;
                    _userService.Update(updateModel);
                }
            }
            foreach (var item in model.ModuleList)
            {
                if (item.Selected)
                {
                    var permission = new PermissionInModule();
                    foreach (var inneritem in item.SubModuleList)
                    {
                        if (inneritem.Selected)
                        {
                            permission.LoginId = model.LoginId;
                            permission.PermissionId = inneritem.Id;
                            permission.ModuleId = item.Id;
                            permission.UserRoleId = model.OrgUsreRole;
                            permission.IsDeleted = false;
                            permission.IsApproved = true;
                            permission.IsActive = true;
                            _permissioninmoduleService.Add(permission);
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Organization", new { id = model.OrgId });
        }
        public OrganizationViewModel UploadLogo(OrganizationViewModel model, HttpPostedFileBase OrganizationLogo)
        {
            if (OrganizationLogo != null && OrganizationLogo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(OrganizationLogo.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    string fName = DateTime.Now.Ticks + fileName;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/OrganizationLogo/"), fName);
                    var bitmap = new Bitmap(OrganizationLogo.InputStream);
                    imageHelper.Save(bitmap, 150, 150, 250, path);
                    OrganizationLogo.SaveAs(path);
                    model.OrganizationLogo = "/UploadedFiles/OrganizationLogo/" + fName;
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
                if (model.OrganizationLogo != null)
                {
                    model.OrganizationLogo = model.OrganizationLogo;
                }

            }
            return model;
        }
        public OrganizationUserViewModel UploadImage(OrganizationUserViewModel model, HttpPostedFileBase profilePic)
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
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection[0];
            _organizationService.Delete(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _organizationService.Activate(ids);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteUser(FormCollection collection)
        {
            var ids = collection[0];
            _orgUserService.Delete(ids);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult AcitvateUser(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _orgUserService.Activate(ids);
            return RedirectToAction("Index");
        }
        public ActionResult ApproveUser(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _orgUserService.Activate(ids);
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Approve(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _organizationService.Activate(ids);
            return RedirectToAction("Index");
        }
        public JsonResult getOrganizationName(string Organizationname)
        {

            var Available = _organizationService.GetOrganizationList().Where(x => x.OrganizationName == StringCipher.Encrypt(Organizationname)).ToList();
            //for (int i = 0; i < Available.Count; i++)
            //{
            //    Available[i] = StringCipher.Decrypt(Available[i]);
            //}
            //Available= Available.Where(x => x.Contains(Convert.ToString(Name))).ToList();
            if (Available.Count > 0) return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}

