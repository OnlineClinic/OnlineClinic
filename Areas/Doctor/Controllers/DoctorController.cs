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
namespace MyOnlineClinic.Web.Areas.Doctor.Controllers
{
    public class DoctorController : BaseController
    {
        protected IFormsAuthentication _formsAuth;
        IDoctorService _doctorservice;
        IClinicService _clinicservice;
        ICountryService _countryService;
        ICityService _cityService;
        IOrganizationService _organizationservice;
        IUserService _userService;
        ITimeZoneService _timezoneservice;
        ILoginHelper _loginHelper;
        IProfessionTypeService _professiontypeservice;
        IStateService _stateservice;
        private readonly ITitleService _TitleServices;
        private readonly IGenderService _genderService;
        public DoctorController(ICountryService countryService, ICityService cityService, IFormsAuthentication formsAuth, IUserService userService, IDoctorService doctorservice, IOrganizationService organizationservice, ITimeZoneService timezoneservice, ILoginHelper loginHelper, IProfessionTypeService professiontypeservice, IClinicService clinicservice, ITitleService TitleServices, IGenderService genderService,  IStateService stateservice )
        {
            _countryService = countryService;
            _cityService = cityService;
            _formsAuth = formsAuth;
            _userService = userService;
            _doctorservice = doctorservice;
            _organizationservice = organizationservice;
            _timezoneservice = timezoneservice;
            _loginHelper = loginHelper;
            _professiontypeservice = professiontypeservice;
            _clinicservice = clinicservice;
            _TitleServices = TitleServices;
            _genderService = genderService;
            _stateservice = stateservice;
        }
        //
        // GET: /Doctor/Doctor/
        public ActionResult Add(int? id)
        {
            // RegisterViewModel doctor = new RegisterViewModel();
            if (id.HasValue)
            {
                var cModel = _doctorservice.GetDoctorById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
                {

                 
                    TitleId=x.Title,
                    UserorgId=x.OrgId,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    SurName = x.SurName,
                    Email = x.Email,
                    //DOB = x.DOB,
                    GenderId = x.Gender,
                    MobileNumber = x.MobileNumber,
                    FaxNumber = x.FaxNumber,
                    ClinicID = Convert.ToInt32(x.ClinicID),
                    City = x.City,
                    State = x.State,
                    CountryId = x.CountryId,
                    Profession = x.Profession,
                    PostCode = x.PostCode,
                    Timezone = x.TimeZoneString,
                    //ClinicID = _clinicService.GetClinicById(id).cli,
                    PhoneNumber = x.PhoneNumber,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    OrgId = x.OrgId,
                    ProfilePic = _userService.Find(x.LoginId).Avatar,
                    IsActive = x.IsActive,
                    MemberId = x.MemberId,
                    LoginId = x.LoginId,
                    Createdby = (Guid)x.CreatedBy,
                    PhoneCode = x.PhoneCode,
                    LanguageSpoken = x.LanguageSpoken,
                    ProfessionCategory = x.ProfessionCategory,
                    IntrestArea = x.IntrestArea,
                    qualification = x.qualification,
                    AHPRANo = x.AHPRANo,
                    VideoProviderNo = x.VideoProviderNo,
                    HomeVisitProviderNo=x.HomeVisitProviderNo,
                    ClinicProviderNo=x.ClinicProviderNo,
                    PrescriberNo = x.PrescriberNo,
                    DoctorProfile = x.DoctorProfile,
                    SignaturePic = x.SignaturePic
                   
                  //  OrganizationAddress=_organizationservice.GetOrganizationAddressbyId(Convert.ToInt32(x.OrgId)),
                }).FirstOrDefault();


                if(cModel != null)
                {
                    if (_organizationservice.GetOrganizationById(cModel.OrgId) != null)
                    {
                        string a = "";
                    }
                }
                //ViewBag.City = cModel.City;
                //ViewBag.State = cModel.State;
                //ViewBag.orgid = cModel.OrgId;
                //ViewBag.clinic = cModel.ClinicID;
                BindDropDown(cModel);

                return View(cModel);
            }
            else
            {
                RegisterViewModel doctor = new RegisterViewModel();
                BindDropDown(doctor);
                return View(doctor);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Add(RegisterViewModel model, HttpPostedFileBase ProfilePic,HttpPostedFileBase Signaturepic)
        {

            if (model.MemberId > 0)
            {
                MyOnlineClinic.Entity.Doctor profile = null;
                // MyOnlineClinic.Entity.Login login = null;
                profile = model.AdminEditDoctor(model);
                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                }
                _doctorservice.Update(profile);
                if (model.MemberId > 0)
                {

                    if (!string.IsNullOrEmpty(model.ProfilePic))
                    {
                        var userid = _userService.Find(model.LoginId);
                        userid.Avatar = model.ProfilePic;
                        _userService.Update(userid);
                    }
                }
                //if (ProfilePic != null && ProfilePic.ContentLength>0)
                //{
                //    _loginHelper.UploadImage(model, ProfilePic);
                //    login = model.GetEditLogin(model);
                //    _userService.Update(login);

                //}
                return RedirectToAction("DoctorList");
            }
            else
            {
                var userExists = _userService.CheckDuplicateUserName(model.UserName);
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                   // ViewBag.ErrorMessage = "This User Name is address already exists.";
                   // BindDropDown(model);
                  // return View(model);
                    
                }
                var userEmailExists = _userService.CheckDuplicateMail(model.Email);
                if (userEmailExists)
                {
                    ModelState.AddModelError("email", "email already exists.");
                   
                   // ViewBag.ErrorMessage = "Email is already Exists";
                   // ViewBag.ErrorMessage = "This email address already exists.";
                    BindDropDown(model);
                    return View(model);
                }
                else
                {                  
                    if (ProfilePic != null)
                    {
                        _loginHelper.UploadImage(model, ProfilePic);
                    }
                    if (Signaturepic != null && Signaturepic.ContentLength > 0)
                    {
                        UploadSignature(model, Signaturepic);
                    }
                    BaseMember profile = null;

                    model.RoleId = RoleType.Doctor;
                    
                    profile = model.GetDoctorAdmin();
                    var user = model.GetLogin(profile, model.Email);
                    _userService.Add(user);
                    string activationToken = user.ActivationToken.ToString();
                    string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyDocEmail/" + activationToken;
                    var fileUrl = Server.MapPath("~/EmailTemplates/PatientRegister.html");
                    string html = System.IO.File.ReadAllText(fileUrl);
                    html = html.Replace("@@ActivationLink", link);
                    EmailService objEmail = new EmailService();
                    objEmail.SendEmail("Doctor Registration", html, model.Email, "tma@myonlineclinic.com.au");
                    return RedirectToAction("DoctorList");               
                }
            }
            //return View();
        }
        public IEnumerable<SelectListItem> PhnCode
        {
            get
            {
                return new[]
            {
                new SelectListItem { Value = "0", Text = "--Select Country Code--" },
                new SelectListItem { Value = "+61 Australiya", Text = "+61 Australiya" },
                new SelectListItem { Value = "+679 Fiji", Text = "+679 Fiji" },             
            };
            }
        }
        public RegisterViewModel BindDropDown(RegisterViewModel model)
        {
            model.GenderList = _genderService.GetGenderList().Where(x => x.Type == (int)RoleType.Doctor).ToList();
            model.TitleList = _TitleServices.GetTitleList().Where(x=> x.Type==(int)RoleType.Doctor).ToList();
            model.CountryList = _countryService.GetCountryList();
            model.CityList = _cityService.GetCityList();
            model.StateList = _stateservice.GetStateList();
            ViewBag.Organization = _organizationservice.GetOrganizationList().ToList();
            model.ClinicList = _clinicservice.GetClinicList();
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            //var clinic = entitiesDb.Clinics.Select(p => new SelectListItem { Value = p.ClinicName, Text = p.ClinicName }).ToList();
            ViewBag.Clinic = new List<SelectListItem>();
            model.OrganizationList = _organizationservice.GetOrganizationList();
            model.ProfessionTypes = _professiontypeservice.GetProfessionTypesList();
            ViewBag.Phonecode = PhnCode;
           // model.OrganizationTypeList = _organizationTypeService.GetOrganizarionTypesList();
            return model;
        }
        //public ActionResult Index()
        //{
        //    var doctors = _doctorservice.GetDoctorList().Select(x => new RegisterViewModel { FullName = x.FirstName + " " + x.SurName, MemberId = x.MemberId }).ToList();
        //    return View(doctors);
        //}

