using MyOnlineClinic.Emailer;
using MyOnlineClinic.Entity;
using System.Data.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
using System.Globalization;
using System.Net;
using PayPal.Api;
using System.Web.Routing;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MyOnlineClinic.Migrator;
using System.IO;
namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    [AuthorizeRole("Admin")]
    public class PatientController : BaseController
    {
        private readonly IAppointmentService appointmentService = null;
        private readonly IHomeVisitAppointmentService _homeVisitAppointmentService = null;
        private readonly IDoctorService doctorService = null;
        private readonly IUserService userService = null;
        protected readonly IUserService UserService;
        private readonly IOrganizationService _organiztionService;
        private readonly IClinicService _clinicService;
        private readonly ILoginHelper _loginHelper;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ILookUpModuleService _moduleService;
        private readonly IPermissionsService _permissionService;
        private readonly ILookUpUserRolesService _roleService;
        private readonly IPermissionInModuleService _permissioninmoduleService;
        private readonly IAdminUserService _adminuserService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ITitleService _TitleServices;
        private readonly IGenderService _genderService;
        private readonly IStateService _stateService;
        private readonly IDoctorService _doctorService;
        public readonly IPatientHistoryService _patientHistoryService;
        private readonly IUregisteredOrganizationService _unregisteredOrganizationService;
        IProfessionTypeService _professiontypeservice;
        IPaymentHelper _paymentService;
        private readonly IPatientService _patientServices;
        private readonly IUnregisteredClinicService _unregisteredClinicService;
        private readonly IPaymentService _servicePayment;
        private readonly IPatientExaminationVisibilityService _patientExaminationVisibilityService;
        private readonly IStandardService _standardService;
        private readonly IVoucherService _voucherService;

        public PatientController(IAppointmentService appointmentService, IDoctorService SdoctorService, IUserService userService, IOrganizationService organiztionService, IClinicService clinicService,
          ICountryService countryService, ICityService cityService, ILookUpModuleService moduleService, IPermissionsService permissionService, ILookUpUserRolesService roleService,
          IPermissionInModuleService permissioninmoduleService, IAdminUserService adminuserService, ITimeZoneService timezoneService, ITitleService TitleServices, IGenderService genderService, IStateService stateService, ILoginHelper loginHelper, IUregisteredOrganizationService unregisteredOrganizationService,
          IProfessionTypeService professiontypeservice, IPaymentHelper paymentService, IPatientService patientServices, IPaymentService servicePayment, IUnregisteredClinicService unregisteredClinicService, IHomeVisitAppointmentService homeVisitAppointmentService, IPatientHistoryService patientHistoryService, IDoctorService doctorService
            , IPatientExaminationVisibilityService patientExaminationVisibilityService, IStandardService standardService, IVoucherService voucherService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = SdoctorService;
            this.userService = userService;
            UserService = userService;
            _organiztionService = organiztionService;
            _clinicService = clinicService;
            _loginHelper = loginHelper;
            _countryService = countryService;
            _cityService = cityService;
            _moduleService = moduleService;
            _permissionService = permissionService;
            _roleService = roleService;
            _permissioninmoduleService = permissioninmoduleService;
            _adminuserService = adminuserService;
            _timeZoneService = timezoneService;
            _TitleServices = TitleServices;
            _genderService = genderService;
            _stateService = stateService;
            _unregisteredOrganizationService = unregisteredOrganizationService;
            _professiontypeservice = professiontypeservice;
            _paymentService = paymentService;
            _patientServices = patientServices;
            _servicePayment = servicePayment;
            _unregisteredClinicService = unregisteredClinicService;
            _homeVisitAppointmentService = homeVisitAppointmentService;
            _patientHistoryService = patientHistoryService;
            _doctorService = doctorService;
            _patientExaminationVisibilityService = patientExaminationVisibilityService;
            _standardService = standardService;
            _voucherService = voucherService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult BookAppointment()
        {
            SearchViewModel model = new SearchViewModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult BookAppointment(FormCollection collection, SearchViewModel model)
        {
            int DoctorId = 0;
            var Docid = model.MemberId;

            DoctorId = Convert.ToInt32(Docid);

            Appointment cModel = new Appointment();
            var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(TempData["AppointmentLocal"]));
            if (AppointmentExisit)
            {
                model.Confirm = "Exist";
                return View(model);
            }

            if (DoctorId > 0)
            {
                var doctorResult = doctorService.GetDoctorById(DoctorId).FirstOrDefault();

                if (doctorResult != null)
                {
                    cModel.DoctorLoginId = doctorResult.LoginId;

                }

                cModel.PatientLoginId = Guid.Parse(TempData["PatientId"].ToString());
                cModel.AppointmentDateLocal = Convert.ToDateTime(TempData["AppointmentLocal"]);
                cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                cModel.AppointmentDateUtc = DateTime.Now;
                cModel.AppointmentType = (int)AppointmentType.VideoConsult;
                appointmentService.Add(cModel);
                model.Confirm = "Book";
            }
            var patient = userService.GetPatientByLoginId(Guid.Parse(TempData["PatientId"].ToString())).MemberId;
            return RedirectToAction("PayWithCreditCard", new
            {
                id = cModel.AppointmentId,
                patientId = patient
            });

        }

        public ActionResult Search(int? id, string name)//, string name
        {
            SearchViewModel model = new SearchViewModel();
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            model.MemberId = Convert.ToInt32(id);
            if (id.HasValue)
            {
                var Cmodel = UserService.GetPatientById(Convert.ToInt32(id)).Select(x => new SearchViewModel
                {
                    PatientId = x.LoginId,
                    org = x.OrgId,
                    Timezone = x.TimeZoneString,

                }).FirstOrDefault();

                var Omodel = _organiztionService.GetOrganizationById(Cmodel.org);
                TempData["PatientId"] = Cmodel.PatientId;
                TempData["OrgId"] = Cmodel.org;
                TempData["orgType"] = Omodel.OrganizationType;
                TempData["Timezone"] = Cmodel.Timezone;
                TempData.Keep("PatientId");
                TempData.Keep("OrgId");
                TempData.Keep("orgType");
                TempData.Keep("Timezone");
            }
            BindType(model);
            //    model.AppointmentType = (int)AppointmentType.VideoConsult;
            if (!string.IsNullOrEmpty(name))
            {
                model.AppointmentType = Convert.ToInt32(name);

            }
            return View(model);

        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Search(SearchViewModel model, FormCollection collection)
        //{

        //    ReadOnlyCollection<TimeZoneInfo> tz;
        //    tz = TimeZoneInfo.GetSystemTimeZones();
        //    var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
        //    ViewBag.Timezone = timeZoneList;
        //    var appointmentlocal = collection.Get("AppointmentDateLocal");
        //    string AppointmentTimeCHeck = collection.Get("FromHour");
        //    if (model.AppointmentDateLocal == null && AppointmentTimeCHeck == "")
        //    {
        //        BindType(model);
        //        ModelState.AddModelError("Error", "Please Select Appointment Date Or time");
        //        return View(model);
        //    }
        //    // var doctorIdList = _clinicService.GetDoctorListByClinicId(Convert.ToInt32(model.Clinicid), (int)RoleType.Doctor);
        //    //List<Entity.Doctor> listDoctor = new List<Entity.Doctor>();
        //    //var doctorIdList = _clinicService.GetDoctorListByClinicId(Convert.ToInt32(clinicid), (int)RoleType.Doctor);

        //    //for (int i = 0; i < doctorIdList.Count; i++)
        //    //{
        //    //    var doctor = doctorService.GetDoctorById(doctorIdList[i].UserId).FirstOrDefault();
        //    //    listDoctor.Add(doctor);
        //    //}

        //    var doctor = doctorService.GetQueryableDoctors();

        //    if (model.DoctorFullName != null)
        //    {
        //        doctor = doctor.FilterByFullName(model.DoctorFullName);
        //    }
        //    if (model.Clinicid > 0)
        //    {
        //        doctor = doctor.FilterByClinic(model.Clinicid);


        //    }
        //    if (model.ProfessionType > 0)
        //    {
        //        doctor = doctor.FilterByCategory(model.ProfessionType);
        //    }
        //    if (model.ProfessionSub > 0)
        //    {
        //        doctor = doctor.FilterByProfessionSubCategory(model.ProfessionSub);
        //    }
        //    if (model.countryid > 0)
        //    {
        //        doctor = doctor.FilterByCountry(model.countryid);
        //    }
        //    if (model.stateid > 0)
        //    {
        //        doctor = doctor.FilterByCountry(model.stateid);
        //    }

        //    if (model.cityid > 0)
        //    {
        //        doctor = doctor.FilterByCity(model.cityid);
        //    }
        //    if (model.language != null)
        //    {
        //        doctor = doctor.FilterByLanguage(model.language);
        //    }

        //    var search = doctor.Select
        //        (x => new SearchViewModel
        //        {
        //            DoctorFullName = x.FirstName,
        //            Qualification = x.qualification,
        //            PhoneNo = x.PhoneNumber,
        //            DoctorProfile = x.DoctorProfile,
        //            DoctorLoginId = x.LoginId,
        //            MemberId = x.MemberId
        //        }).ToList();

        //    //var cityResult = search.Join(
        //    //                             _unregisteredClinicService.GetClinicById(model.Clinicid),
        //    //                             clinic => clinic.Clinicid,
        //    //                             (news, city) => new { News = news, City = city }
        //    //                           )
        //    //                           .GroupBy(g => g.City)
        //    //                            .Select(g => new FilterObject
        //    //                            {
        //    //                                Id = g.Key.LookUpCityId,
        //    //                                Title = g.Key.LookUpCityName,
        //    //                                Count = g.Count()
        //    //                            }).ToList();

        //    for (int i = 0; i < search.Count; i++)
        //    {
        //        var ProfilePic = string.Empty;
        //        try
        //        {
        //            if (doctorService.GetDoctorById(search[i].MemberId) != null)
        //            {

        //                search[i].ProfilePic = UserService.Find(search[i].DoctorLoginId).Avatar;

        //            }

        //        }
        //        catch { }


        //    }

        //    model.AppointmentList = search;
        //    string AppointmentTime = collection.Get("FromHour") + ":" + collection.Get("FromMin") + " " + collection.Get("FromAMPM");
        //    DateTime dtAppointdate = DateTime.ParseExact(appointmentlocal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    string AppointmentLocal = dtAppointdate.ToString("dd/MM/yyyy").ToString() + " " + AppointmentTime;
        //    var timezone = TempData["Timezone"];
        //    TempData["AppointmentLocal"] = AppointmentLocal;
        //    TempData.Keep("AppointmentLocal");
        //    BindType(model);
        //    return View(model);
        //}

        //GET ://patietn Registraion by Admin

        [AllowAnonymous]
        public ActionResult Add(int? id)
        {
            RegisterViewModel model = new RegisterViewModel();
            if (id.HasValue)
            {
                var Cmodel = UserService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
                {
                    FirstName = StringCipher.Decrypt(x.FirstName),
                    SurName = StringCipher.Decrypt(x.SurName),
                    State = x.State,
                    CountryId = x.CountryId,
                    City = x.City,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    Email = StringCipher.Decrypt(x.Email),
                    DOB = x.DOB,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    MemberId = x.MemberId,
                    MiddleName = StringCipher.Decrypt(x.MiddleName),
                    MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                    MedicareRefNumber = StringCipher.Decrypt(x.MedicareRefNo),
                    MedicareExpire = x.Expiry,
                    DavCardColor = x.DavColorCard,
                    DavNumber = StringCipher.Decrypt(x.DVANumber),
                    PrivateFund = StringCipher.Decrypt(x.PrivateFund),
                    PrivateFundMembershipNo = StringCipher.Decrypt(x.PrivateFundMembershipNo),
                    HCCPensionNo = StringCipher.Decrypt(x.HCCPensionNo),
                    HCCPensionNoExpiry = StringCipher.Decrypt(x.HCCPensionNoExpiry),
                    UsualGPName = StringCipher.Decrypt(x.UsualGPName),
                    UsualGPAddress = StringCipher.Decrypt(x.UsualGPAddress),
                    DavDisablities = StringCipher.Decrypt(x.DavDisablities),
                    UsualGPClinicName = StringCipher.Decrypt(x.UsualGPClinicName),
                    GPAddress2 = StringCipher.Decrypt(x.GPAddress2),
                    UsualGpCity = x.UsualGpCity,
                    UsualGpCountry = x.UsualGpCountry,
                    UsualGpState = x.UsualGpState,
                    UsualGpPostCode = StringCipher.Decrypt(x.UsualGpPostCode),
                    // organizationName = _organiztionService.GetOrganizationById(x.OrgId).OrganizationName,
                    PharmacyName = StringCipher.Decrypt(x.PharmacyName),
                    PharmacyAddress1 = StringCipher.Decrypt(x.PharmacyAddress1),
                    PharmacyAddress2 = StringCipher.Decrypt(x.PharmacyAddress2),
                    PharmacySuburb = StringCipher.Decrypt(x.PharmacySuburb),
                    PharmacyCity = x.PharmacyCity,
                    PharmacyCountryId = x.PharmacyCountryId,
                    PharmacyFaxNumber = StringCipher.Decrypt(x.PharmacyFaxNumber),
                    PharmacyMobileNumber = StringCipher.Decrypt(x.PharmacyMobileNumber),
                    PharmacyPhoneNumber = StringCipher.Decrypt(x.PharmacyPhoneNumber),
                    PharmacyPostCode = StringCipher.Decrypt(x.PharmacyPostCode),
                    PharmacyState = x.PharmacyState,
                    GernealNotes = x.GernealNotes,
                    TreatingDoctors = StringCipher.Decrypt(x.TreatingDoctors),
                    GenderId = x.Gender,
                    TitleId = x.Title,
                    OrgId = x.OrgId,
                    LoginId = x.LoginId,
                    //UserorgId = x.OrgId
                    // PatientOrgId = _patientServices.GetUserOrganizationById(x.MemberId).UserOrganizationId,
                     TimeZoneId = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString)


                }).FirstOrDefault();

                if (_organiztionService.GetOrganizationById(Cmodel.OrgId) != null)
                {
                    Cmodel.organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(Cmodel.OrgId).OrganizationName);
                    Cmodel.OrganizationAddress1 = StringCipher.Decrypt(_organiztionService.GetOrganizationById(Cmodel.OrgId).Address1);
                    Cmodel.OrganizationAddress2 = StringCipher.Decrypt(_organiztionService.GetOrganizationById(Cmodel.OrgId).Address2);
                    Cmodel.OrganizationCountryId = _organiztionService.GetOrganizationById(Cmodel.OrgId).Country;
                    Cmodel.OrganizationCityId = _organiztionService.GetOrganizationById(Cmodel.OrgId).City;
                    Cmodel.OrganizationStateId = _organiztionService.GetOrganizationById(Cmodel.OrgId).State;
                    Cmodel.OrganizationPostalCode = StringCipher.Decrypt(_organiztionService.GetOrganizationById(Cmodel.OrgId).PostCode);
                }


                Cmodel.OrgClinicIdsList = _patientServices.GetClinicById(Convert.ToInt32(id)).Select(x =>
                    new RegisterViewModel
                    {
                        UserClinicId = x.ClinicId,
                        UserClinicName = StringCipher.Decrypt(x.Clinics.ClinicName),
                        UserClinicAddress1 = StringCipher.Decrypt(x.Clinics.Address1),
                        UserClinicAddress2 = StringCipher.Decrypt(x.Clinics.Address2),
                        UserClinicPostalCode = StringCipher.Decrypt(x.Clinics.PostCode),
                        UserClinicCountryId = x.Clinics.Country,
                        UserClinicStateId = x.Clinics.State,
                        UserClinicCityId = x.Clinics.City


                    }).ToList();

                Cmodel.ListoftreatingDoctors = doctorService.GetTreatingDoctorsList().Where(x => x.PatientId == Cmodel.MemberId && x.IsActive == true && x.IsDeleted == false).Select(x => new TreatingDoctorViewModel
                {
                    FirstName = StringCipher.Decrypt(doctorService.GetDoctorById(x.DoctorId).FirstOrDefault().FirstName),
                    LastName = StringCipher.Decrypt(doctorService.GetDoctorById(x.DoctorId).FirstOrDefault().SurName),
                    DoctorId = x.DoctorId
                }).ToList();

                for (int i = 0; i < Cmodel.OrgClinicIdsList.Count; i++)
                {
                    Cmodel.UpdateOrgClinicIds += Cmodel.OrgClinicIdsList[i].UserClinicId + ",";
                }

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
                if (model.PharmacyState != null)
                {

                    model.PharmacyStateName = StringCipher.Decrypt(_stateService.GetStateById(model.PharmacyState).StateName);
                }

                if (model.PharmacyCountryId != null)
                {

                    model.PharmacyCountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.PharmacyCountryId).CountryName);
                }
                if (model.PharmacyCity != null)
                {

                    model.PharmacyCityName = StringCipher.Decrypt(_cityService.GetCityById(model.PharmacyCity).LookUpCityName);
                }
                if (model.ProfilePic != null)
                {
                    model.ProfilePic = UserService.Find(model.LoginId).Avatar;
                }
                if (Cmodel.ProfilePic == null)
                {
                    Cmodel.ProfilePic = UserService.Find(Cmodel.LoginId).Avatar;
                }
                BindDropDown(Cmodel);

                return View(Cmodel);
            }
            else
            {
                BindDropDown(model);
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Add(RegisterViewModel model, HttpPostedFileBase ProfilePic)
        {
            Patient updateprofile = null;
            int Unorgid = 0;
            updateprofile = model.AdminEditMember(model);

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneId);
            TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
            updateprofile.TimeZoneString = Common.GetTimeZoneOffset(offset);

            if (model.MemberId > 0)
            {
                updateprofile.DOB = DateTime.Now;
                updateprofile.ModifiedBy = base.loginUserModel.LoginId;
                updateprofile.ModifiedDate = DateTime.UtcNow;
                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                }
                UserService.UpdatePatient(updateprofile);

                if (model.MemberId > 0)
                {

                    if (!string.IsNullOrEmpty(model.ProfilePic))
                    {
                        var userid = UserService.Find(model.LoginId);
                        userid.Avatar = model.ProfilePic;
                        UserService.Update(userid);
                    }
                }

                //Update Treating Doctors
                if (model.TreatingDoctorsIds != null)
                {

                    var length = doctorService.GetTreatingDoctorsList().Where(x => x.PatientId == model.MemberId).ToList();

                    if (length != null)
                    {
                        for (int i = 0; i < length.Count(); i++)
                        {
                            length[i].IsDeleted = true;
                            length[i].IsActive = false;
                            doctorService.UpdateTreatingDoctor(length[i]);
                        }
                    }
                    string[] id = model.TreatingDoctorsIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < id.Count(); i++)
                    {

                        TreatingDoctors treatingDoctors = new TreatingDoctors();
                        treatingDoctors.DoctorId = Convert.ToInt32(id[i]);
                        treatingDoctors.PatientId = model.MemberId;
                        treatingDoctors.OrgId = model.OrgId;
                        treatingDoctors.CreatedBy = base.loginUserModel.LoginId;
                        _patientServices.AddTreating(treatingDoctors);


                    }
                }


                ////Add User Organization By dheeraj
                //if (model.OrgId == 0)
                //{

                //    var unregisteredOrganizations = new UnregisteredOrganizations
                //    {
                //        Name = StringCipher.Encrypt(model.organizationName),
                //        AddressLine1 = StringCipher.Encrypt(model.OrganizationAddress1),
                //        AddressLine2 = StringCipher.Encrypt(model.OrganizationAddress2),
                //        Country = model.OrganizationCountryId,
                //        State = model.OrganizationStateId,
                //        City = model.OrganizationCityId,
                //        PostCode = StringCipher.Encrypt(model.OrganizationPostalCode),
                //        UserId = updateprofile.MemberId,
                //        Type = (int)RoleType.Patient,
                //        IsActive = false,
                //        ModifiedBy = base.loginUserModel.LoginId
                //    };
                //    _unregisteredOrganizationService.Update(unregisteredOrganizations);
                //    Unorgid = unregisteredOrganizations.Id;
                //}
                //delete clinic

                //Add User Clinic By dheeraj
                if (model.OrgClinicIds != null)
                {
                    var length = entitiesDb.UserClinics.Where(w => w.UserId == model.MemberId && w.Type == (int)RoleType.Patient).ToList();
                    if (length != null)
                    {
                        for (int i = 0; i < length.Count; i++)
                        {
                            UserClinic studentToDelete = entitiesDb.UserClinics.Where(s => s.UserId == model.MemberId).FirstOrDefault<UserClinic>();
                            entitiesDb.Entry(studentToDelete).State = System.Data.Entity.EntityState.Deleted;
                            entitiesDb.SaveChanges();
                        }
                    }

                    string[] id = model.OrgClinicIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < id.Length; i++)
                    {
                        var UserClinic = new UserClinic
                        {
                            UserId = updateprofile.MemberId,
                            OrganizationId = model.OrgId,
                            Type = (int)RoleType.Patient,
                            ClinicId = Convert.ToInt32(id[i]),
                            UserClinicId = model.ClinicID
                        };
                        _unregisteredOrganizationService.AddClinic(UserClinic);
                    }

                }

                BindDropDown(model);

            }
            else
            {

                var userExists = UserService.CheckDuplicateUserName(StringCipher.Encrypt(model.UserName));
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                }
                var userEmailExists = UserService.CheckDuplicateMail(StringCipher.Encrypt(model.Email));
                if (userEmailExists)
                {
                    ModelState.AddModelError("Email", "User with this email id already exists.");
                    ViewBag.ErrorMessage = "This email address already exists.";
                    BindDropDown(model);
                    return View(model);
                }


                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                }


                BindDropDown(model);
                Patient profile = null;
                model.RoleId = RoleType.Patient;
                profile = model.GetAdminPatient(model.UserorgId);
                var user = model.GetLoginAdmin(profile, model.Email);
                var Password = RandomPassword.Generate(6, 8);
                user.IsEmailVerified = true;
                user.IsActive = true;
                user.IsApproved = true;
                user.LoginPassword = Password;
                UserService.Add(user);

                //Add If Organization not exsit User Unregister Organization By dheeraj
                if (model.UserorgId == 0)
                {

                    var unregisteredOrganizations = new UnregisteredOrganizations
                    {
                        Name = StringCipher.Encrypt(model.organizationName),
                        AddressLine1 = StringCipher.Encrypt(model.OrganizationAddress1),
                        AddressLine2 = StringCipher.Encrypt(model.OrganizationAddress2),
                        Country = model.OrganizationCountryId,
                        State = model.OrganizationStateId,
                        City = model.OrganizationCityId,
                        PostCode = StringCipher.Encrypt(model.OrganizationPostalCode),
                        UserId = user.Member.MemberId,
                        Type = (int)RoleType.Patient,
                        IsActive = false
                    };
                    _unregisteredOrganizationService.Add(unregisteredOrganizations);
                    Unorgid = unregisteredOrganizations.Id;
                }
                //Add User Clinic By dheeraj
                if (model.OrgClinicIds != null)
                {
                    string[] id = model.OrgClinicIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < id.Length; i++)
                    {
                        var UserClinic = new UserClinic
                        {
                            UserId = user.Member.MemberId,
                            OrganizationId = model.UserorgId,
                            Type = (int)RoleType.Patient,
                            ClinicId = Convert.ToInt32(id[i])
                        };
                        _unregisteredOrganizationService.AddClinic(UserClinic);
                    }

                }
                //Add If Clinic not exsit User Unregister Clinic By dheeraj
                else
                {
                    var UnRegisteredClinic = new UnRegisteredClinic
                    {
                        UserId = user.Member.MemberId,
                        Name = StringCipher.Encrypt(model.UserClinic),
                        OrgId = Unorgid,
                        AddressLine1 = StringCipher.Encrypt(model.ClinicAddress1),
                        AddressLine2 = StringCipher.Encrypt(model.ClinicAddress2),
                        Country = model.ClinicCountryId,
                        State = model.ClinicStateId,
                        City = model.ClinicCityId,
                        PostCode = StringCipher.Encrypt(model.ClinicPostCodeCode),
                        Type = (int)RoleType.Patient,
                        IsActive = false
                    };
                    _unregisteredClinicService.Add(UnRegisteredClinic);
                }
                //Add Multiple Organization And treating Doctor
                if (model.OrgClinicDoctorsIds != null)
                {
                    string[] id = model.OrgClinicDoctorsIds.Split(new[] { "$", "#" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < id.Length; j = j + 2)
                    {
                        TreatingDoctors treatingDoctors = new TreatingDoctors();
                        int DoctorId = Convert.ToInt32(id[j + 1].Replace("DoctorId=", ""));
                        string ClinicDocIds = id[j].Replace("ClinicId=", "");
                        string[] arrClinicId = ClinicDocIds.Split(',');

                        for (int i = 0; i < arrClinicId.Length; i++)
                        {
                            treatingDoctors.DoctorId = DoctorId;
                            treatingDoctors.PatientId = user.Member.MemberId;
                            treatingDoctors.OrgId = model.UserorgId;
                            treatingDoctors.CreatedBy = base.loginUserModel.LoginId;
                            _patientServices.AddTreating(treatingDoctors);
                        }
                    }
                }

                //string activationToken = user.ActivationToken.ToString();
                //   string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
                var fileUrl = Server.MapPath("~/EmailTemplates/PatientRegistration.html");
                string html = System.IO.File.ReadAllText(fileUrl);
                html = html.Replace("@@Password", Password);
                EmailService objEmail = new EmailService();
                Task taskA = new Task(() => objEmail.SendEmail("User Registration", html, model.Email, "noreply@myonlineclinic.com.au"));
                // Start the task.
                taskA.Start();


                // add patient Examination Visibility Default Entry
                PatientExaminationVisibility objexamvisiblility = new PatientExaminationVisibility();
                objexamvisiblility.PatientLoginId = user.LoginId;
                objexamvisiblility.AllExamination_YN = true;
                objexamvisiblility.AllInOnevideo_YN = true;
                objexamvisiblility.BloodGulose_YN = true;
                objexamvisiblility.BloodPressure_YN = true;
                objexamvisiblility.Bmi_YN = true;
                objexamvisiblility.ECG_YN = true;
                objexamvisiblility.Ent_YN = true;
                objexamvisiblility.HeartRate_YN = true;
                objexamvisiblility.Height_YN = true;
                objexamvisiblility.Oxygen_YN = true;
                objexamvisiblility.Skin_YN = true;
                objexamvisiblility.Temprature_YN = true;
                objexamvisiblility.Waist_YN = true;
                objexamvisiblility.Weight_YN = true;
                _patientExaminationVisibilityService.Add(objexamvisiblility);

                //set Standard Default entry
                SetStandard objstandard = new SetStandard();
                objstandard.PatientId = user.LoginId;
                objstandard.BloodGlucoseAfterBreakfastMaxValue = 0;
                objstandard.BloodGlucoseAfterBreakfastMinValue = 0;
                objstandard.BloodGlucoseAfterLunchMaxValue = 0;
                objstandard.BloodGlucoseAfterLunchMinValue = 0;
                objstandard.BloodGlucoseBeforeBedMaxValue = 0;
                objstandard.BloodGlucoseBeforeBedMinValue = 0;
                objstandard.BloodGlucoseBeforeBreakfastMaxValue = 0;
                objstandard.BloodGlucoseBeforeBreakfastMinValue = 0;
                objstandard.BloodGlucoseBeforeDinnerMaxValue = 0;
                objstandard.BloodGlucoseBeforeDinnerMinValue = 0;
                objstandard.BloodGlucoseBeforeLunchMaxValue = 0;
                objstandard.BloodGlucoseUnit = "mmol/l";
                objstandard.BloodOxygenMaxValue = 0;
                objstandard.BloodOxygenMinValue = 0;
                objstandard.BloodOxygenUnit = "%";
                objstandard.BloodPressureDiastolicMaxValue = 0;
                objstandard.BloodPressureDiastolicMinValue = 0;
                objstandard.BloodPressureSystolicMaxValue = 0;
                objstandard.BloodPressureSystolicMinValue = 0;
                objstandard.BloodPressureUnit = "mmhg";
                objstandard.BMIMaxValue = 0;
                objstandard.BMIMinValue = 0;
                objstandard.BMIUnit = "kg/m2";
                objstandard.PulseRateMax = 0;
                objstandard.PulseRateMin = 0;
                objstandard.PulseUnit = "bpm";
                objstandard.TemperatureMaxValue = 0;
                objstandard.TemperatureMinValue = 0;
                objstandard.TemperatureUnit = "°C";
                _standardService.Add(objstandard);



            }


            return RedirectToAction("FindPatient");
        }
        public ActionResult FindPatient(int? id)
        {

            if (id.HasValue) { ViewBag.message = true; }
            Session["VoucherID"] = id;
            var activatepatient = userService.GetPatientList();
            for (int i = 0; i < activatepatient.Count; i++)
            {
                activatepatient[i].IsApproved = userService.Find(activatepatient[i].LoginId).IsApproved;
            }
            var patient = activatepatient.Where(x => x.IsApproved == true).Select(x =>
            {
                return new RegisterViewModel()
                {
                    FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    LoginId = x.LoginId,
                    VoucherId = id,                    
                    ProfilePic = UserService.Find(x.LoginId).Avatar,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                };
            }).OrderByDescending(y => y.ModiNfiedDate).ToList();
            for (int i = 0; i < patient.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organiztionService.GetOrganizationById(patient[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(patient[i].OrgId).OrganizationName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    patient[i].organizationName = organizationName;
                }
            }

            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).Where(x => x.ModuleId == 9 && x.IsActive == true && x.IsDeleted == false && x.IsApproved == true).ToList();
            return View(patient);
        }
        [HttpPost]
        public ActionResult FindPatient(int? id, SearchParametersViewModel searchModel)
        {

            if (id.HasValue) { ViewBag.message = true; }
            Session["VoucherID"] = id;
            var activatepatient = userService.GetPatientList();
            for (int i = 0; i < activatepatient.Count; i++)
            {
                activatepatient[i].IsApproved = userService.Find(activatepatient[i].LoginId).IsApproved;
            }
            var patient = activatepatient.Where(x => x.IsApproved == true).Select(x =>
            {
                return new RegisterViewModel()
                {
                    FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    LoginId = x.LoginId,
                    VoucherId = id,
                    ModiNfiedDate = x.CreatedDate,
                    ProfilePic = UserService.Find(x.LoginId).Avatar,
                    CountryId = x.CountryId,
                    State = x.State,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                };
            }).OrderByDescending(y => y.ModiNfiedDate).ToList();
            for (int i = 0; i < patient.Count; i++)
            {
                var cliniccount = _doctorService.GetUserclinicList().Where(x => x.Type == (int)RoleType.Patient && x.UserId == patient[i].MemberId).ToList().GroupBy(x => x.ClinicId).Select(group => group.First()).ToList();
                //var cliniccount = _userService.GetClinicList(Convert.ToInt32(model.MemberId), (int)RoleType.Doctor);
                var clinicname = string.Empty;
                if (cliniccount != null)
                {
                    for (int j = 0; j < cliniccount.Count; j++)
                    {
                        clinicname += StringCipher.Decrypt(_clinicService.GetClinicById(cliniccount[j].ClinicId).ClinicName) + ",";
                    }
                }
                var organizationName = string.Empty;
                try
                {
                    if (_organiztionService.GetOrganizationById(patient[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(patient[i].OrgId).OrganizationName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    patient[i].organizationName = organizationName;
                }
                patient[i].ClinicName = clinicname;
            }

            patient = patient.Where(x =>
                (
                 (searchModel.OrgName != null && x.organizationName != null && x.organizationName.IndexOf(searchModel.OrgName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.OrgName == null)
                  &&
               ((searchModel.ClinicName != null && x.ClinicName.Contains(searchModel.ClinicName)) || searchModel.ClinicName == null)
              &&
              ((searchModel.PostCode != null && x.PostCode.IndexOf(searchModel.PostCode, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.PostCode == null)
              &&
              ((searchModel.CountryId != 0 && x.CountryId == searchModel.CountryId) || searchModel.CountryId == 0)
              && ((searchModel.StateId != 0 && x.State == searchModel.StateId) || searchModel.StateId == 0)
                    //&& ((searchModel.OrgAdmin != 0 && x.FaxNumber == Convert.ToString(searchModel.OrgAdmin)) || searchModel.OrgAdmin == 0)
              && ((searchModel.ActivationStatus != null && x.IsActive == Convert.ToBoolean(searchModel.ActivationStatus)) ||
                    searchModel.ActivationStatus == null)
                      && ((searchModel.PatientName != null && x.FullName.IndexOf(searchModel.PatientName, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   searchModel.PatientName == null)
             && ((searchModel.Email != null && x.Email.IndexOf(searchModel.Email, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   searchModel.Email == null)
             ).ToList();
            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).Where(x => x.ModuleId == 9 && x.IsActive == true && x.IsDeleted == false && x.IsApproved == true).ToList();
            return View(patient);

        }
        public ActionResult RecentPatient(int? id)
        {

            if (id.HasValue) { ViewBag.message = true; }
            Session["VoucherID"] = id;
            var activatepatient = userService.GetPatientList();
            for (int i = 0; i < activatepatient.Count; i++)
            {
                activatepatient[i].IsApproved = userService.Find(activatepatient[i].LoginId).IsApproved;
            }
            var patient = activatepatient.Where(x => x.IsApproved == false).Select(x =>
            {
                return new RegisterViewModel()
                {
                    FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    VoucherId = id,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    ProfilePic = UserService.Find(x.LoginId).Avatar
                };
            }).OrderByDescending(y => y.ModiNfiedDate).ToList();
            for (int i = 0; i < patient.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organiztionService.GetOrganizationById(patient[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(patient[i].OrgId).OrganizationName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    patient[i].organizationName = organizationName;
                }
            }

            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).Where(x => x.ModuleId == 9 && x.IsActive == true && x.IsDeleted == false && x.IsApproved == true).ToList();
            return View(patient);
        }
        [HttpPost]
        public ActionResult RecentPatient(int? id, SearchParametersViewModel searchModel)
        {
            if (id.HasValue) { ViewBag.message = true; }
            Session["VoucherID"] = id;
            var query = userService.GetPatientList();
            //var query = _organiztionService.GetOrganizationList();
            for (int i = 0; i < query.Count; i++)
            {
                query[i].IsApproved = userService.Find(query[i].LoginId).IsApproved;
            }

            var patient = query.Where(x => x.IsApproved == false).Select(x =>
            {
                return new RegisterViewModel()
                {
                    FullName = StringCipher.Decrypt(x.FirstName + " " + x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    VoucherId = id,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    ProfilePic = UserService.Find(x.LoginId).Avatar
                };
            }).OrderByDescending(y => y.ModyfiedDate).ToList();
            for (int i = 0; i < patient.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organiztionService.GetOrganizationById(patient[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(patient[i].OrgId).OrganizationName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    patient[i].organizationName = organizationName;
                }
            }
            patient = patient.Where(x =>
              (
               (searchModel.OrgName != null && x.organizationName.IndexOf(searchModel.OrganizationName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.OrganizationName == null)

               && ((searchModel.PostCode != null && x.PostCode.IndexOf(searchModel.PostCode, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.PostCode == null)
               && ((searchModel.CountryId != 0 && x.CountryId == searchModel.CountryId) || searchModel.CountryId == 0)
               && ((searchModel.StateId != 0 && x.State == searchModel.StateId) || searchModel.StateId == 0)
               && ((searchModel.OrgAdmin != 0 && x.FaxNumber == Convert.ToString(searchModel.OrgAdmin)) || searchModel.OrgAdmin == 0)
               && ((searchModel.ActivationStatus != null && x.IsActive == Convert.ToBoolean(searchModel.ActivationStatus)) ||
                     searchModel.ActivationStatus == null)
                       && ((searchModel.DoctorName != null && x.FullName.IndexOf(searchModel.DoctorName, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    searchModel.DoctorName == null)
              && ((searchModel.Email != null && x.FullName.IndexOf(searchModel.Email, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    searchModel.Email == null)

              ).ToList();
            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).Where(x => x.ModuleId == 9 && x.IsActive == true && x.IsDeleted == false && x.IsApproved == true).ToList();
            return View(patient);

        }
        public SearchViewModel BindType(SearchViewModel model)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> Prof = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = AppointmentType.VideoConsult.ToString(), Value = ((int)AppointmentType.VideoConsult).ToString() });
            items.Add(new SelectListItem { Text = AppointmentType.HomeVisit.ToString(), Value = ((int)AppointmentType.HomeVisit).ToString() });
            items.Add(new SelectListItem { Text = AppointmentType.ClinicVisit.ToString(), Value = ((int)AppointmentType.ClinicVisit).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.Clinic.ToString(), Value = ((int)Profession.Clinic).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.GP.ToString(), Value = ((int)Profession.GP).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.Allied_Health.ToString(), Value = ((int)Profession.Allied_Health).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.Specialist.ToString(), Value = ((int)Profession.Specialist).ToString() });
            model.AppointmentTypeList = items;
            model.ProfessionList = Prof;
            model.CountryList = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();
            model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();
            model.ProfessionTypes = _professiontypeservice.GetProfessionTypesList().Select(x => new ProfessionTypes { Id = x.Id, ProfessionName = StringCipher.Decrypt(x.ProfessionName) }).ToList();
            model.ProfessionSubList = _professiontypeservice.GetProfessionSubTypesList();
            var ClinicDecrypt = _clinicService.GetClinicsListbyOrgid(Convert.ToInt32(TempData["OrgId"]), Convert.ToInt32(TempData["orgType"])).ToList();
            for (int i = 0; i < ClinicDecrypt.Count; i++)
            {
                ClinicDecrypt[i].ClinicName = StringCipher.Decrypt(ClinicDecrypt[i].ClinicName);
            }
            model.ClinicList = ClinicDecrypt;
            return model;

        }
        public RegisterViewModel BindDropDown(RegisterViewModel model)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var genderlist = _genderService.GetGenderList().Where(t => t.Type == (int)RoleType.Patient && t.IsActive == true).ToList();
            for (int i = 0; i < genderlist.Count; i++)
            {
                genderlist[i].GenderName = StringCipher.Decrypt(genderlist[i].GenderName);
            }
            model.GenderList = genderlist;

            // = _countryService.GetCountryList();
            var countrylist = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();
            for (int i = 0; i < countrylist.Count; i++)
            {
                countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
            }
            model.CountryList = countrylist;
            var Citylist = _cityService.GetCityList().Where(x => x.IsActive == true).ToList();
            for (int i = 0; i < Citylist.Count; i++)
            {
                Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
            }
            model.CityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();
            //model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            //model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();          
            var Statelist = _stateService.GetStateList().Where(x => x.IsActive == true).ToList(); 
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.StateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            model.OrganizationList = _organiztionService.GetOrganizationList();
            for (int i = 0; i < model.OrganizationList.Count; i++)
            {
                model.OrganizationList[i].OrganizationName = StringCipher.Decrypt(model.OrganizationList[i].OrganizationName);
            }


            var titlelist = _TitleServices.GetTitleList().Where(t => t.Type == (int)RoleType.Patient && t.IsActive == true).ToList();
            for (int i = 0; i < titlelist.Count; i++)
            {
                titlelist[i].TitleName = StringCipher.Decrypt(titlelist[i].TitleName);
            }
            model.TitleList = titlelist;

            items.Add(new SelectListItem { Text = DVACardcolor.Gold.ToString(), Value = ((int)DVACardcolor.Gold).ToString() });
            items.Add(new SelectListItem { Text = DVACardcolor.White.ToString(), Value = ((int)DVACardcolor.White).ToString() });
            items.Add(new SelectListItem { Text = DVACardcolor.Orange.ToString(), Value = ((int)DVACardcolor.Orange).ToString() });
            model.dvacolor = items;
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();

            ViewBag.Timezone = timeZoneList;
            var clinic = entitiesDb.Clinics.Select(p => new SelectListItem { Value = p.ClinicName, Text = p.ClinicName }).ToList();
            ViewBag.Clinic = clinic;

            model.OrganizationCountryList = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();
            model.OrganizationCityList = Citylist;
            // model.OrganizationCityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            //model.OrganizationStateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();
            model.OrganizationStateList = Statelist;
            //: new List<LookUpState>();
            model.ClinicCountryList = countrylist;
            model.ClinicCityList = Citylist;
            // model.ClinicCityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            model.ClinicStateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            return model;
        }
        public ActionResult GetClinicByID(int Id)
        {
            var Clinic = _clinicService.GetClinicInOrganization(Id).ToList();

            return Json(Clinic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {
            #region Detail
            var model = UserService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
            {
                FullName = StringCipher.Decrypt(_TitleServices.GetTitleById(x.Title).TitleName) + " " + StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                FirstName = StringCipher.Decrypt(x.FirstName),
                SurName = StringCipher.Decrypt(x.SurName),
                Title = StringCipher.Decrypt(_TitleServices.GetTitleById(x.Title).TitleName),
                State = x.State,
                CountryId = x.CountryId,
                City = x.CountryId,
                Address1 = StringCipher.Decrypt(x.Address1),
                MiddleName = StringCipher.Decrypt(x.MiddleName),
                Address2 = StringCipher.Decrypt(x.Address2),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                PostCode = StringCipher.Decrypt(x.PostCode),
                OrgId = x.OrgId,
                MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                MedicareRefNumber = StringCipher.Decrypt(x.MedicareRefNo),
                MedicareExpire = x.Expiry,
                DavCardColor = x.DavColorCard,
                DavNumber = StringCipher.Decrypt(x.DVANumber),
                PrivateFund = StringCipher.Decrypt(x.PrivateFund),
                PrivateFundMembershipNo = StringCipher.Decrypt(x.PrivateFundMembershipNo),
                HCCPensionNo = StringCipher.Decrypt(x.HCCPensionNo),
                HCCPensionNoExpiry = StringCipher.Decrypt(x.HCCPensionNoExpiry),
                UsualGPName = StringCipher.Decrypt(x.UsualGPName),
                UsualGPAddress = StringCipher.Decrypt(x.UsualGPAddress),
                DavDisablities = StringCipher.Decrypt(x.DavDisablities),
                UsualGPClinicName = StringCipher.Decrypt(x.UsualGPClinicName),
                GPAddress2 = StringCipher.Decrypt(x.GPAddress2),
                UsualGpCity = x.UsualGpCity,
                UsualGpCountry = x.UsualGpCountry,
                UsualGpState = x.UsualGpState,
                UsualGpPostCode = StringCipher.Decrypt(x.UsualGpPostCode),
                PharmacyName = StringCipher.Decrypt(x.PharmacyName),
                PharmacyAddress1 = StringCipher.Decrypt(x.PharmacyAddress1),
                PharmacyAddress2 = StringCipher.Decrypt(x.PharmacyAddress2),
                PharmacySuburb = StringCipher.Decrypt(x.PharmacySuburb),
                PharmacyCity = x.PharmacyCity,
                PharmacyCountryId = x.PharmacyCountryId,
                PharmacyFaxNumber = StringCipher.Decrypt(x.PharmacyFaxNumber),
                PharmacyMobileNumber = StringCipher.Decrypt(x.PharmacyMobileNumber),
                PharmacyPhoneNumber = StringCipher.Decrypt(x.PharmacyPhoneNumber),
                PharmacyPostCode = StringCipher.Decrypt(x.PharmacyPostCode),
                PharmacyState = x.PharmacyState,
                GernealNotes = StringCipher.Decrypt(x.GernealNotes),
                TreatingDoctors = StringCipher.Decrypt(x.TreatingDoctors),
                LoginId = x.LoginId,
                GenderId = x.Gender,
                MemberId = x.MemberId,
                DVAColorName = x.DavColorCard == (Int32)DVACardcolor.Gold ? "Gold" : x.DavColorCard == (Int32)DVACardcolor.Orange ? "Orange" : x.DavColorCard == (Int32)DVACardcolor.White ? "White" : "",
                //GPCountryName = StringCipher.Decrypt(_countryService.GetCountryById(x.UsualGpCountry).CountryName),
                //GPStateName = StringCipher.Decrypt(_stateService.GetStateById(x.UsualGpState).StateName),
                //GPCityName = StringCipher.Decrypt(_cityService.GetCityById(x.UsualGpCity).LookUpCityName)

            }).ToList().FirstOrDefault();

            var TreatingDoctor = doctorService.GetTreatingDoctorByPatientId(model.MemberId).Where(x => x.IsActive == true && x.IsDeleted == false).Select(x => new TreatingDoctorViewModel
            {
                TitleName = StringCipher.Decrypt(_TitleServices.GetTitleById(x.Doctors.Title).TitleName),
                FirstName = StringCipher.Decrypt(x.Doctors.FirstName),
                LastName = StringCipher.Decrypt(x.Doctors.SurName),
                IsDelete = x.IsDeleted,
                IsActive = x.IsActive


            }).ToList();
            if (model.State != null)
            {
                model.StateName = StringCipher.Decrypt(_stateService.GetStateById(model.State).StateName);
            }
            if (model.UsualGpState != null)
            {
                model.usualGpStateName = StringCipher.Decrypt(_stateService.GetStateById(model.UsualGpState).StateName);
            }
            if (model.UsualGpCity != null)
            {
                model.GPCityName = StringCipher.Decrypt(_cityService.GetCityById(model.UsualGpCity).LookUpCityName);
            }
            if (model.CountryId != null)
            {
                model.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.CountryId).CountryName);
            }
            if (model.City != null)
            {
                model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);
            }
            if (model.OrgId != 0)
            {

                model.OrganizationDetails = _organiztionService.GetOrganizationList().Where(x => x.OrganizationId == model.OrgId).Select(x =>
               new OrganizationViewModel
               {
                   OrganizationName = StringCipher.Decrypt(x.OrganizationName) == null ? string.Empty : StringCipher.Decrypt(x.OrganizationName),
                   Address1 = StringCipher.Decrypt(x.Address1) == null ? string.Empty : StringCipher.Decrypt(x.Address1),
                   FaxNumber = StringCipher.Decrypt(x.FaxNumber) == null ? string.Empty : StringCipher.Decrypt(x.FaxNumber),
                   PostCode = StringCipher.Decrypt(x.PostCode) == null ? string.Empty : StringCipher.Decrypt(x.PostCode),
                   Address2 = StringCipher.Decrypt(x.Address2) == null ? string.Empty : StringCipher.Decrypt(x.Address2),
                   PhoneNumber = StringCipher.Decrypt(x.PhoneNumber) == null ? string.Empty : StringCipher.Decrypt(x.PhoneNumber),
                   Country = x.Country,
                   State = x.State,
                   City = x.City


               }).FirstOrDefault();

                if (model.OrganizationDetails.Country != null)
                {
                    model.OrganizationDetails.OrgCountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.OrganizationDetails.Country).CountryName == null ? string.Empty : _countryService.GetCountryById(model.OrganizationDetails.Country).CountryName);

                }
                if (model.OrganizationDetails.State != null)
                {
                    model.OrganizationDetails.OrgStateName = StringCipher.Decrypt(_stateService.GetStateById(model.OrganizationDetails.State).StateName == null ? string.Empty : _stateService.GetStateById(model.OrganizationDetails.State).StateName);
                }
                if (model.OrganizationDetails.City != null)
                {
                    model.OrganizationDetails.OrgCityName = StringCipher.Decrypt(_cityService.GetCityById(model.OrganizationDetails.City).LookUpCityName == null ? string.Empty : _cityService.GetCityById(model.OrganizationDetails.City).LookUpCityName);
                }
            }
            if (model.PharmacyState != null)
            {

                model.PharmacyStateName = StringCipher.Decrypt(_stateService.GetStateById(model.PharmacyState).StateName);
            }

            if (model.PharmacyCountryId != null)
            {

                model.PharmacyCountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.PharmacyCountryId).CountryName);
            }
            if (model.PharmacyCity != null)
            {

                model.PharmacyCityName = StringCipher.Decrypt(_cityService.GetCityById(model.PharmacyCity).LookUpCityName);
            }
            if (model.ProfilePic != null)
            {
                model.ProfilePic = UserService.Find(model.LoginId).Avatar;
            }
            if (model.GenderId != null)
            {
                model.Gender = StringCipher.Decrypt(_genderService.GetGenderById(model.GenderId).GenderName);
            }
            if (model.UsualGpCountry != null)
            {
                model.UsualGpCountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.UsualGpCountry).CountryName);
            }
            model.TreatingDoctorss = TreatingDoctor;
            model.ProfilePic = UserService.Find(model.LoginId).Avatar;
            model.clinicmodellist = doctorService.GetclinicByDoctorId(model.MemberId, Convert.ToInt32(RoleType.Patient)).Select(y => new DoctorClinicModel { ClinicId = y.ClinicId, ClinicName = StringCipher.Decrypt(_clinicService.GetClinicById(y.ClinicId).ClinicName), ClinicAddress1 = StringCipher.Decrypt(_clinicService.GetClinicById(y.ClinicId).Address1), ClinicAddress2 = StringCipher.Decrypt(_clinicService.GetClinicById(y.ClinicId).Address2), Postcode = StringCipher.Decrypt(_clinicService.GetClinicById(y.ClinicId).PostCode), PhoneNumber = StringCipher.Decrypt(_clinicService.GetClinicById(y.ClinicId).PhoneNumber), CityName = StringCipher.Decrypt(model.CityName), StateName = StringCipher.Decrypt(model.StateName), CountryName = StringCipher.Decrypt(model.CountryName), Checked = false }).ToList();
            #endregion

            #region Myhistory
            var LoginId = userService.GetPatById(Convert.ToInt32(id)).LoginId;
            var Allergieslist = _patientHistoryService.GetPatientAllergiesbyPID(LoginId).ToList();
            for (int i = 0; i < Allergieslist.Count; i++)
            {
                Allergieslist[i].AllergyName = StringCipher.Decrypt(Allergieslist[i].AllergyName);
                Allergieslist[i].WhatHappens = StringCipher.Decrypt(Allergieslist[i].WhatHappens);
            }
            var MedicationHistorylist = _patientHistoryService.GetPatientMedicationbyPID(LoginId).ToList();
            for (int i = 0; i < MedicationHistorylist.Count; i++)
            {
                MedicationHistorylist[i].Name = StringCipher.Decrypt(MedicationHistorylist[i].Name);
            }
            var PastMedicallist = _patientHistoryService.GetPatientPastMedicalbyPID(LoginId).ToList();
            for (int i = 0; i < PastMedicallist.Count; i++)
            {
                PastMedicallist[i].ConditionName = StringCipher.Decrypt(PastMedicallist[i].ConditionName);
            }
            var Smokinglist = _patientHistoryService.GetPatientSmokingbyPID(LoginId).ToList();
            for (int i = 0; i < Smokinglist.Count; i++)
            {
                Smokinglist[i].HowMany = StringCipher.Decrypt(Smokinglist[i].HowMany);
            }
            var Alcohollist = _patientHistoryService.GetPatientAlcohalbyPID(LoginId).ToList();
            for (int i = 0; i < Alcohollist.Count; i++)
            {
                Alcohollist[i].InWeek = StringCipher.Decrypt(Alcohollist[i].InWeek);
                Alcohollist[i].InDay = StringCipher.Decrypt(Alcohollist[i].InDay);
            }
            var FamilyHistorylist = _patientHistoryService.GetPatientFamilybyPID(LoginId).ToList();
            for (int i = 0; i < FamilyHistorylist.Count; i++)
            {
                FamilyHistorylist[i].ConditionName = StringCipher.Decrypt(FamilyHistorylist[i].ConditionName);
                FamilyHistorylist[i].FamilyMember = StringCipher.Decrypt(FamilyHistorylist[i].FamilyMember);
            }
            var Employmentlist = _patientHistoryService.GetPatientEmploymentbyPID(LoginId).ToList();
            for (int i = 0; i < Employmentlist.Count; i++)
            {
                Employmentlist[i].Employment = StringCipher.Decrypt(Employmentlist[i].Employment);
            }
            var OtherMedicalRecords = _patientHistoryService.GetPatientMedicalRecordbyPID(LoginId).ToList();
            for (int i = 0; i < OtherMedicalRecords.Count; i++)
            {
                OtherMedicalRecords[i].FileName = StringCipher.Decrypt(OtherMedicalRecords[i].FileName);
                OtherMedicalRecords[i].Subject = StringCipher.Decrypt(OtherMedicalRecords[i].Subject);
            }
            model.AllergiesList = Allergieslist;
            model.MedicationHistoryList = MedicationHistorylist;
            model.PastMedicallist = PastMedicallist;
            model.SmokingList = Smokinglist;
            model.AlcoholList = Alcohollist;
            model.FamilyHistoryList = FamilyHistorylist;
            model.EmploymentList = Employmentlist;
            model.OtherMedicalRecordsList = OtherMedicalRecords;

            #endregion

            return View(model);
        }
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection["serviceIds"];
            UserService.Delete(ids);
            return RedirectToAction("Findpatient");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            UserService.Activate(ids);
            return RedirectToAction("Findpatient");
        }
        public ActionResult AssignVoucher(int id)
        {
            return View();
        }
        public ActionResult GetProfessionSubTypes(int Id)
        {
            var ProfessionSubType = _professiontypeservice.GetProfessionSubTypesList().Where(t => t.Id == Id).ToList();
            return Json(ProfessionSubType, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult PayWithCreditCard(int? id, int? patientId, int? consultTimeType)
        {
            CreditCardViewModel model = new CreditCardViewModel();
            if (id.HasValue)
                model.AppointmentId = Convert.ToInt32(id);
            if (patientId.HasValue)
                model.PatientId = Convert.ToInt32(patientId);
            if (consultTimeType.HasValue)
                model.ConsultTimeId = Convert.ToInt32(consultTimeType);
            return View("Payment", model);
        }
        [HttpPost]
        public ActionResult PayWithCreditCard(CreditCardViewModel model, FormCollection collection)
        {
            var ids = collection["ddlPaidBy"];

            if (!string.IsNullOrEmpty(ids))
            {
                if (ids == "1") //Voucher
                {
                    var Decryptvocucher = StringCipher.Encrypt(model.VoucherNumber);
                    var voucher = _voucherService.GetVoucherBByNo(Decryptvocucher);
                    var patientLoginId = userService.GetPatById(model.PatientId).LoginId;
                    var AppointmentDetail = appointmentService.GetAppointmentId(model.AppointmentId).FirstOrDefault();

                    if (voucher != null)
                    {
                        var assigen = _voucherService.GetVoucherListbyUserid().Where(x => x.VoucherID == voucher.VoucherID && x.UserID == patientLoginId && x.RoleType == (int)RoleType.Patient);
                        
                        if (assigen != null && assigen.Count() > 0)
                        {
                            if (assigen.FirstOrDefault().vouchers.Status == 1)
                            {
                                ModelState.Clear();
                                ModelState.AddModelError("Voucher", "Voucher Number Already Used");
                                model = new CreditCardViewModel();
                                return View("Payment", model);
                            }

                            var cModel = new MyOnlineClinic.Entity.Payment();
                            cModel.Amount = 0;
                            cModel.Status = 1;
                            cModel.Response = model.VoucherNumber;
                            cModel.PaymentDate = DateTime.UtcNow;
                            cModel.PaymentSource = 4;
                            cModel.LoginId = userService.GetPatById(model.PatientId).LoginId;
                            cModel.AppointmentId = model.AppointmentId;
                            _servicePayment.Add(cModel);

                            voucher.Status = 1;
                            _voucherService.Update(voucher);

                            AppointmentDetail.IsPaid = true;
                            appointmentService.Update(AppointmentDetail);
                        }
                        else
                        {
                            ModelState.AddModelError("Voucher", "Invalid Voucher Number");
                            return View("Payment", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Voucher", "Invalid Voucher Number");
                        return View("Payment", model);
                    }
                }
                else if (ids == "2")//CreditCard
                {
                    decimal doctorFee = 0;
                    string currencyCode = string.Empty;
                    int currencyType = 0;
                    try
                    {
                        var doctorLoginId = appointmentService.GetAppointmentById(model.AppointmentId).DoctorLoginId;
                        var doctorId = doctorService.GetDoctorByLoginId(doctorLoginId).MemberId;
                        var doctorFeeDetails = doctorService.GetConsultFeeById(doctorId).ToList().Where(x => x.AppointmentType == (int)AppointmentType.ClinicVisit);
                        currencyType = doctorFeeDetails.FirstOrDefault().CurrencyType;

                        switch (currencyType)
                        {
                            case 1:
                                currencyCode = "AUD";
                                break;
                            case 2:
                                currencyCode = "EUR";
                                break;
                            case 3:
                                currencyCode = "INR";
                                break;
                            case 4:
                                currencyCode = "SGD";
                                break;
                            case 5:
                                currencyCode = "EUR";
                                break;
                            case 6:
                                currencyCode = "GBP";
                                break;
                            default:
                                currencyCode = "USD";
                                break;
                        }


                        if (model.ConsultTimeId == (int)ConsultTime.LongConsult)
                        {
                            doctorFee = Convert.ToDecimal(doctorFeeDetails.FirstOrDefault().LongFee);
                        }
                        else if (model.ConsultTimeId == (int)ConsultTime.StandardConsult)
                        {
                            doctorFee = Convert.ToDecimal(doctorFeeDetails.FirstOrDefault().StatdardFee);
                        }
                        else if (model.ConsultTimeId == (int)ConsultTime.VeryLongConsult)
                        {
                            doctorFee = Convert.ToDecimal(doctorFeeDetails.FirstOrDefault().VeryLongFee);
                        }
                        else
                        {
                            doctorFee = 0;
                        }

                        model.ConsultFee = doctorFee + 3.5m;
                        model.Currency = currencyCode;
                        model.PatientLoginId = userService.GetPatById(model.PatientId).LoginId;
                        model.DoctorLoginId = doctorLoginId;

                    }
                    catch { }
                   
                    var response = _paymentService.PayWithCreditCard(model);

                    if (response != null && response.Status == "approved")
                    {
                        var cModel = new MyOnlineClinic.Entity.Payment();
                        cModel.Amount = doctorFee;
                        cModel.Status = 1;
                        cModel.Response = response.ResponseString;
                        cModel.PaymentDate = DateTime.UtcNow;
                        cModel.PaymentSource = 2;
                        cModel.LoginId = userService.GetPatById(model.PatientId).LoginId;
                        cModel.AppointmentId = model.AppointmentId;
                        _servicePayment.Add(cModel);

                        var AppointmentDetail = appointmentService.GetAppointmentId(model.AppointmentId).FirstOrDefault();
                        AppointmentDetail.IsPaid = true;
                        appointmentService.Update(AppointmentDetail);
                    }
                    else
                    {

                        return View("Payment", model);
                    }
                }
            }

            return RedirectToAction("BookAppointment");
        }

        [HttpPost]
        public JsonResult doesUserNameExist(string Email)
        {
            var user = UserService.CheckDuplicateMail(StringCipher.Encrypt(Email));

            if (user)
            {
                // add the id that you want to communicate to the client
                // in case of validation success as a custom HTTP header
                Response.AddHeader("X-ID", "1");
                return Json("Email Is Already Exists", JsonRequestBehavior.AllowGet);

            }
            Response.AddHeader("X-ID", "0");
            return Json(true, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetOrganizationList(string query = "")
        {


            var org = string.IsNullOrEmpty(query) ? _organiztionService.GetOrganizationList().Where(x => x.IsActive == true).Select(x =>
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
                     Country = x.Country,
                     State = x.State,
                     City = x.City,
                     ModifiedDate = x.ModifiedDate,
                     ClinicList = _clinicService.GetClinicInOrganization(x.OrganizationId).ToList(),
                     IsActive = x.IsActive,
                     // OrganizationTypeName = _organizationTypeService.GetOrganizarionTypesById(x.OrganizationType).OrganizationTypeName,
                 }).ToList()
            : _organiztionService.GetOrganizationList().Where(x => StringCipher.Decrypt(x.OrganizationName).Contains(query) && x.IsActive == true).Select(x =>
                 new OrganizationViewModel
                 {
                     OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                     OrganizationId = x.OrganizationId,
                     OrganizationType = x.OrganizationType,
                     LoginId = x.LoginId,
                     Address1 = StringCipher.Decrypt(x.Address1),
                     FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                     PostCode = StringCipher.Decrypt(x.PostCode),
                     Address2 = StringCipher.Decrypt(x.Address2),
                     PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                     OrganizationLogo = x.OrganizationLogo,
                     Country = x.Country,
                     State = x.State,
                     City = x.City,
                     ModifiedDate = x.ModifiedDate,
                     //ClinicList = _clinicService.GetClinicInOrganization(x.OrganizationId).ToList(),
                     IsActive = x.IsActive,

                 }).ToList();

            return Json(org, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTreatingDoctors(string query = "", string OrgId = "", string DoctorsId = "")
        {
            IEnumerable<SqlParameter> parameters = null;
            var dt = Common.ExecuteStoredProcedure(new EntitiesDB(), "[dbo].[GetDoctorTreating]", parameters).Tables[0];
            var list = Common.DataTableToList<TreatingdoctorsViewModel>(dt);
            if (OrgId != "")
            {
                var SearchDoctor = list.Select(x => new TreatingdoctorsViewModel
                {
                    firstname = StringCipher.Decrypt(x.firstname),
                    surname = StringCipher.Decrypt(x.surname),

                    OrgId = x.OrgId,
                    MemberId = x.MemberId

                }).Where(x => x.OrgId == Convert.ToInt32(OrgId)).ToList();
                SearchDoctor = SearchDoctor.Where(x => !DoctorsId.Contains(Convert.ToString(x.MemberId))).ToList();
                return Json(SearchDoctor, JsonRequestBehavior.AllowGet);
            }


            return Json("", JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetClinicListByOrganization(string query = "", string OrgId = "", string ClinicIds = "")
        {

            var clinic = string.IsNullOrEmpty(query) ? _clinicService.GetClinicInByOrg(OrgId).Where(x => x.IsActive == true).Select(x =>
                new ClinicModel
                {
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    ClinicId = x.ClinicId,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    Country = x.Country,
                    State = x.State,
                    City = x.City,
                    IsActive = x.IsActive,
                    OrganizationId = x.organization.OrganizationId,
                    CountryName = _countryService.GetCountryById(x.Country).CountryName,
                    StateName = _stateService.GetStateById(x.State).StateName,
                    CityName = _cityService.GetCityById(x.City).LookUpCityName

                }).ToList()
            : _clinicService.GetClinicInByOrg(OrgId).Where(x => StringCipher.Decrypt(x.ClinicName).Contains(query)).Select(x =>
                new ClinicModel
                {
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    ClinicId = x.ClinicId,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    Country = x.Country,
                    State = x.State,
                    City = x.City,
                    IsActive = x.IsActive,
                    OrganizationId = x.organization.OrganizationId,
                    CountryName = _countryService.GetCountryById(x.Country).CountryName,
                    StateName = _stateService.GetStateById(x.State).StateName,
                    CityName = _cityService.GetCityById(x.City).LookUpCityName

                }).ToList();

            clinic = clinic.Where(x => !ClinicIds.Contains(Convert.ToString(x.ClinicId))).ToList();
            return Json(clinic, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HomeVisit(int? id, string name)
        {
            if (!id.HasValue || string.IsNullOrEmpty(name) || Convert.ToInt32(name) != (int)AppointmentType.HomeVisit)
            {
                return RedirectToAction("FindPatient");
            }
            var clinicModel = new ClinicModel();
            //clinicModel.PatientId = Convert.ToInt32(id);
            var pModel = UserService.GetPatientById(Convert.ToInt32(id)).Select(x =>
                new HomeVisitModel
                {
                    FirstName = StringCipher.Decrypt(x.FirstName),
                    Surname = StringCipher.Decrypt(x.SurName),
                    Address1 = StringCipher.Decrypt(x.Address1),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    Country = x.CountryId,
                    State = x.State,
                    City = x.City,
                    UsualGPName = StringCipher.Decrypt(x.UsualGPName),
                    UsualGPClinic = StringCipher.Decrypt(x.UsualGPClinicName),
                    UsualGPAddress = StringCipher.Decrypt(x.UsualGPAddress) + " " + StringCipher.Decrypt(x.UsualGPAddress),
                    PostCode = StringCipher.Decrypt(x.PostCode),
                }).FirstOrDefault();
            if (pModel != null)
            {
                pModel.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(pModel.Country).CountryName);
                pModel.StateList = _stateService.GetStateList().ToList();
                pModel.CityList = _cityService.GetCityList().ToList();
                var Citylist = _cityService.GetCityList();
                for (int i = 0; i < Citylist.Count; i++)
                {
                    Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
                }
                pModel.CityList = Citylist;
                var Statelist = _stateService.GetStateList();
                for (int i = 0; i < Statelist.Count; i++)
                {
                    Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
                }
                pModel.StateList = Statelist;
                pModel.PatientId = Convert.ToInt32(id);
            }
            return View(pModel);
        }

        [HttpPost]
        public ActionResult HomeVisit(FormCollection collection, HomeVisitModel model)
        {
            //HomeVisitAppointment nModel = new HomeVisitAppointment();
            Appointment cModel = new Appointment();
            string fromtime = collection.Get("FromHour") + ":" + collection.Get("FromMin") + " " + collection.Get("FromAMPM");
            DateTime dtFromdate = DateTime.ParseExact(collection.Get("AppointmentDateLocal"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime dtFromdate = DateTime.ParseExact(collection.Get("AppointmentDateLocal"), "yyyy-dd-MM", CultureInfo.InvariantCulture);
            string fromtimeL = dtFromdate.ToString("dd/MM/yyyy").ToString() + " " + fromtime;
            var appointmentDateTime = DateTime.ParseExact(fromtimeL, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var city = collection.Get("hidCityName");
            var state = collection.Get("hidStateName");
            var postcode = collection.Get("hidPostCode");
            var latitude = collection.Get("hidlatitude");
            var longitude = collection.Get("hidlongitude");
            var location = collection.Get("txtLocation");
            var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(fromtimeL));
            if (AppointmentExisit)
            {
                model.Confirm = "Exist";
                return View(model);
            }
            //cModel.DoctorLoginId = Guid.Parse("0");
            var patientResult = UserService.GetPatientById(model.PatientId);

            if (patientResult != null)
            {
                cModel.PatientLoginId = patientResult.FirstOrDefault().LoginId;
            }
            cModel.ConsultTime = 0;
            cModel.AppointmentDateLocal = appointmentDateTime;
            cModel.AppointmentStatus = (int)AppointmentStatus.New;

            //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneValue);
            //TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(appointmentDateTime));
            ////cModel.TimeZoneString = GetTimeZoneOffset(offset);
            //cModel.AppointmentDateUtc = LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString);

            cModel.AppointmentDateUtc = appointmentDateTime;
            cModel.AppointmentEndDateUtc = appointmentDateTime;
            cModel.AppointmentType = (int)AppointmentType.HomeVisit;
            cModel.IsActive = true;
            cModel.IsDeleted = false;
            cModel.IsApproved = true;
            appointmentService.Add(cModel);

            HomeVisitAppointment nModel = new HomeVisitAppointment();
            nModel.AppointmentId = cModel.AppointmentId;
            nModel.AppointmentDateLocal = appointmentDateTime;
            nModel.AppointmentDateUtc = DateTime.Now;
            nModel.PatientLoginId = patientResult.FirstOrDefault().LoginId;
            //nModel.CityId = Convert.ToInt32(model.City);
            //nModel.StateId = Convert.ToInt32(model.State);
            nModel.location = location;
            nModel.CityId = _cityService.GetCityList().Where(x => StringCipher.Decrypt(x.LookUpCityName) == city).FirstOrDefault().LookUpCityId;
            nModel.StateId = _stateService.GetStateList().Where(x => StringCipher.Decrypt(x.StateName) == state).FirstOrDefault().Id;
            nModel.PostCode = postcode;
            nModel.latitude = latitude;
            nModel.longitude = longitude;
            nModel.SeverityOfProblem = collection.Get("ddlSeverity");
            nModel.TriageCategory = collection.Get("ddlTriageCategory");
            nModel.ReasonForContact = model.Problem;
            nModel.RegularGP = collection.Get("ddlRegularGP");
            nModel.AppointmentDateUtc = DateTime.Now;
            _homeVisitAppointmentService.Add(nModel);

            return RedirectToAction("Findpatient");
        }
        public DateTime LocalToUtc(DateTime dts, string timeZoneString)
        {
            string Time = "";
            string dt = "";
            DateTime utcLocal = new DateTime();
            if (timeZoneString != "")
            {
                if (timeZoneString.Substring(0, 1) == "+")
                {
                    Time = timeZoneString.Substring(1, 5);
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime dateVal = DateTime.ParseExact(Time, "HH:mm", culture);
                    utcLocal = dts;
                    utcLocal = utcLocal.AddHours(-dateVal.Hour);
                    utcLocal = utcLocal.AddMinutes(-dateVal.Minute);

                }
                if (timeZoneString.Substring(0, 1) == "-")
                {
                    Time = timeZoneString.Substring(1, 6);
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime dateVal = DateTime.ParseExact(Time, "HH:mm", culture);
                    utcLocal = dts;
                    utcLocal = utcLocal.AddHours(dateVal.Hour);
                    utcLocal = utcLocal.AddMinutes(dateVal.Minute);
                }
            }
            return utcLocal;
        }
        [HttpGet]
        public ActionResult ClinicVisit(int? id, string name)
        {
            if (!id.HasValue || string.IsNullOrEmpty(name) || Convert.ToInt32(name) != (int)AppointmentType.ClinicVisit)
            {
                return RedirectToAction("FindPatient");
            }
            var clinicModel = new ClinicModel();
            clinicModel.ProfessionTypesList = _professiontypeservice.GetProfessionTypesList().Select(x => new ProfessionTypes { Id = x.Id, ProfessionName = StringCipher.Decrypt(x.ProfessionName) }).ToList();
            clinicModel.ProfessionSubList = new List<ProfessionSubType>();
            clinicModel.PatientId = Convert.ToInt32(id);
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;

            return View(clinicModel);
        }

        [HttpPost]
        public ActionResult ClinicVisit(ClinicModel clinicModel)
        {
            Appointment cModel = new Appointment();
            var appointmentDateTime = DateTime.ParseExact(clinicModel.AppointmentDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var AppointmentExisit = appointmentService.CheckAppointment(appointmentDateTime);

            if (AppointmentExisit)
            {
                ModelState.AddModelError("Error", "Appointment already taken for this datetime");
                clinicModel.ProfessionTypesList = _professiontypeservice.GetProfessionTypesList().Select(x => new ProfessionTypes { Id = x.Id, ProfessionName = StringCipher.Decrypt(x.ProfessionName) }).ToList(); ;
                clinicModel.ProfessionSubList = new List<ProfessionSubType>();
                ReadOnlyCollection<TimeZoneInfo> tz;
                tz = TimeZoneInfo.GetSystemTimeZones();
                var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
                ViewBag.Timezone = timeZoneList;
                return View(clinicModel);
            }

            if (clinicModel.DoctorId > 0)
            {
                var doctorResult = doctorService.GetDoctorById(clinicModel.DoctorId).FirstOrDefault();
                if (doctorResult != null)
                {
                    cModel.DoctorLoginId = doctorResult.LoginId;
                }

                var patientResult = UserService.GetPatientById(clinicModel.PatientId);

                if (patientResult != null)
                {
                    cModel.PatientLoginId = patientResult.FirstOrDefault().LoginId;
                }
                cModel.AppointmentDateLocal = appointmentDateTime;
                cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                cModel.AppointmentDateUtc = DateTime.Now;
                cModel.AppointmentType = (int)AppointmentType.ClinicVisit;
                cModel.AppointmentEndDateUtc = DateTime.UtcNow;
                appointmentService.Add(cModel);
            }
            return RedirectToAction("PayWithCreditCard", new
            {
                id = cModel.AppointmentId,
                patientId = clinicModel.PatientId,
                consultTimeType = clinicModel.ConsultTimeType
            });
        }

        [HttpGet]
        public JsonResult GetOrganizationListForAppointment(string query = "")
        {
            query = query.Trim();
            List<Organization> org = string.IsNullOrEmpty(query) ? _organiztionService.GetOrganizationList() : _organiztionService.GetOrganizationList().Where(x => x.OrganizationName.Contains(query)).ToList();
            for (int i = 0; i < org.Count; i++)
            {
                org[i].OrganizationName = StringCipher.Decrypt(org[i].OrganizationName);
            }
            return Json(org, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetClinicList(int? id, string query = "")
        {
            query = query.Trim();
            List<MyOnlineClinic.Entity.Clinic> clinicList = _clinicService.GetClinicsListbyOrgid(Convert.ToInt32(id), 0, query);
            for (int i = 0; i < clinicList.Count; i++)
            {
                clinicList[i].ClinicName = StringCipher.Decrypt(clinicList[i].ClinicName);
            }
            return Json(clinicList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDoctorList(int? clinicid)
        {
            List<Entity.Doctor> listDoctor = new List<Entity.Doctor>();
            var doctorIdList = _clinicService.GetDoctorListByClinicId(Convert.ToInt32(clinicid), (int)RoleType.Doctor);

            for (int i = 0; i < doctorIdList.Count; i++)
            {
                var doctor = doctorService.GetDoctorById(doctorIdList[i].UserId).FirstOrDefault();
                doctor.FirstName = StringCipher.Decrypt(doctor.FirstName);

                listDoctor.Add(doctor);
            }

            return Json(listDoctor, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAvabilityChart(string doctorIds, string searchDate)
        {
            var fromDate = string.Empty;
            var toDate = string.Empty;

            if (searchDate.Contains("¶"))
            {
                string[] arrDates = searchDate.Split('¶');
                fromDate = arrDates[0];
                toDate = arrDates[arrDates.Length - 1];
            }

            Dictionary<string, object> dicMain = new Dictionary<string, object>();
            List<List<Dictionary<string, object>>> listDoctor = new List<List<Dictionary<string, object>>>();
            List<Dictionary<string, object>> timeList = new List<Dictionary<string, object>>();
            var model = doctorService.GetDoctorAvabilityList(doctorIds, searchDate, fromDate, toDate);

            if (model != null)
            {
                var arrDoctorId = doctorIds.Split(',');

                for (int z = 0; z < arrDoctorId.Length; z++)
                {
                    var doctorList = model.Where(x => x.DoctorAvailabilitys.DoctorID == Convert.ToInt32(arrDoctorId[z])).ToList();

                    for (int i = 0; i < doctorList.Count; i++)
                    {
                        var startDate = doctorList[i].FromDateLocal;
                        var endDate = doctorList[i].ToDateLocal;

                        var dateDiffence = endDate.Subtract(startDate);
                        var endTime = startDate.AddHours(dateDiffence.Hours).AddMinutes(dateDiffence.Minutes);

                        List<string> time = new List<string>();
                        while (startDate <= endTime)
                        {
                            time.Add(startDate.ToString("h:mm tt"));
                            startDate = startDate.AddMinutes(15);
                        }

                        Dictionary<string, object> doctor = new Dictionary<string, object>();
                        doctor.Add("DoctorName", StringCipher.Decrypt(doctorList[i].DoctorAvailabilitys.Doctors.FirstName));
                        doctor.Add("DoctorId", doctorList[i].DoctorAvailabilitys.DoctorID);
                        doctor.Add("Date", doctorList[i].FromDateLocal.ToString("dd/MM/yyyy"));
                        doctor.Add("time", time);
                        timeList.Add(doctor);
                    }
                    listDoctor.Add(timeList);
                    timeList = new List<Dictionary<string, object>>();
                }
            }
            dicMain.Add("data", listDoctor);

            return Json(dicMain, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfessionSubCategory(int? id)
        {
            return Json(_professiontypeservice.GetProfessionTypesListById(Convert.ToInt32(id)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FutureConsult(int id, int type)
        {
            var commonUtilities = new CommonUtilities();
            var Loginid = userService.GetPatById(id).LoginId;
            var FutureConsult = appointmentService.GetAppointmentList().Where(x => x.PatientLoginId == Loginid && x.AppointmentStatus == (int)AppointmentStatus.Approved && x.AppointmentType == type).Select(x => new AppointmentViewModel
            {
                AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                AppointmentId = x.AppointmentId,
                AppointmentTypeText = (x.AppointmentType == (int)AppointmentType.VideoConsult ?
                                      commonUtilities.GetDisplayName(AppointmentType.VideoConsult) :
                                      x.AppointmentType == (int)AppointmentType.ClinicVisit ?
                                      commonUtilities.GetDisplayName(AppointmentType.ClinicVisit) :
                                      commonUtilities.GetDisplayName(AppointmentType.HomeVisit)),
                PatientName = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.SurName),
                AppointmentDateUtc = x.AppointmentDateUtc,
                AppointmentStatus = x.AppointmentStatus,
                Address1 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address1),
                Address2 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address2),
                Country = userService.Find(x.PatientLoginId).Member.CountryId,
                CountryName = StringCipher.Decrypt(_countryService.GetCountryById(userService.Find(x.PatientLoginId).Member.CountryId).CountryName),
                AppointmentStatusName = x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Pending ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Pending) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Completed ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Completed) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Approved ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Approved) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Rejected ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Rejected) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotCompleted ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotCompleted) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.New ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.New) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Assigned ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Assigned) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.InTransit ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.InTransit) :

                                                x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Seen ?
                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Seen) :

                                                commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Waiting),
                MemberId = userService.Find(x.PatientLoginId).Member.MemberId,
                PatientLoginId = x.PatientLoginId,
                AppointmentType = x.AppointmentType


            }).ToList();
            //Where(x => x.PatientLoginId == loginid && x.AppointmentStatus == (int)AppointmentStatus.Approved && x.AppointmentType == id).ToList();

            return Json(FutureConsult, JsonRequestBehavior.AllowGet);

        }

        public JsonResult PastConsult(int id, int type)
        {
            var commonUtilities = new CommonUtilities();
            var Loginid = userService.GetPatById(id).LoginId;
            var PastConsult = appointmentService.GetAppointmentList().Where(x => x.PatientLoginId == Loginid && x.AppointmentStatus != (int)AppointmentStatus.Pending && x.AppointmentStatus != (int)AppointmentStatus.Approved && x.AppointmentType == type).Select(x => new AppointmentViewModel
            {
                AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                AppointmentId = x.AppointmentId,
                AppointmentTypeText = (x.AppointmentType == (int)AppointmentType.VideoConsult ?
                                      commonUtilities.GetDisplayName(AppointmentType.VideoConsult) :
                                      x.AppointmentType == (int)AppointmentType.ClinicVisit ?
                                      commonUtilities.GetDisplayName(AppointmentType.ClinicVisit) :
                                      commonUtilities.GetDisplayName(AppointmentType.HomeVisit)),
                PatientName = StringCipher.Decrypt(userService.Find(Loginid).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(Loginid).Member.SurName),
                AppointmentDateUtc = x.AppointmentDateUtc,
                AppointmentStatus = x.AppointmentStatus,
                MemberId = userService.Find(Loginid).Member.MemberId,
                PatientLoginId = x.PatientLoginId,
                AppointmentType = x.AppointmentType,
                AppointmentStatusName = x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Pending ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Pending) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Completed ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Completed) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Approved ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Approved) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Rejected ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Rejected) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotCompleted ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotCompleted) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.New ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.New) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Assigned ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Assigned) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.InTransit ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.InTransit) :

                                               x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Seen ?
                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Seen) :

                                               commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Waiting)



            }).ToList();

            return Json(PastConsult, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CapturePayment(CaptureViewModel model)
        {
            var response = _paymentService.CapturePayment(model);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefundPayment(RefundViewModel model)
        {
            var response = _paymentService.RefundPayment(model);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        //get all History
        public void GetallHistory(PatientHistoryViewModel model)
        {
            var LoginId = base.loginUserModel.LoginId;
            var Allergieslist = _patientHistoryService.GetPatientAllergiesbyPID(LoginId).ToList();
            for (int i = 0; i < Allergieslist.Count; i++)
            {
                Allergieslist[i].AllergyName = StringCipher.Decrypt(Allergieslist[i].AllergyName);
                Allergieslist[i].WhatHappens = StringCipher.Decrypt(Allergieslist[i].WhatHappens);
            }
            var MedicationHistorylist = _patientHistoryService.GetPatientMedicationbyPID(LoginId).ToList();
            for (int i = 0; i < MedicationHistorylist.Count; i++)
            {
                MedicationHistorylist[i].Name = StringCipher.Decrypt(MedicationHistorylist[i].Name);
            }
            var PastMedicallist = _patientHistoryService.GetPatientPastMedicalbyPID(LoginId).ToList();
            for (int i = 0; i < PastMedicallist.Count; i++)
            {
                PastMedicallist[i].ConditionName = StringCipher.Decrypt(PastMedicallist[i].ConditionName);
            }
            var Smokinglist = _patientHistoryService.GetPatientSmokingbyPID(LoginId).ToList();
            for (int i = 0; i < Smokinglist.Count; i++)
            {
                Smokinglist[i].HowMany = StringCipher.Decrypt(Smokinglist[i].HowMany);
            }
            var Alcohollist = _patientHistoryService.GetPatientAlcohalbyPID(LoginId).ToList();
            for (int i = 0; i < Alcohollist.Count; i++)
            {
                Alcohollist[i].InWeek = StringCipher.Decrypt(Alcohollist[i].InWeek);
                Alcohollist[i].InDay = StringCipher.Decrypt(Alcohollist[i].InDay);
            }
            var FamilyHistorylist = _patientHistoryService.GetPatientFamilybyPID(LoginId).ToList();
            for (int i = 0; i < FamilyHistorylist.Count; i++)
            {
                FamilyHistorylist[i].ConditionName = StringCipher.Decrypt(FamilyHistorylist[i].ConditionName);
                FamilyHistorylist[i].FamilyMember = StringCipher.Decrypt(FamilyHistorylist[i].FamilyMember);
            }
            var Employmentlist = _patientHistoryService.GetPatientEmploymentbyPID(LoginId).ToList();
            for (int i = 0; i < Employmentlist.Count; i++)
            {
                Employmentlist[i].Employment = StringCipher.Decrypt(Employmentlist[i].Employment);
            }
            var OtherMedicalRecords = _patientHistoryService.GetPatientMedicalRecordbyPID(LoginId).ToList();
            for (int i = 0; i < OtherMedicalRecords.Count; i++)
            {
                OtherMedicalRecords[i].FileName = StringCipher.Decrypt(OtherMedicalRecords[i].FileName);
                OtherMedicalRecords[i].Subject = StringCipher.Decrypt(OtherMedicalRecords[i].Subject);
            }
            model.AllergiesList = Allergieslist;
            model.MedicationHistoryList = MedicationHistorylist;
            model.PastMedicalList = PastMedicallist;
            model.SmokingList = Smokinglist;
            model.AlcoholList = Alcohollist;
            model.FamilyHistoryList = FamilyHistorylist;
            model.EmploymentList = Employmentlist;
            model.OtherMedicalRecordsList = OtherMedicalRecords;
        }
        #region patient Allergies Section
        public ActionResult AddAllergies(PatientHistoryViewModel model)
        {
            PatientAllergies _PatientAllergies = null;
            if (model != null)
            {
                model.MemberId = 1;
                model.LoginId = base.loginUserModel.LoginId;
                _PatientAllergies = model.GetPatientAlleries(model);
                if (model.PatientHistoryId != 0)
                {
                    _patientHistoryService.UpdatePatientAllergies(_PatientAllergies);
                }
                else
                {
                    _patientHistoryService.AddPatientAllergies(_PatientAllergies);
                }
                _PatientAllergies.AllergyName = StringCipher.Decrypt(_PatientAllergies.AllergyName);
                _PatientAllergies.WhatHappens = StringCipher.Decrypt(_PatientAllergies.WhatHappens);
            }
            return Json(_PatientAllergies, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAllergies(int Id)
        {
            _patientHistoryService.DeletePatientAllergies(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region patient Medication Section
        public ActionResult AddMedicationHistory(PatientHistoryViewModel model)
        {
            PatientMedication _PatientMedication = null;
            model.MemberId = 1;
            model.LoginId = base.loginUserModel.LoginId;
            _PatientMedication = model.GetPatientMedication(model);
            if (model != null)
            {
                if (model.PatientHistoryId != 0)
                {
                    _patientHistoryService.UpdatePatientMedication(_PatientMedication);
                }
                else
                {
                    _patientHistoryService.AddPatientMedication(_PatientMedication);
                }
                _PatientMedication.Name = StringCipher.Decrypt(_PatientMedication.Name);
                _PatientMedication.Strength = StringCipher.Decrypt(_PatientMedication.Strength);
                _PatientMedication.Dose = StringCipher.Decrypt(_PatientMedication.Dose);
                _PatientMedication.YearStaeted = StringCipher.Decrypt(_PatientMedication.YearStaeted);
            }
            return Json(_PatientMedication, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMedicationHistory(int Id)
        {
            _patientHistoryService.DeletePatientMedication(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region patient Past Medical Section
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddPastMedical(PatientHistoryViewModel model)
        {
            PatientPastMedical _PatientPastMedical = null;
            model.MemberId = 1;
            model.LoginId = base.loginUserModel.LoginId;
            _PatientPastMedical = model.GetPatientPastMedical(model);
            if (model != null)
            {
                if (model.PatientHistoryId != 0)
                {
                    _patientHistoryService.UpdatePatientPastMedical(_PatientPastMedical);
                }
                else
                {
                    _patientHistoryService.AddPatientPastMedical(_PatientPastMedical);
                }
                _PatientPastMedical.ConditionName = StringCipher.Decrypt(_PatientPastMedical.ConditionName);
                _PatientPastMedical.DiagnosedYear = StringCipher.Decrypt(_PatientPastMedical.DiagnosedYear);
            }
            return Json(_PatientPastMedical, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletePastMedical(int Id)
        {
            _patientHistoryService.DeletePatientPastMedical(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region patient Smoking Section
        public ActionResult AddSmoking(PatientHistoryViewModel model)
        {
            PatientSmoking _PatientSmoking = null;
            if (model != null)
            {
                model.MemberId = 1;
                model.LoginId = base.loginUserModel.LoginId;
                _PatientSmoking = model.GetPatientSmoking(model);
                if (model.PatientHistoryId != 0)
                {
                    _patientHistoryService.UpdatePatientSmoking(_PatientSmoking);
                }
                else
                {
                    _patientHistoryService.AddPatientSmoking(_PatientSmoking);
                }
                _PatientSmoking.HowMany = StringCipher.Decrypt(_PatientSmoking.HowMany);
                _PatientSmoking.YearStarted = StringCipher.Decrypt(_PatientSmoking.YearStarted);
            }
            return Json(_PatientSmoking, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteSmoking(int Id)
        {
            _patientHistoryService.DeletePatientSmoking(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region patient Alcohol Section
        public ActionResult AddAlcohol(PatientHistoryViewModel model)
        {
            PatientAlcohal _patientAlcohal = null;
            if (model != null)
            {
                model.MemberId = 1;
                model.LoginId = base.loginUserModel.LoginId;
                _patientAlcohal = model.GetPatientAlcohal(model);
                if (model.PatientHistoryId != 0)
                {
                    _patientHistoryService.UpdatePatientAlcohal(_patientAlcohal);
                }
                else
                {
                    _patientHistoryService.AddPatientAlcohal(_patientAlcohal);
                }
                _patientAlcohal.InDay = StringCipher.Decrypt(_patientAlcohal.InDay);
                _patientAlcohal.InWeek = StringCipher.Decrypt(_patientAlcohal.InWeek);
            }
            return Json(_patientAlcohal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAlcohal(int Id)
        {
            _patientHistoryService.DeletePatientAlcohal(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region patient Family History Section
        public ActionResult AddFamilyHistory(PatientHistoryViewModel model)
        {
            PatientFamily _PatientFamily = null;
            if (model != null)
            {
                model.MemberId = 1;
                model.LoginId = base.loginUserModel.LoginId;
                _PatientFamily = model.GetPatientFamilyHistory(model);
                if (model.PatientHistoryId != 0)
                {
                    _patientHistoryService.UpdatePatientFamily(_PatientFamily);
                }
                else
                {
                    _patientHistoryService.AddPatientFamily(_PatientFamily);
                }
                _PatientFamily.ConditionName = StringCipher.Decrypt(_PatientFamily.ConditionName);
                _PatientFamily.FamilyMember = StringCipher.Decrypt(_PatientFamily.FamilyMember);
            }
            return Json(_PatientFamily, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteFamilyHistory(int Id)
        {
            _patientHistoryService.DeletePatientFamily(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region patient Employment History Section
        public ActionResult AddEmployment(PatientHistoryViewModel model)
        {
            PatientEmployment _PatientEmployment = null;
            if (model != null)
            {
                model.MemberId = 1;
                model.LoginId = base.loginUserModel.LoginId;
                _PatientEmployment = model.GetPatientEmployment(model);
                if (model.PatientHistoryId != 0)
                {
                    _patientHistoryService.UpdatePatientEmployment(_PatientEmployment);
                }
                else
                {
                    _patientHistoryService.AddPatientEmployment(_PatientEmployment);
                }
                _PatientEmployment.Employment = StringCipher.Decrypt(_PatientEmployment.Employment);
                _PatientEmployment.EmploymentDate = Convert.ToDateTime(_PatientEmployment.EmploymentDate);
            }
            return Json(_PatientEmployment, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmployment(int Id)
        {
            _patientHistoryService.DeletePatientEmployment(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region patient Other Medical Records Section
        public ActionResult AddMedicalRecord(PatientHistoryViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object
                    HttpFileCollectionBase files = Request.Files;
                    var medicalRecord = new PatientMedicalRecord();
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var convertedFileName = GetFileName(model.OtherMedicalRecordsFilename);
                        var newConvertedFielName = convertedFileName;
                        convertedFileName = Path.Combine(Server.MapPath("~/UploadedFiles/OtherNedicalRecords/"), convertedFileName);
                        file.SaveAs(convertedFileName);

                        medicalRecord.Subject = model.OtherMedicalRecordsSubject;
                        medicalRecord.FileName = model.OtherMedicalRecordsFilename;
                        medicalRecord.LoginId = model.LoginId;
                        //ecg.EcgFielsConvertedName = newConvertedFielName;
                        _patientHistoryService.AddPatientMedicalRecord(medicalRecord);
                        //jsonObject.Add("success", true);
                        //jsonObject.Add("message", );
                    }
                    return Json(medicalRecord, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
            //UploadImage(model);
            ////PatientMedicalRecord _PatientMedicalRecord = null;
            //PatientMedicalRecord patientMedicalRecord = new PatientMedicalRecord();
            //if (model != null)
            //{
            //    model.MemberId = 1;
            //    //_PatientMedicalRecord = model.GetPatientMedicalRecord(model);
            //    if (model.PatientHistoryId != 0)
            //    {
            //        patientMedicalRecord.MemberId = model.MemberId;
            //        patientMedicalRecord.Subject = model.OtherMedicalRecordsSubject;
            //        patientMedicalRecord.FileName = GetFileName(model.OtherMedicalRecordsFilename);
            //        _patientHistoryService.UpdatePatientMedicalRecord(patientMedicalRecord);
            //    }
            //    else
            //    {
            //        patientMedicalRecord.MemberId = model.MemberId;
            //        patientMedicalRecord.Subject = model.OtherMedicalRecordsSubject;
            //        patientMedicalRecord.FileName = GetFileName(model.OtherMedicalRecordsFilename);
            //        //_patientHistoryService.UpdatePatientMedicalRecord(patientMedicalRecord);
            //        _patientHistoryService.AddPatientMedicalRecord(patientMedicalRecord);
            //    }
            //}
            //return Json(patientMedicalRecord, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMedicalRecord(int Id)
        {
            _patientHistoryService.DeletePatientMedicalRecord(Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public static string GetFileName(string fileName)
        {
            return Guid.NewGuid().ToString().Replace('-', '_') + Path.GetExtension(fileName).ToString();
        }
        public PatientHistoryViewModel UploadImage(PatientHistoryViewModel model)
        {
            if (model.OtherMedicalRecordsFilename != null)
            {
                var fileName = Path.GetFileName(model.OtherMedicalRecordsFilename);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    if (!string.IsNullOrEmpty(model.OtherMedicalRecordsFilename))
                    {
                        //Delete exising File
                        var file = Server.MapPath(model.OtherMedicalRecordsFilename);
                        imageHelper.Delete(file);
                    }
                    string fName = Convert.ToString(DateTime.Now.ToString("ddMMyyyy-HHmmss-fff")) + extension;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/OtherNedicalRecords/"), fName);

                    model.OtherMedicalRecordsFilename = "/UploadedFiles/OtherNedicalRecords/" + fName;
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
                if (model.OtherMedicalRecordsFilename != null)
                {
                    model.OtherMedicalRecordsFilename = model.OtherMedicalRecordsFilename;
                }

            }
            return model;
        }
        #endregion
    }
}
