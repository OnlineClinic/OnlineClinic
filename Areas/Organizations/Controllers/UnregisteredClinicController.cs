using MyOnlineClinic.Emailer;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class UnregisteredClinicController : BaseController
    {
        private readonly IUnregisteredClinicService _uregisteredClinicService;
        private readonly IUregisteredOrganizationService _uregisteredOrganizationService;
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
        private readonly ITimeZoneService _timeZoneService;
        private readonly IStateService _stateservice;
        IDoctorService _doctorService;
        IPatientService _patientservice;
        IUserService _userservice;
        private readonly IClinicUserService _clinicuserservice;
        public UnregisteredClinicController(IUnregisteredClinicService uregisteredClinicService, IUregisteredOrganizationService uregisteredOrganizationService, IOrganizationService organizationService, ICountryService countryService, ICityService cityService,
            IUserService userService, IOrganizationUserService orgUserService, ILoginHelper loginService, IClinicService clinicService,
            IOrganizationTypeService organizationTypeService, ITimeZoneService timeZoneService, ILookUpUserRolesService roleService,
             ILookUpModuleService moduleService, IPermissionInModuleService permissioninmoduleService, IPermissionsService permissionService, IStateService stateservice, ITitleService TitleServices, IDoctorService doctorService, IPatientService patientservice, IUserService userservice, IClinicUserService clinicuserservice)
        {
            _uregisteredClinicService = uregisteredClinicService;
            _uregisteredOrganizationService = uregisteredOrganizationService;
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
            _doctorService = doctorService;
            _patientservice = patientservice;
            _userservice = userservice;
            _clinicuserservice = clinicuserservice;
        }
        public ActionResult Index()
        {
            var organization = _organizationService.GetOrganizationByLoginId(base.loginUserModel.LoginId);

            var uregisteredClinic = _uregisteredClinicService.ClinicList().Where(x => x.IsDeleted == false).Select(x => new UnregisteredClinicViewModel
            {
                Name = StringCipher.Decrypt(x.Name),
                Id = x.Id,
                AddressLine1 = StringCipher.Decrypt(x.AddressLine1),
                AddressLine2 = StringCipher.Decrypt(x.AddressLine2),
                PostCode = StringCipher.Decrypt(x.PostCode),
                addedby = x.Type == (int)RoleType.Doctor ? "Doctor" : "Patient",
                OrganizationId = x.OrgId,
                ModifiedDate = (DateTime)x.ModifiedDate
            }).Where(t => organization != null && t.OrganizationId == organization.OrganizationId).ToList(); 
            if (TempData["shortMessage"] != null)
            {
                ViewBag.Message = TempData["shortMessage"].ToString();
            }
            return View(uregisteredClinic);
        }
        public ActionResult Edit(int? Id)
        {
            if (Id.HasValue)
            {
                int id = (int)Id;
                UnregisteredClinicViewModel cModel = new UnregisteredClinicViewModel();

                if (Id.HasValue)
                {
                    var model = entitiesDb.UnRegisteredClinics.Where(x => x.Id == id).FirstOrDefault();
                    model.Name = StringCipher.Decrypt(model.Name);
                    model.AddressLine1 = StringCipher.Decrypt(model.AddressLine1);
                    model.AddressLine2 = StringCipher.Decrypt(model.AddressLine2);
                    cModel = new UnregisteredClinicViewModel(model);
                    ClinicUserViewModel userModel = new ClinicUserViewModel();
                    cModel.ClinicUser = userModel;
                    //{
                    //    ClinicUser = new OrganizationUserViewModel(),

                    //};
                    BindDropDown(cModel);
                    return View(cModel);
                }
                else
                {
                    BindDropDown(cModel);
                    return View(cModel);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(UnregisteredClinicViewModel model, HttpPostedFileBase ProfilePic, HttpPostedFileBase OrganizationLogo)
        {
            var userName = model.ClinicUser.Email;
            UnRegisteredClinic cModel = new UnRegisteredClinic();
            cModel = model.GetEditModel(model);
            if (cModel.Id > 0)
            {
                if (_userService.CheckDuplicateMail(userName))
                {
                    BindDropDown(model);
                    ViewBag.ErrorMessage = "This email address already exists.";
                    return View(model);
                }
                if (_userService.CheckDuplicateUserName(userName))
                {
                    BindDropDown(model);
                    ViewBag.ErrorMessage = "This user name already exists.";
                    return View(model);
                }
                UploadLogo(model, OrganizationLogo);
                cModel.ClinicLogo = model.ClinicLogo;
                ClinicUsers oModel = new ClinicUsers();
                _uregisteredClinicService.Update(cModel);
                if (model.ClinicUser != null)
                {
                    var userRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == "Clinic" && x.RoleName == "Admin");
                    if (userRole != null)
                    {
                        model.ClinicUser.OrgUsreRole = userRole.Id;
                    }
                    else { model.ClinicUser.OrgUsreRole = (int)RoleType.ClinicAdmin; }

                    UploadImage(model.ClinicUser, ProfilePic);
                    BaseMember profile = null;
                    profile = GetClinicAdmin(model, cModel.Id, model.OrganizationId);

                    var user = GetLogin(profile, model.ClinicUser.Email, model);
                    var randomPassword = RandomPassword.Generate(6, 8);
                    user.LoginPassword = randomPassword;
                    _userService.Add(user);
                    string activationToken = user.ActivationToken.ToString();
                    string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
                    var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
                    string html = System.IO.File.ReadAllText(fileUrl);
                    html = html.Replace("@@ActivationLink", link).Replace("@@Password", randomPassword);
                    var email = new EmailService();
                    email.SendEmail("User Registration", html, model.ClinicUser.Email, "tma@myonlineclinic.com.au");
                }
            }
            return RedirectToAction("Index");
        }
        public UnregisteredClinicViewModel BindDropDown(UnregisteredClinicViewModel model)
        {
            var title = _TitleServices.GetTitleList();
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
            //model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            //model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();          
            var Statelist = _stateservice.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.Cities = model.Id > 0 ? Citylist : new List<LookUpCity>();
            model.States = model.Id > 0 ? Statelist : new List<LookUpState>();
            model.OrganizationList = _organizationService.GetOrganizationList();
            for (int i = 0; i < model.OrganizationList.Count; i++)
            {
                model.OrganizationList[i].OrganizationName = StringCipher.Decrypt(model.OrganizationList[i].OrganizationName);
            }
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
            ViewBag.TimeZones = timeZoneList;
            //model.TimeZoneList = _timeZoneService.GetTimeZone();
            return model;
        }
        public ActionResult Lock(int? Id)
        {
            int Orgid = 0;
            var model = _uregisteredClinicService.GetClinicById(Convert.ToInt32(Id));
            if (model.OrganizationType == "U" && model.OrgId > 0)
            {
                if (entitiesDb.UnregisteredOrganizations.Where(x => x.Id == model.OrgId).FirstOrDefault().IsDeleted == true)
                {
                    if (_organizationService.GetOrganizationByUnregId(Convert.ToInt32(Id)) == true)
                    //if (entitiesDb.Organizations.Where(x => x.UnRegOrgId == Id).Select(x => x.UnRegOrgId).FirstOrDefault() > 0)
                    {
                        var getorg = _organizationService.GetOrganizationList().Where(x => x.UnRegOrgId == Convert.ToInt32(Id)).FirstOrDefault().OrganizationId;
                        Orgid = getorg;
                    }
                    else
                    {
                        ViewBag.errormsg = "Plz Register Organizatin first before Lock this";
                        TempData["shortMessage"] = "Plz Register Organizatin first before Lock this";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    ViewBag.errormsg = "Plz Register Organizatin first before Lock this";
                    TempData["shortMessage"] = "Plz Register Organizatin first before Lock this";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Orgid = model.OrgId;
            }
            if (model != null)
            {
                Entity.Clinic c = new Entity.Clinic();
                GeteditClinic(model, c, Orgid);
                _clinicService.Add(c);
                int Userid = model.UserId;
                UserClinic user = new UserClinic();
                user.UserId = model.UserId;
                user.OrganizationId = Orgid;
                user.Type = model.Type;
                user.ClinicId = c.ClinicId;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = base.loginUserModel.LoginId;
                user.TimeZoneString = model.TimeZoneString;
                _uregisteredOrganizationService.AddClinic(user);
                model.IsDeleted = true;
                _uregisteredClinicService.Update(model);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        private void GeteditClinic(UnRegisteredClinic model, Entity.Clinic c, int orgid)
        {
            c.ClinicName = model.Name;
            c.Address1 = model.AddressLine1;
            c.Address2 = model.AddressLine2;
            c.PostCode = model.PostCode;
            c.FaxNumber = model.FaxNumber;
            c.ClinicLogo = model.ClinicLogo;
            c.TimeZoneString = model.TimeZoneString;
            c.Country = model.Country;
            c.State = model.State;
            c.City = model.City;
            c.LoginId = base.loginUserModel.LoginId;
            c.ClinicType = model.ClinicType;
            c.IpAddress = model.IpAddress;
            c.IsActive = model.IsActive;
            c.IsApproved = model.IsApproved;
            c.IsActive = true;
            c.IsDeleted = false;
            c.IpAddress = model.IpAddress;
            c.OrganizationId = orgid;
        }
        public UnregisteredClinicViewModel UploadLogo(UnregisteredClinicViewModel model, HttpPostedFileBase OrganizationLogo)
        {
            if (OrganizationLogo != null && OrganizationLogo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(OrganizationLogo.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    if (!string.IsNullOrEmpty(model.ClinicLogo))
                    {
                        //Delete exising File
                        var file = Server.MapPath(model.ClinicLogo);
                        imageHelper.Delete(file);
                    }
                    string fName = DateTime.Now.Ticks + fileName;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/OrganizationLogo/"), fName);
                    var bitmap = new Bitmap(OrganizationLogo.InputStream);
                    imageHelper.Save(bitmap, 150, 150, 250, path);
                    OrganizationLogo.SaveAs(path);
                    model.ClinicLogo = "/UploadedFiles/OrganizationLogo/" + fName;
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
                if (model.ClinicLogo != null)
                {
                    model.ClinicLogo = model.ClinicLogo;
                }

            }
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
        public BaseMember GetClinicAdmin(UnregisteredClinicViewModel model, int clinicId, int orgId)
        {
            BaseMember profile = new ClinicUsers
            {
                MemberId = model.ClinicUser.UserId,
                Title = model.ClinicUser.Titleid,
                FirstName = StringCipher.Encrypt(model.ClinicUser.FirstName),
                MiddleName = StringCipher.Encrypt(model.ClinicUser.MiddleName),
                SurName = StringCipher.Encrypt(model.ClinicUser.SurName),
                Address1 = StringCipher.Encrypt(model.ClinicUser.Address1),
                Address2 = StringCipher.Encrypt(model.ClinicUser.Address2),
                Suburb = model.ClinicUser.Suburb,
                PostCode = StringCipher.Encrypt(model.ClinicUser.PostCode),
                CountryId = model.ClinicUser.CountryId,
                State = model.ClinicUser.State,
                City = model.ClinicUser.City,
                DOB = DateTime.Now,
                Gender = model.ClinicUser.Genderid,
                MobileNumber = StringCipher.Encrypt(model.ClinicUser.MobileNumber),
                PhoneNumber = StringCipher.Encrypt(model.ClinicUser.PhoneNumber),
                FaxNumber = StringCipher.Encrypt(model.ClinicUser.FaxNumber),
                OrgId = orgId,
                ClinicId = clinicId,
                ClinicUsreRole = model.ClinicUser.OrgUsreRole
            };
            profile = SetMember(profile, model, clinicId, model.OrganizationId);
            return profile;
        }
        public BaseMember SetMember(BaseMember member, UnregisteredClinicViewModel model, int clinicId, int orgId)
        {
            member.Title = model.ClinicUser.Titleid;
            member.FirstName = StringCipher.Encrypt(model.ClinicUser.FirstName);
            member.MiddleName = StringCipher.Encrypt(model.ClinicUser.MiddleName);
            member.Email = StringCipher.Encrypt(model.ClinicUser.Email);
            member.SurName = StringCipher.Encrypt(model.ClinicUser.SurName);
            member.CountryId = model.ClinicUser.CountryId;
            member.City = model.ClinicUser.City;
            member.Address1 = StringCipher.Encrypt(model.ClinicUser.Address1);
            member.Address2 = StringCipher.Encrypt(model.ClinicUser.Address2);
            member.Suburb = model.ClinicUser.Suburb;
            member.PostCode = StringCipher.Encrypt(model.ClinicUser.PostCode);
            member.CountryId = model.ClinicUser.CountryId;
            member.State = model.ClinicUser.State;
            member.DOB = DateTime.Now;
            member.Gender = model.ClinicUser.Genderid;
            member.MobileNumber = StringCipher.Encrypt(model.ClinicUser.MobileNumber);
            member.PhoneNumber = StringCipher.Encrypt(model.ClinicUser.PhoneNumber);
            member.FaxNumber = StringCipher.Encrypt(model.ClinicUser.FaxNumber);
            member.OrgId = orgId;
            return member;
        }
        public Login GetLogin(BaseMember member, string userName, UnregisteredClinicViewModel model)
        {
            Login login = new Login
            {
                Email = StringCipher.Encrypt(userName),
                LoginName = StringCipher.Encrypt(userName),
                LoginPassword = RandomPassword.Generate(8, 10),
                LookUpRoleId = (int)RoleType.ClinicAdmin,
                Avatar = model.ClinicUser.ProfilePic,
                IsActive = true,
            };
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, string.Empty, false);
            login.SetCreated(login.LoginId, string.Empty, false);
            return login;

        }
	}
}