        public ActionResult DoctorList()
        {
            var doctor = _doctorservice.GetDoctorList().Select(x =>
            {
                var clinicUsers = _organizationservice.GetOrganizationById(x.OrgId);
                return clinicUsers != null ? new RegisterViewModel
                {
                    ProfilePic = _userService.Find(x.LoginId).Avatar,
                    FullName = x.FirstName + " " + x.SurName,
                    MemberId = x.MemberId,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    organizationName = _organizationservice.GetOrganizationById(x.OrgId).OrganizationName + "/" + _clinicservice.GetClinicById(x.OrgId).ClinicName
                } :  new RegisterViewModel();
            }).ToList();
            return View(doctor);
            //var doctors = _doctorservice.GetDoctorList().Select(x => new RegisterViewModel
            //{
            //    ProfilePic = _userService.Find(x.LoginId).Avatar,
            //    FullName = x.FirstName + " " + x.SurName,
            //    MemberId = x.MemberId, 
            //    Email = x.Email,
            //    //organizationName = x.organization.OrganizationName,
            //    IsActive=x.IsActive,
            //    ModyfiedDate=Convert.ToDateTime(x.ModifiedDate) }).ToList();
            //var doctors = _doctorservice.GetDoctorList().Select(x => new RegisterViewModel { FullName = x.FirstName + " " + x.SurName, MemberId = x.MemberId , Email= x.Email}).ToList();
            //return View(doctors);
        }
        public ActionResult GetOrganizationAddress(int Id)
        {
            var OrgAddress = _organizationservice.GetOrganizationById(Id);
            // Returns string "Electronic" or "Mail"
            return Json(OrgAddress, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClinicByID(int Id)
        {
            var Clinic = _clinicservice.GetClinicInOrganization(Id).ToList();
            // Returns string "Electronic" or "Mail"
            return Json(Clinic, JsonRequestBehavior.AllowGet);
        }
        public RegisterViewModel UploadSignature(RegisterViewModel model, HttpPostedFileBase SignaturPic)
        {
            if (SignaturPic != null && SignaturPic.ContentLength > 0)
            {
                var fileName = Path.GetFileName(SignaturPic.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    if (!string.IsNullOrEmpty(model.SignaturePic))
                    {
                        //Delete exising File
                        var file = Server.MapPath(model.SignaturePic);
                        imageHelper.Delete(file);
                    }
                    string fName = DateTime.Now.Ticks + fileName;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/OrganizationLogo/"), fName);
                    var bitmap = new Bitmap(SignaturPic.InputStream);
                    imageHelper.Save(bitmap, 150, 150, 250, path);
                    SignaturPic.SaveAs(path);
                    model.SignaturePic = "/UploadedFiles/Signature/" + fName;
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
                if (model.SignaturePic != null)
                {
                    model.SignaturePic = model.SignaturePic;
                }
            }
            return model;
        }
        public ActionResult DoctorDetail(int id)
        {     
                //var users = new List<RegisterViewModel>();
                var model = _doctorservice.GetDoctorById(id).Select(x => new RegisterViewModel
                {
                  //  FullName = x.FirstName + " " + x.MiddleName + " " + x.SurName,
                    //MiddleName=x.MiddleName,
                    //SurName = x.SurName,
                    Email = x.Email,
                    DOB = x.DOB,
                    GenderId = x.Gender,
                    MobileNumber = x.MobileNumber,
                    FaxNumber = x.FaxNumber,
                    City = x.City,
                    State = x.State,
                    CountryId = x.CountryId,
                    Profession = x.Profession,
                    PostCode = x.PostCode,
                    Timezone = x.TimeZoneString,
                    PhoneNumber = x.PhoneNumber,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    OrgId = x.OrgId,
                    //organizationName = _organizationservice.GetOrganizationById(id).OrganizationName,
                    //ClinicName = _clinicservice.GetClinicById(id).ClinicName,
                    //OrganizationAddress = _organizationservice.GetOrganizationById(id).Address1,

                    //ClinicAddress = _clinicservice.GetClinicById(id).Address1,
                    //CountryName = _countryService.GetCountryById(id).CountryName,
                    //StateName = _stateservice.GetStateById(id).StateName,
                    //CityName = _cityService.GetCityById(id).LookUpCityName,
                    IsActive = x.IsActive,
                    PhoneCode = x.PhoneCode,
                    LanguageSpoken = x.LanguageSpoken,
                    ProfessionCategory = x.ProfessionCategory,
                    IntrestArea = x.IntrestArea,
                    qualification = x.qualification,
                    AHPRANo = x.AHPRANo,
                    VideoProviderNo = x.VideoProviderNo,
                    HomeVisitProviderNo=x.HomeVisitProviderNo,
                    ClinicProviderNo=x.ClinicProviderNo,
                    PrescriberNo = x.PrescriberNo,
                    DoctorProfile = x.DoctorProfile,
                    SignaturePic = x.SignaturePic,
                    MemberId = x.MemberId
                    // OrganizationName = x.Clinic.organization.OrganizationName,
                    // IsApproved = x.IsActive
                }).ToList().FirstOrDefault();
                //cliniUsers = UserService.Find(model.LoginId).Avatar;
             //organizationName = _organizationservice.GetOrganizationById(id).OrganizationName,
                    //ClinicName = _clinicservice.GetClinicById(id).ClinicName,
                    //OrganizationAddress = _organizationservice.GetOrganizationById(id).Address1,

                    //ClinicAddress = _clinicservice.GetClinicById(id).Address1,
                    //CountryName = _countryService.GetCountryById(id).CountryName,
                    //StateName = _stateservice.GetStateById(id).StateName,
                    //CityName = _cityService.GetCityById(id).LookUpCityName,
            //try
            //{ 
                if (model.OrgId !=0)
                {
                    model.organizationName = _organizationservice.GetOrganizationById(model.OrgId).OrganizationName;
                    model.OrganizationAddress = _organizationservice.GetOrganizationById(model.OrgId).Address1;
                }
                if (model.ClinicID != 0 )
                {
                    model.ClinicName = _clinicservice.GetClinicById(model.ClinicID).ClinicName;
                    model.ClinicAddress = _clinicservice.GetClinicById(model.ClinicID).Address1;
                }
               
                 
                
              
                 
                
                if (model.CountryId !=0)
                 {
                     model.CountryName = _countryService.GetCountryById(model.CountryId).CountryName;
                 }
                if (model.State != 0)
                {
                    model.StateName = _stateservice.GetStateById(model.State).StateName;
                }
                if (model.State!=0)
                {
                    model.CityName = _cityService.GetCityById(model.City).LookUpCityName;
                }
                try { 
                    model.ProfilePic = _userService.Find(model.LoginId).Avatar;
                }
                catch { }
                return View(model);
                //var model = _doctorservice.GetDoctorById(Convert.ToInt32(id));
                //return View();
           
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _doctorservice.Activate(ids);
            return RedirectToAction("DoctorList");
        }
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection[0];
            _doctorservice.Delete(ids);
            return RedirectToAction("DoctorList");
        }
        //public ActionResult GetOrganizationAddress()
        //{
        //    var organization = null;

        //    return JsonResult(organization, JsonRequestBehavior.AllowGet);
        //}

    }
}