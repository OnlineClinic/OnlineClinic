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

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    public class BookAppointmentController : BaseController
    {
        private readonly IAppointmentService appointmentService = null;
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
        private readonly IUregisteredOrganizationService _unregisteredOrganizationService;
        IProfessionTypeService _professiontypeservice;
        IPaymentHelper _paymentService;
        private readonly IPatientService _patientServices;
        private readonly IUnregisteredClinicService _unregisteredClinicService;
        private readonly IPaymentService _servicePayment;
        private readonly IVoucherService _voucherService;

        public BookAppointmentController(IAppointmentService appointmentService, IDoctorService doctorService, IUserService userService, IOrganizationService organiztionService, IClinicService clinicService,
        ICountryService countryService, ICityService cityService, ILookUpModuleService moduleService,
          IPermissionsService permissionService, ILookUpUserRolesService roleService,
          IPermissionInModuleService permissioninmoduleService, IAdminUserService adminuserService, ITimeZoneService timezoneService, ITitleService TitleServices, IGenderService genderService, IStateService stateService, ILoginHelper loginHelper, IUregisteredOrganizationService unregisteredOrganizationService

          , IProfessionTypeService professiontypeservice, IPaymentHelper paymentService, IPatientService patientServices, IPaymentService servicePayment, IUnregisteredClinicService unregisteredClinicService, IVoucherService voucherService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
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
            _voucherService = voucherService;
        }


        public ActionResult Index(int id)
        {
            var Loginid = userService.GetPatientById(id).FirstOrDefault().LoginId;

            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            SearchViewModel model = new SearchViewModel();
            var Cmodel = UserService.GetLoginPatientById(Loginid).Select(x => new SearchViewModel
            {
                PatientId = x.LoginId,
                org = x.OrgId,
                Timezone = x.TimeZoneString,
                MemberId = x.MemberId,
                PatientLoginId = x.LoginId

            }).FirstOrDefault();
            var Omodel = _organiztionService.GetOrganizationById(Cmodel.org);
            var ClinicDecrypt = _clinicService.GetClinicsListbyOrgid(Cmodel.org, Omodel.OrganizationType).ToList();
            for (int i = 0; i < ClinicDecrypt.Count; i++)
            {
                ClinicDecrypt[i].ClinicName = StringCipher.Decrypt(ClinicDecrypt[i].ClinicName);
            }
            model.ClinicList = ClinicDecrypt;
            model.PatientLoginId = Cmodel.PatientLoginId;
            model.MemberId = Cmodel.MemberId;
            BindType(model);
            return View(model);
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
            model.CountryList = _countryService.GetCountryList().Select(x => new LookUpCountry { CountryId = x.CountryId, CountryName = StringCipher.Decrypt(x.CountryName) }).ToList();
            var CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            for (int i = 0; i < CityList.Count; i++)
            {
                CityList[i].LookUpCityName = StringCipher.Decrypt(CityList[i].LookUpCityName);
            }
            model.CityList = CityList;

            var Statelist = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.StateList = Statelist;
            model.ProfessionTypes = _professiontypeservice.GetProfessionTypesList().Select(x => new ProfessionTypes { Id = x.Id, ProfessionName = StringCipher.Decrypt(x.ProfessionName) }).ToList();
            model.ProfessionSubList = _professiontypeservice.GetProfessionSubTypesList();

            return model;

        }
        public ActionResult GetProfessionSubTypes(int Id)
        {
            var ProfessionSubType = _professiontypeservice.GetProfessionSubTypesList().Where(t => t.Id == Id).ToList();
            return Json(ProfessionSubType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(SearchViewModel model)
        {
            var ConvertedAvailableDateTime = string.Empty;
            if (model.AppointmentDateLocalText != null)
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneValue);
                var datetime = model.AppointmentDateLocalText + " " + model.FromTime + ":" + model.ToTime + " " + model.FromAMPM;
                TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(datetime));
                var TimeZoneString = GetTimeZoneOffset(offset);
                ConvertedAvailableDateTime = (LocalToUtc(Convert.ToDateTime(datetime.ToString()), TimeZoneString)).ToString("yyyy-MM-dd HH:mm:ss:000");
            }
            var Clinicid = 0;
            var professiontype = 0;
            if (model.AppointmentSelectionType == 4)
            {
                Clinicid = Convert.ToInt32(model.Clinicid);
            }
            else
            {
                professiontype = model.AppointmentSelectionType;
            }
            SqlParameter[] parameters = new SqlParameter[9]{              
            new SqlParameter("@Name",string.IsNullOrEmpty(model.DoctorFullName)?"":StringCipher.Encrypt(model.DoctorFullName)), 
            new SqlParameter("@DoctorProfessionId",professiontype), 
            new SqlParameter("@countryId", model.countryid==null?0:model.countryid),
            new SqlParameter("@stateId", model.stateid==null?0:model.stateid),                    
            new SqlParameter("@postalCode",string.IsNullOrEmpty(model.postcode)?"" :StringCipher.Encrypt(model.postcode)),
            new SqlParameter("@availableDateTime", ConvertedAvailableDateTime),
            new SqlParameter("@Language", string.IsNullOrEmpty(model.language)?"" : StringCipher.Decrypt(model.language)),                    
            new SqlParameter("@ConsultTime", model.ConsultType),                     
            new SqlParameter("@ClinicID",Clinicid)
                    
                };



            var dt1 = Common.ExecuteStoredProcedure(new EntitiesDB(), "[dbo].[Search]", parameters);
            if (dt1.Tables.Count > 0)
            {
                if (Clinicid > 0)
                {
                    // var list2 = Common.DataTableToList<DoctorClinicResponseModel>(dt1.Tables[1]);
                }
                var list = Common.DataTableToList<DoctorSearchResponseModel>(dt1.Tables[0]);

                list = list.Select(x => new DoctorSearchResponseModel
                {
                    Avatar = x.Avatar == null ? " " : x.Avatar,
                    DoctorFullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    AreaofIntrest = StringCipher.Decrypt(x.AreaofIntrest),
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                    DoctorProfile = StringCipher.Decrypt(x.DoctorProfile),
                    LanguageSpoken = StringCipher.Decrypt(x.LanguageSpoken),
                    DoctorLoginId = x.LoginId,
                    TitleName = x.TitleName,
                    qualification = StringCipher.Decrypt(x.qualification),
                    ProfessionName = x.ProfessionName,
                    CityName = StringCipher.Decrypt(x.CityName),
                    StateName = x.StateName,
                    CountryName = x.CountryName,
                    DoctorUserId = x.DoctorUserId,


                    // Clinics = list2.Select(y => new DoctorClinicResponseModel { Id = y.Id, Name = StringCipher.Decrypt(y.Name) }).ToList()
                }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult BookAppointment()
        {
            SearchViewModel model = new SearchViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Book(SearchViewModel model, Guid PatientIdLogin)
        {
            var PatientId = PatientIdLogin;
            var patient = userService.GetPatientByLoginId(PatientId);
            PaymentViewModel returnModel = new PaymentViewModel();
            var datetime = string.Empty;
            if (model.hdnDocIdwhenAval > 0)
            {
                var doctorResult = doctorService.GetDoctorById(Convert.ToInt32(model.hdnDocIdwhenAval)).FirstOrDefault();
                Appointment cModel = new Appointment();
                datetime = model.hdnAppointmentDateForAval + " " + model.hdnAppointmentTimeForAval;
                var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(datetime));
                if (AppointmentExisit)
                {
                    returnModel.AlreadyExists = true;
                    return Json(returnModel, JsonRequestBehavior.AllowGet);
                }
                else if (datetime != null)
                {
                    cModel.PatientLoginId = PatientId;
                    cModel.DoctorLoginId = doctorResult.LoginId;
                    cModel.AppointmentDateLocal = Convert.ToDateTime(datetime);
                    cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneValue);
                    TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(datetime));
                    cModel.TimeZoneString = GetTimeZoneOffset(offset);
                    cModel.AppointmentDateUtc = LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString);
                    cModel.AppointmentEndDateUtc = (LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString)).AddMinutes(model.ConsultType);
                    cModel.AppointmentType = (int)AppointmentType.VideoConsult;

                    ViewBag.ConsultType = model.ConsultType;
                    appointmentService.Add(cModel);
                    model.Confirm = "Book";
                    returnModel.ConsultTime = model.ConsultType;
                    returnModel.DoctorId = cModel.DoctorLoginId;
                    returnModel.patientid = cModel.PatientLoginId;
                    returnModel.AppointmentId = cModel.AppointmentId;
                    returnModel.AppointmentType = cModel.AppointmentType;
                    returnModel.PatientMemberId = patient.MemberId;
                    
                    returnModel.Booked = true;

                    return Json(returnModel, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var doctorResult = doctorService.GetDoctorById(Convert.ToInt32(model.HdnDoctorId)).FirstOrDefault();
                Appointment cModel = new Appointment();
                datetime = model.AppointmentDateLocalText + " " + model.FromTime + ":" + model.ToTime + " " + model.FromAMPM;
                var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(datetime), doctorResult.LoginId);
                if (AppointmentExisit)
                {
                    returnModel.AlreadyExists = true;
                    return Json(returnModel, JsonRequestBehavior.AllowGet);
                }
                else if (datetime != null)
                {
                    cModel.PatientLoginId = PatientId;
                    cModel.DoctorLoginId = doctorResult.LoginId;
                    cModel.AppointmentDateLocal = Convert.ToDateTime(datetime);
                    cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneValue);
                    TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(datetime));
                    cModel.TimeZoneString = GetTimeZoneOffset(offset);
                    cModel.AppointmentDateUtc = LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString);
                    cModel.AppointmentEndDateUtc = (LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString)).AddMinutes(model.ConsultType);
                    cModel.ConsultTime = model.ConsultType;
                    cModel.AppointmentType = (int)AppointmentType.VideoConsult;
                    appointmentService.Add(cModel);
                    model.Confirm = "Book";


                    returnModel.ConsultTime = model.ConsultType;
                    returnModel.DoctorId = cModel.DoctorLoginId;
                    returnModel.patientid = cModel.PatientLoginId;
                    returnModel.AppointmentId = cModel.AppointmentId;
                    returnModel.AppointmentType = cModel.AppointmentType;
                    returnModel.Booked = true;

                    //return RedirectToAction("PayWithCreditCard", new
                    //{
                    //    AppointmentId = cModel.AppointmentId,
                    //    patientid = cModel.PatientLoginId,
                    //    DoctorId = cModel.DoctorLoginId,
                    //    ConsultTime = model.ConsultType,
                    //    AppointmentType = cModel.AppointmentType
                    //});
                    return Json(returnModel, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckAvailability(int Id)
        {
            var test = doctorService.GetDoctorAvabilityById(Id);

            Dictionary<string, object> dicMain = new Dictionary<string, object>();
            List<List<Dictionary<string, object>>> listDoctor = new List<List<Dictionary<string, object>>>();
            List<Dictionary<string, object>> timeList = new List<Dictionary<string, object>>();

            if (test != null)
            {
                var doctorList = test.Where(x => x.DoctorAvailabilitys.DoctorID == Convert.ToInt32(Id) && x.DoctorAvailabilitys.AvailabilityType == (int)AppointmentType.VideoConsult).ToList();

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
                    doctor.Add("DoctorName", doctorList[i].DoctorAvailabilitys.Doctors.FirstName);
                    doctor.Add("DoctorId", doctorList[i].DoctorAvailabilitys.DoctorID);
                    doctor.Add("Date", doctorList[i].FromDateLocal.ToString("dd-MM-yyyy"));
                    doctor.Add("ToDate", doctorList[i].ToDateLocal.ToString("dd/MM/yyyy"));
                    doctor.Add("time", time);
                    timeList.Add(doctor);
                }
                listDoctor.Add(timeList);
                timeList = new List<Dictionary<string, object>>();
            }

            dicMain.Add("data", listDoctor);

            return Json(dicMain, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PayByVoucehr(PaymentViewModel model)
        {

            var Decryptvocucher = StringCipher.Encrypt(model.VoucherNo);
            var AppointmentDetail = appointmentService.GetAppointmentId(model.AppointmentId).FirstOrDefault();
            var vocuehr = _voucherService.GetVoucherBByNo(Decryptvocucher);
            if (vocuehr != null)
            {
                var assigen = _voucherService.GetVoucherListbyUserid().Where(x => x.VoucherID == vocuehr.VoucherID && x.UserID == model.patientid && x.RoleType == (int)RoleType.Patient).Count();
                if (assigen > 0)
                {
                    var PModel = new MyOnlineClinic.Entity.Payment();
                    PModel.Amount = 0;
                    PModel.Status = 1;
                    PModel.Response = model.VoucherNo;
                    PModel.PaymentDate = DateTime.UtcNow;
                    PModel.PaymentSource = 4;
                    PModel.LoginId = model.patientid;
                    PModel.AppointmentId = model.AppointmentId;
                    _servicePayment.Add(PModel);

                    AppointmentDetail.IsPaid = true;
                    appointmentService.Update(AppointmentDetail);
                    model.Booked = true;
                    return Json(model, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    model.Booked = false;
                    return Json(model, JsonRequestBehavior.AllowGet);
                }

            }
            model.Booked = false;
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult PayWithCreditCard(CreditCardViewModel model)
        {
            int currencyType = 0;
            var MemberId = doctorService.GetDoctorByLoginId(model.DoctorLoginId).MemberId;
            var DoctorFee = doctorService.GetDoctorConsultFee(MemberId).Where(x => x.AppointmentType == (int)AppointmentType.VideoConsult).FirstOrDefault();
            currencyType = DoctorFee.CurrencyType;

            if (DoctorFee != null)
            {
                if (Convert.ToInt16(model.ConsultType) == (int)ConsultTime.StandardConsult)
                {
                    model.ConsultFee = DoctorFee.StatdardFee ?? 0;
                }
                else if (Convert.ToInt16(model.ConsultType) == (int)ConsultTime.LongConsult)
                {
                    model.ConsultFee = DoctorFee.LongFee ?? 0;
                }
                else if (Convert.ToInt16(model.ConsultType) == (int)ConsultTime.VeryLongConsult)
                {
                    model.ConsultFee = DoctorFee.VeryLongFee ?? 0;
                }
                model.ConsultFee = model.ConsultFee + 3.5m;
            }
          
            string currencyCode = string.Empty;
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

            //To-Do make currency dynamic according to doctor currency
            model.Currency = currencyCode;
         
            var response = _paymentService.PayWithCreditCard(model);

            if (response != null && response.Status == "approved")
            {
                var cModel = new MyOnlineClinic.Entity.Payment();
                cModel.Amount = model.ConsultFee;
                cModel.Status = 1;
                cModel.Response = response.ResponseString;
                cModel.PaymentDate = DateTime.UtcNow;
                cModel.PaymentSource = 2;
                cModel.LoginId = model.PatientLoginId;
                cModel.AppointmentId = model.AppointmentId;
                _servicePayment.Add(cModel);

                var AppointmentDetail = appointmentService.GetAppointmentId(model.AppointmentId).FirstOrDefault();
                AppointmentDetail.IsPaid = true;
                appointmentService.Update(AppointmentDetail);

                model.Booked = true;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model.Booked = false;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public string GetTimeZoneOffset(TimeSpan offset)
        {
            string Time = "";
            string dt = "";
            if (offset != null)
            {
                dt = offset.ToString();
                if (dt.Substring(0, 1) == "-")
                {
                    Time = dt.Substring(0, 6);
                }
                else
                {
                    Time = dt.Substring(0, 5);
                    Time = "+" + Time;
                }
            }
            return Time;
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
                    Time = timeZoneString.Substring(1, 5);
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime dateVal = DateTime.ParseExact(Time, "HH:mm", culture);
                    utcLocal = dts;
                    utcLocal = utcLocal.AddHours(dateVal.Hour);
                    utcLocal = utcLocal.AddMinutes(dateVal.Minute);
                }
            }
            return utcLocal;
        }
    }
}