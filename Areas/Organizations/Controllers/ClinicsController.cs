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
    public class ClinicsController : BaseController
    {
        private readonly IOrganizationService _organizationService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IUserService _userService;
        private readonly IOrganizationUserService _orgUserService;
        private readonly IClinicService _clinicService;
        private readonly IClinicUserService _clinicUserService;
        private readonly ILoginHelper _loginService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ILookUpUserRolesService _roleService;
        private readonly IStateService _stateService;
        private readonly ITitleService _titleservice;
        public ClinicsController(IOrganizationService organizationService, ICountryService countryService, ICityService cityService,
            IUserService userService, IOrganizationUserService orgUserService, IClinicService clinicService, IClinicUserService clinicUserService,
            ILoginHelper loginService, ITimeZoneService timeZoneService, ILookUpUserRolesService roleService, IStateService stateService, ITitleService titleservice)
        {
            _organizationService = organizationService;
            _countryService = countryService;
            _cityService = cityService;
            _userService = userService;
            _orgUserService = orgUserService;
            _clinicService = clinicService;
            _clinicUserService = clinicUserService;
            _loginService = loginService;
            _timeZoneService = timeZoneService;
            _roleService = roleService;
            _stateService = stateService;
            _titleservice = titleservice;
        }

        //
        // GET: /Admin/Organization/
        public ActionResult Index()
        {
            int orgId = 0;
            var org = _organizationService.GetOrganizationById(base.loginUserModel.OrganizationId);
            if (org != null)
            {
                orgId = org.OrganizationId;
            }
            var clinics = _clinicService.GetClinicInOrganization(orgId).Select(x =>
                new ClinicModel
                {
                    ClinicName = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.ClinicName)),
                    ClinicType = x.ClinicType,
                    LoginId = x.LoginId,
                    Address1 = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.Address1)),
                    FaxNumber = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.FaxNumber)),
                    Address2 = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.Address2)),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    ClinicId = x.ClinicId,
                    IsActive = x.IsActive,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true)
                }).ToList();
            return View(clinics);
        }
        [HttpPost]
        public ActionResult Index(SearchParametersViewModel searchModel)
        {
            int orgId = 0;
            var org = _organizationService.GetOrganizationByLoginId(base.loginUserModel.LoginId);
            if (org != null)
            {
                orgId = org.OrganizationId;
            }
            var clinics = _clinicService.GetClinicInOrganization(orgId).ToList();
            for (int i = 0; i < clinics.Count; i++)
            {
                if (_clinicUserService.GetUsersByClinic(clinics[i].ClinicId).FirstOrDefault() != null)
                {//forclinic admin name use address1
                    clinics[i].Address1 = StringCipher.Decrypt(_clinicUserService.GetUsersByClinic(clinics[i].ClinicId).FirstOrDefault().FirstName) + " " + StringCipher.Decrypt(_clinicUserService.GetUsersByClinic(clinics[i].ClinicId).FirstOrDefault().SurName);
                }
            }
            clinics = FilterRecord(searchModel, clinics);
            var clinic = clinics.Select(x =>
            {
                var clinicUsers = _clinicUserService.GetUsersByClinic(x.ClinicId).FirstOrDefault();
                return clinicUsers != null ? new ClinicModel
                {
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    ClinicLogo = x.ClinicLogo,
                    ClinicId = x.ClinicId,
                    ClinicType = x.ClinicType,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    // OrganizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(x.OrganizationId).OrganizationName),
                    CliniAdminName = StringCipher.Decrypt(clinicUsers.FirstName) + "" + StringCipher.Decrypt(clinicUsers.SurName),
                    ModifiedDate = x.ModifiedDate ?? DateTime.Now,
                    IsActive = x.IsActive
                } : null;
            }).ToList();
            //var clinics = _clinicService.GetClinicInOrganization(orgId).Select(x =>
            //    new ClinicModel
            //    {
            //        ClinicName = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.ClinicName)),
            //        ClinicType = x.ClinicType,
            //        LoginId = x.LoginId,
            //        Address1 = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.Address1)),
            //        FaxNumber = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.FaxNumber)),
            //        Address2 = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(StringCipher.Decrypt(x.Address2)),
            //        PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
            //        ClinicId = x.ClinicId,
            //        IsActive = x.IsActive,

            //    }).ToList();

            return View(clinics);
        }
        public ActionResult Add(int? id)
        {
            ClinicModel cModel = new ClinicModel();
            ClinicUserViewModel userModel = new ClinicUserViewModel();
            cModel.ClinicUser = userModel;
            if (id.HasValue)
            {
                var model = _clinicService.GetClinicById(Convert.ToInt32(id));
                model.Address1 = StringCipher.Decrypt(model.Address1);
                model.Address2 = StringCipher.Decrypt(model.Address2);
                model.ClinicName = StringCipher.Decrypt(model.ClinicName);
                model.PostCode = StringCipher.Decrypt(model.PostCode);
                model.PhoneNumber = StringCipher.Decrypt(model.PhoneNumber);
                model.FaxNumber = StringCipher.Decrypt(model.FaxNumber);
                model.PKIMedicareCertificateNo = StringCipher.Decrypt(model.PKIMedicareCertificateNo);
                cModel = new ClinicModel(model)
                {
                    ClinicUser =
                        _clinicUserService.GetUsersByClinic(Convert.ToInt32(id)).Select(x => new ClinicUserViewModel()
                        {
                            Titleid = x.Title,
                            FirstName = StringCipher.Decrypt(x.FirstName),
                            MiddleName = StringCipher.Decrypt(x.MiddleName),
                            SurName = StringCipher.Decrypt(x.SurName),
                            State = x.State,
                            State1 = x.State,
                            City1 = x.City,
                            CountryId = x.CountryId,
                            City = x.City,
                            Address1 = StringCipher.Decrypt(x.Address1),
                            Address2 = StringCipher.Decrypt(x.Address2),
                            PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                            Email = StringCipher.Decrypt(x.Email),
                            DOB = x.DOB,
                            UserId = x.MemberId,
                            OrgId = x.OrgId,
                            ProfilePic = _userService.Find(x.LoginId).Avatar,
                            LoginId = x.LoginId,
                            FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                            PostCode = StringCipher.Decrypt(x.PostCode),
                            TimeZoneId = x.TimeZoneString,
                        }).FirstOrDefault()
                };
                BindDropDownLists(cModel);
                cModel.TimeZone = Common.GetTimeZoneStandardIdAndDisplayName(model.TimeZoneString);
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
        public ActionResult Add(ClinicModel model, HttpPostedFileBase ProfilePic, HttpPostedFileBase OrganizationLogo)
        {
            try
            {
                var userName = model.ClinicUser.Email;

                int orgId = 0;
                var organization = _organizationService.GetOrganizationList().FirstOrDefault(x => x.OrganizationId == base.loginUserModel.OrganizationId);
                if (organization != null)
                {
                    orgId = organization.OrganizationId;
                }
                MyOnlineClinic.Entity.Clinic cModel = new MyOnlineClinic.Entity.Clinic();
                model.OrganizationId = orgId;
                cModel = model.GetEditModel(model);
                if (cModel.ClinicId > 0)
                {
                    cModel.ModifiedDate = DateTime.UtcNow;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZone);
                    TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
                    cModel.TimeZoneString = Common.GetTimeZoneOffset(offset);
                    _clinicService.Update(cModel);
                    var oModel = model.ClinicUser.GetEditModel(model.ClinicUser);
                    oModel.DOB = DateTime.Now;
                    oModel.OrgId = model.OrganizationId;
                    oModel.ClinicUsreRole = (int)5;
                    oModel.ClinicId = model.ClinicId;
                    oModel.MemberId = model.ClinicUser.UserId;
                    var userRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == "Clinic" && x.RoleName == "Admin");
                    if (userRole != null)
                    {
                        oModel.ClinicUsreRole = userRole.Id;
                    }
                    else { oModel.ClinicUsreRole = (int)RoleType.ClinicAdmin; }

                    if (ProfilePic != null && ProfilePic.ContentLength > 0)
                    {
                        //Uploading image if new image uploaded.
                        UploadImage(model.ClinicUser, ProfilePic);
                    }

                    _clinicUserService.Update(oModel);
                    if (OrganizationLogo != null && OrganizationLogo.ContentLength > 0)
                    {
                        //Uploading image if new image uploaded.
                        UploadLogo(model, OrganizationLogo);
                    }

                    //updating organization
                    _clinicService.Update(cModel);
                }
                else
                {

                    if (model.ClinicUser != null)
                    {
                        if (_userService.CheckDuplicateMail(StringCipher.Encrypt(userName)))
                        {
                            BindDropDownLists(model);
                            ViewBag.ErrorMessage = "This email address already exists.";
                            return View(model);
                        }
                        if (_userService.CheckDuplicateUserName(StringCipher.Encrypt(userName)))
                        {
                            BindDropDownLists(model);
                            ViewBag.ErrorMessage = "This user name already exists.";
                            return View(model);
                        }

                        var userRole = _roleService.GetUserRolesList().FirstOrDefault(x => x.RoleType.RoleTypeName == StringCipher.Encrypt("Clinic") && x.RoleName == StringCipher.Encrypt("Admin"));
                        if (userRole != null)
                        {
                            model.ClinicUser.OrgUsreRole = userRole.Id;
                        }

                        else { model.ClinicUser.OrgUsreRole = (int)RoleType.ClinicAdmin; }
                        cModel.CreatedBy = base.loginUserModel.LoginId;
                        UploadLogo(model, OrganizationLogo);
                        cModel.ClinicLogo = model.ClinicLogo;
                        _clinicService.Add(cModel);

                        UploadImage(model.ClinicUser, ProfilePic);

                        BaseMember profile = null;
                        profile = GetClinicAdmin(model.ClinicUser, cModel.ClinicId, orgId);
                        var user = GetLogin(profile, model.ClinicUser.Email, model);
                        var randomPassword = RandomPassword.Generate(6, 8);
                        user.LoginPassword = randomPassword;
                        _userService.Add(user);
                        var clinic = _clinicService.GetClinicById(cModel.ClinicId);
                        if (clinic != null)
                        {
                            clinic.LoginId = user.LoginId;
                            _clinicService.Update(clinic);
                        }

                        string activationToken = user.ActivationToken.ToString();
                        string link = ConfigurationManager.AppSettings["SitePath"] + "Account/Activate/" + activationToken;
                        var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
                        string html = System.IO.File.ReadAllText(fileUrl);
                        html = html.Replace("@@ActivationLink", link);

                        EmailService objEmail = new EmailService();
                        objEmail.SendEmail("User Registration", html, userName, Common.fromEmailAddress);

                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                BindDropDownLists(model);
                return RedirectToAction("Index");
            }
        }

        public ClinicModel BindDropDownLists(ClinicModel model)
        {
            var title = _titleservice.GetTitleList();
            for (int i = 0; i < title.Count; i++)
            {
                title[i].TitleName = StringCipher.Decrypt(title[i].TitleName);
            }
            //_TitleServices.GetTitleList()
            model.TitleList = title.Where(x => x.Type == (int)RoleType.ADMIN).ToList();
            model.Countries = _countryService.GetCountryList().ToList();


            for (int i = 0; i < model.Countries.Count; i++)
            {
                model.Countries[i].CountryName = StringCipher.Decrypt(model.Countries[i].CountryName);
            }


            model.States = _stateService.GetStateList();
            for (int i = 0; i < model.States.Count; i++)
            {
                model.States[i].StateName = StringCipher.Decrypt(model.States[i].StateName);
            }

            model.Cities = _cityService.GetCityList();

            for (int i = 0; i < model.Cities.Count; i++)
            {
                model.Cities[i].LookUpCityName = StringCipher.Decrypt(model.Cities[i].LookUpCityName);
            }

            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;

            model.OrganizationList = _organizationService.GetOrganizationList();
            //model.TimeZoneList = _timeZoneService.GetTimeZone();
            return model;
        }

        public Login GetLogin(BaseMember member, string userName, ClinicModel model)
        {
            Login login = new Login
            {
                Email = userName,
                LoginName = userName,
                LoginPassword = "cliUser",
                LookUpRoleId = (int)RoleType.ClinicUser,
                Avatar = model.ClinicLogo,
                IsActive = true,
            };
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, string.Empty, false);
            login.SetCreated(login.LoginId, string.Empty, false);
            return login;

        }

        public BaseMember GetClinicAdmin(ClinicUserViewModel model, int clinicId, int orgId)
        {
            BaseMember profile = new ClinicUsers
            {
                MemberId = model.UserId,
                //Title = model.,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                SurName = model.SurName,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Suburb = model.Suburb,
                PostCode = model.PostCode,
                CountryId = model.CountryId,
                State = model.State,
                City = model.City,
                DOB = DateTime.Now,
                Gender = model.Genderid,
                MobileNumber = model.MobileNumber,
                PhoneNumber = model.PhoneNumber,
                FaxNumber = model.FaxNumber,
                OrgId = orgId,
                ClinicId = clinicId,
                ClinicUsreRole = model.OrgUsreRole

            };
            profile = SetMember(profile, model, orgId);
            return profile;
        }

        public BaseMember SetMember(BaseMember member, ClinicUserViewModel model, int orgId)
        {
            member.FirstName = model.FirstName;
            member.Email = model.Email;
            member.SurName = model.SurName;
            member.CountryId = model.CountryId;
            member.City = model.City;
            member.Address1 = model.Address1;
            member.Address2 = model.Address2;
            member.Suburb = model.Suburb;
            member.PostCode = model.PostCode;
            member.CountryId = model.CountryId;
            member.State = model.State;
            member.DOB = DateTime.Now;
            member.Gender = model.Genderid;
            member.MobileNumber = model.MobileNumber;
            member.PhoneNumber = model.PhoneNumber;
            member.FaxNumber = model.FaxNumber;
            member.OrgId = orgId;

            return member;
        }


        public OrganizationUserViewModel BindDropDown(OrganizationUserViewModel model)
        {
            model.Countries = _countryService.GetCountryList();
            model.Cities = _cityService.GetCityList();
            return model;
        }

        public ActionResult EditOrganizarionUser(int id)
        {

            var model = _orgUserService.GetUserById(id);
            var orgModel = new OrganizationUserViewModel(model);
            BindDropDown(orgModel);
            return View("Edit", orgModel);
        }
        [HttpPost]
        public ActionResult EditOrganizarionUser(OrganizationUserViewModel model, HttpPostedFileBase ProfilePic)
        {
            var updateModel = _userService.Find(model.LoginId);
            updateModel.Member.FirstName = model.FirstName;
            updateModel.Member.MiddleName = model.MiddleName;
            updateModel.Member.SurName = model.SurName;
            updateModel.Member.Suburb = model.Suburb;
            updateModel.Member.Title = model.Titleid;
            updateModel.Member.State = model.State;
            updateModel.Member.CountryId = model.CountryId;
            updateModel.Member.City = model.City;
            updateModel.Member.Address1 = model.Address1;
            updateModel.Member.Address2 = model.Address2;
            updateModel.Member.DOB = model.DOB;
            updateModel.Member.PhoneNumber = model.PhoneNumber;
            updateModel.Member.MobileNumber = model.MobileNumber;
            updateModel.Member.FaxNumber = model.FaxNumber;
            updateModel.Member.PostCode = model.PostCode;
            updateModel.Member.OrgId = model.OrgId;
            //UploadImage(model, ProfilePic);
            if (model.UserId > 0)
            {
                _userService.UpdateMember(updateModel);
                if (!string.IsNullOrEmpty(model.ProfilePic))
                {
                    updateModel.Avatar = model.ProfilePic;
                    _userService.Update(updateModel);
                }
            }
            return RedirectToAction("Add", "Organization", new { id = model.OrgId });
        }
        public ClinicModel UploadLogo(ClinicModel model, HttpPostedFileBase OrganizationLogo)
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
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection[0];
            _clinicService.Delete(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _clinicService.Activate(ids);
            return RedirectToAction("Index");
        }
        private List<Entity.Clinic> FilterRecord(SearchParametersViewModel searchModel, List<Entity.Clinic> query)
        {
            for (int i = 0; i < query.Count; i++)
            {
                query[i].ClinicName = StringCipher.Decrypt(query[i].ClinicName);
                query[i].Address2 = StringCipher.Decrypt(_organizationService.GetOrganizationById(query[i].OrganizationId).OrganizationName);
                query[i].PostCode = StringCipher.Decrypt(query[i].PostCode);
                if (query[i].LoginId != new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    query[i].FaxNumber = Convert.ToString(_userService.Find(query[i].LoginId).LookUpRoleId);
                }
                //query[i].Address1 = _clinicUserService.GetUserById(query[i].ClinicId).SurName != null ? StringCipher.Decrypt(_clinicUserService.GetUsersByClinic(query[i].ClinicId).FirstOrDefault().FirstName) + " " + StringCipher.Decrypt(_clinicUserService.GetUsersByClinic(query[i].ClinicId).FirstOrDefault().SurName) : StringCipher.Decrypt(_clinicUserService.GetUsersByClinic(query[i].ClinicId).FirstOrDefault().FirstName);
            }
            //here address2 is using for getting organization name          
            //query = query.FilterByOrganizationName(searchModel.OrgName,searchModel.ClinicName,searchModel.PostCode,searchModel.CountryId,searchModel.StateId,searchModel.ClinicAdmin,searchModel.ActivationStatus);  
            var ObjFilter = new FilterParameters
            {
                OrganizationName = StringCipher.Decrypt(searchModel.OrgName),
                ClinicName = StringCipher.Decrypt(searchModel.ClinicName),
                PostCode = StringCipher.Decrypt(searchModel.PostCode),
                CountryId = searchModel.CountryId,
                StateId = searchModel.StateId,
                ClinicAdmin = searchModel.ClinicAdmin,
                OrgAdmin = searchModel.OrgAdmin,
                ActivationStatus = searchModel.ActivationStatus,
                ClinicAdminName = searchModel.ClinicAdminName
            };
            query = query.FilterByOrganizationName(ObjFilter);

            return query;
        }
        public ActionResult getClinicName(string ClinicName)
        {
            var Available = _clinicService.GetClinicList().Where(x => x.ClinicName == StringCipher.Encrypt(ClinicName)).ToList();
            //for (int i = 0; i < Available.Count; i++)
            //{
            //    Available[i] = StringCipher.Decrypt(Available[i]);
            //}
            //Available = Available.Where(x => x.Contains(Convert.ToString(Name))).ToList();
            if (Available.Count > 0) return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}