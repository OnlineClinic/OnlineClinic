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
using System.Globalization;
using System.Web.Routing;

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class DoctorAvailabilityController : BaseController
    {
        private readonly IDoctorService _doctorService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly IClinicService _clinicservice;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly CommonUtilities _commonUtilities;

        public DoctorAvailabilityController(IDoctorService doctorService, IUserService userService, IOrganizationService organizationService,
            IClinicService clinicservice, ICountryService countryService, IStateService stateService, ICityService cityService,
            CommonUtilities commonUtilities)
        {
            _doctorService = doctorService;
            _userService = userService;
            _organizationService = organizationService;
            _clinicservice = clinicservice;
            _countryService = countryService;
            _cityService = cityService;
            _stateService = stateService;
            _commonUtilities = commonUtilities;
        }
        //
        // GET: /Admin/DoctorAvailability/
        public ActionResult Index(int? id)
        {
            ViewBag.DoctorId = Convert.ToInt32(id);
            var doctorAvailabilityModel = new DoctorAvailabilityModel();
            var doctordetail = _doctorService.GetDoctorById(Convert.ToInt32(id)).FirstOrDefault();
            if (doctordetail != null)
            {
                doctorAvailabilityModel.FullName = StringCipher.Decrypt(doctordetail.FirstName) + " " + StringCipher.Decrypt(doctordetail.SurName);
                doctorAvailabilityModel.ProfilePic = _userService.Find(doctordetail.LoginId).Avatar;
                var orgname = StringCipher.Decrypt(_organizationService.GetOrganizationById(doctordetail.OrgId).OrganizationName);
                //var listclinic = _clinicservice.GetClinicInOrganization(doctordetail.OrgId).ToList();
                //doctorAvailabilityModel.OrgName = orgname + "/" + _clinicservice.GetClinicById(doctordetail.OrgId).ClinicName;
                doctorAvailabilityModel.OrgName = orgname;
            }
            var doctorAvailabilityList1 = _doctorService.GetDoctorAvailabilityList().Where(x => x.AvailabilityType == (Int32)AppointmentType.VideoConsult && x.DoctorID == Convert.ToInt32(id) && x.IsDeleted == false).Select(x => new DoctorAvailabilityModel
            {
                //AvailabilityTypeName = x.AvailabilityType == (Int32)AppointmentType.VideoConsult ? AppointmentType.VideoConsult.ToString() : AppointmentType.ClinicVisit.ToString(),
                //AvailabilityTypeName = x.AvailabilityType == (Int32)AppointmentType.VideoConsult ? "VideoConsult" : x.AvailabilityType == (Int32)AppointmentType.ClinicVisit ? "ClinicVisit" : "Home Visit",
                FromDateLocal = x.FromDateLocal,
                ToDateLocal = x.ToDateLocal,
                FullName = StringCipher.Decrypt(doctorAvailabilityModel.FullName),
                ProfilePic = doctorAvailabilityModel.ProfilePic,
                OrgName = StringCipher.Decrypt(doctorAvailabilityModel.OrgName),
                TimeZone = StringCipher.Decrypt(x.TimeZoneString),
                AvailabilityId = x.AvailabilityId
            }).ToList();
            var doctorAvailabilityList2 = _doctorService.GetDoctorAvailabilityList().Where(x => x.AvailabilityType == (Int32)AppointmentType.ClinicVisit && x.DoctorID == Convert.ToInt32(id) && x.IsDeleted == false).Select(x => new DoctorAvailabilityModel
            {
                FromDateLocal = x.FromDateLocal,
                ToDateLocal = x.ToDateLocal,
                FullName = StringCipher.Decrypt(doctorAvailabilityModel.FullName),
                ProfilePic = doctorAvailabilityModel.ProfilePic,
                OrgName = StringCipher.Decrypt(doctorAvailabilityModel.OrgName),
                TimeZone = x.TimeZoneString,
                AvailabilityId = x.AvailabilityId
            }).ToList();
            var doctorAvailabilityList3 = _doctorService.GetDoctorAvailabilityList().Where(x => x.AvailabilityType == (Int32)AppointmentType.HomeVisit && x.DoctorID == Convert.ToInt32(id) && x.IsDeleted == false).Select(x => new DoctorAvailabilityModel
            {
                FromDateLocal = x.FromDateLocal,
                ToDateLocal = x.ToDateLocal,
                FullName = StringCipher.Decrypt(doctorAvailabilityModel.FullName),
                ProfilePic = doctorAvailabilityModel.ProfilePic,
                OrgName = StringCipher.Decrypt(doctorAvailabilityModel.OrgName),
                TimeZone = x.TimeZoneString,
                AvailabilityId = x.AvailabilityId
            }).ToList();
            doctorAvailabilityModel.Type1 = doctorAvailabilityList1;
            doctorAvailabilityModel.Type2 = doctorAvailabilityList2;
            doctorAvailabilityModel.Type3 = doctorAvailabilityList3;
            return View(doctorAvailabilityModel);
        }


        //
        // GET: /Admin/DoctorAvailability/Create
        public ActionResult AddAvailability(int? id)
        {
            var doctorAvailabilityModel = new DoctorAvailabilityModel();
            //doctorAvailabilityModel.FromDateLocal = DateTime.Now;
            //doctorAvailabilityModel.ToDateLocal = DateTime.Now.AddDays(3);
            var doctordetail = _doctorService.GetDoctorById(Convert.ToInt32(id)).FirstOrDefault();
            if (doctordetail != null)
            {

                doctorAvailabilityModel.FullName = StringCipher.Decrypt(doctordetail.FirstName) + " " + StringCipher.Decrypt(doctordetail.SurName);
                doctorAvailabilityModel.ProfilePic = _userService.Find(doctordetail.LoginId).Avatar;
                var orgname = StringCipher.Decrypt(_organizationService.GetOrganizationById(doctordetail.OrgId).OrganizationName);
                //var listclinic = _clinicservice.GetClinicInOrganization(doctordetail.OrgId).ToList();
                //doctorAvailabilityModel.OrgName = orgname + "/" + _clinicservice.GetClinicById(doctordetail.OrgId).ClinicName;
                doctorAvailabilityModel.OrgName = orgname;
            }
            //var model = _clinicservice.GetClinicsListbyDoctorid(Convert.ToInt32(id));
            doctorAvailabilityModel.DoctorID = Convert.ToInt32(id);

            binddropdwon(doctorAvailabilityModel);
            return View(doctorAvailabilityModel);
        }
        public DoctorAvailabilityModel binddropdwon(DoctorAvailabilityModel model)
        {

            var dc = _clinicservice.GetClinicsListbyDoctorid(model.DoctorID).Where(x => x.OrganizationId == base.loginUserModel.OrganizationId).ToList();

            model.Clinics = dc.Select(x => new ClinicModel { ClinicName = StringCipher.Decrypt(x.Clinics.ClinicName), ClinicId = x.Clinics.ClinicId }).ToList();
              

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = _commonUtilities.GetDisplayName(AppointmentType.ClinicVisit), Value = ((int)AppointmentType.ClinicVisit).ToString() });
            items.Add(new SelectListItem { Text = _commonUtilities.GetDisplayName(AppointmentType.VideoConsult), Value = ((int)AppointmentType.VideoConsult).ToString() });
            items.Add(new SelectListItem { Text = _commonUtilities.GetDisplayName(AppointmentType.HomeVisit), Value = ((int)AppointmentType.HomeVisit).ToString() });
            model.AppointmentTypeList = items;
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            var countrylist = _countryService.GetCountryList();
            for (int i = 0; i < countrylist.Count; i++)
            {
                countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
            }
            model.CountryList = countrylist;
            model.CityList = new List<LookUpCity>();
            model.StateList = new List<LookUpState>();
            return model;
        }

        [HttpPost]
        public ActionResult AddAvailability(DoctorAvailabilityModel model, FormCollection collection)
        {
            try
            {
                if (Convert.ToInt32(collection.Get("AppointmentType")) != (Int32)AppointmentType.HomeVisit)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("DoctorID", collection.Get("DoctorID"));
                    dic.Add("AppointmentType", collection.Get("AppointmentType"));
                    dic.Add("Fromdate", collection.Get("FromdateLocal"));
                    dic.Add("FromHour", collection.Get("FromHour"));
                    dic.Add("FromMin", collection.Get("FromMin"));
                    dic.Add("FromAMPM", collection.Get("FromAMPM"));
                    dic.Add("Todate", collection.Get("TodateLocal"));
                    dic.Add("ToHour", collection.Get("ToHour"));
                    dic.Add("ToMin", collection.Get("ToMin"));
                    dic.Add("ToAMPM", collection.Get("ToAMPM"));
                    dic.Add("DayIds", collection.Get("DayIds"));
                    if (Convert.ToInt32(collection.Get("AppointmentType")) == (Int32)AppointmentType.VideoConsult)
                    {
                        dic.Add("TimeZoneId", collection.Get("TimeZoneId"));
                        dic.Add("ClinicID", "0");
                        dic.Add("CountryId", "0");
                        dic.Add("State", "0");
                        dic.Add("City", "0");
                        dic.Add("PostCode", "0");
                    }
                    else if (Convert.ToInt32(collection.Get("AppointmentType")) == (Int32)AppointmentType.ClinicVisit)
                    {
                        dic.Add("TimeZoneId", collection.Get("hdntimezone"));
                        dic.Add("ClinicID", base.loginUserModel.ClinicId.ToString());
                        dic.Add("CountryId", "0");
                        dic.Add("State", "0");
                        dic.Add("City", "0");
                        dic.Add("PostCode", "0");
                    }
                    insertAvailability(dic);
                    if (TempData["ErrorMessage"] != null)
                    {
                        return RedirectToAction("AddAvailability", new { Id = model.DoctorID });
                    }
                    else
                    {
                        return RedirectToAction("index", new { Id = model.DoctorID });
                    }

                }
                else
                {
                    var counter = Convert.ToInt32(collection.Get("Counter"));
                    for (int i = 1; i <= counter; i++)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("DoctorID", collection.Get("DoctorID"));
                        dic.Add("AppointmentType", collection.Get("AppointmentType"));
                        dic.Add("Fromdate", collection.Get("FromdateLocal" + i));
                        dic.Add("FromHour", collection.Get("FromHour" + i));
                        dic.Add("FromMin", collection.Get("FromMin" + i));
                        dic.Add("FromAMPM", collection.Get("FromAMPM" + i));
                        dic.Add("Todate", collection.Get("TodateLocal" + i));
                        dic.Add("ToHour", collection.Get("ToHour" + i));
                        dic.Add("ToMin", collection.Get("ToMin" + i));
                        dic.Add("ToAMPM", collection.Get("ToAMPM" + i));
                        dic.Add("DayIds", collection.Get("DayIds" + i));

                        dic.Add("TimeZoneId", "");
                        dic.Add("ClinicID", "0");
                        dic.Add("CountryId", collection.Get("countryId" + i));
                        dic.Add("State", collection.Get("stateId" + i));
                        dic.Add("City", collection.Get("cityId" + i));
                        dic.Add("PostCode", collection.Get("hidPostcode" + i));
                        insertAvailability(dic);
                    }
                    return RedirectToAction("index", new { Id = model.DoctorID });
                }
            }
            catch
            {
                return View(model);
            }

        }

        public ActionResult GetTimezoneOfClinics(int Id)
        {
            var City = _clinicservice.GetTimezoneofClinics(Id);
            // Returns string "Electronic" or "Mail"
            return Json(City, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAvailabilityByTypeId(int Id, string fdate, string tdate, string doctorid)
        {
            DateTime fromdate = DateTime.ParseExact(fdate, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            DateTime todate = DateTime.ParseExact(tdate, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var DoctorAvailability = _doctorService.GetDoctorAvailabilityListByTypeIdWithDates(Id, fromdate, todate, Convert.ToInt32(doctorid));
            // Returns string "Electronic" or "Mail"
            return Json(DoctorAvailability, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAvailabilityByAvailabilityId(int Id)
        {
            Dictionary<string, object> dicMain = new Dictionary<string, object>();
            List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
            var DoctorAvailability = _doctorService.GetAvailabilityByAvailabilityId(Id);
            if (DoctorAvailability != null)
            {
                for (int i = 0; i < DoctorAvailability.Count; i++)
                {
                    var startDate = DoctorAvailability[i].FromDateLocal.ToString("dd MMM yyyy");
                    var startTime = DoctorAvailability[i].FromDateLocal.ToString("h:mm tt");
                    var endTime = DoctorAvailability[i].ToDateLocal.ToString("h:mm tt");
                    var time = startTime + "-" + endTime;
                    Dictionary<string, object> avdetails = new Dictionary<string, object>();
                    avdetails.Add("Startdate", startDate);
                    avdetails.Add("Time", time);
                    avdetails.Add("TimeZone", DoctorAvailability[i].TimeZoneString);
                    dic.Add(avdetails);
                }
            }
            dicMain.Add("data", dic);
            // Returns string "Electronic" or "Mail"
            return Json(dicMain, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Details(int Id)
        //{
        //    var doctorAvailabilityModel = new DoctorAvailabilityModel();
        //    var doctorAvailabilityDaysByAvailabilityId = _doctorService.GetAvailabilityDaysList(Convert.ToInt32(Id)).
        //        Select(x => new DoctorAvailabilityModel
        //    {
        //        FromDateLocal = x.FromDateLocal,
        //        ToDateLocal = x.ToDateLocal,
        //    }).ToList();
        //    doctorAvailabilityModel.Type1 = doctorAvailabilityDaysByAvailabilityId;
        //    return View(doctorAvailabilityModel);
        //}



        public void insertAvailability(Dictionary<string, string> dic)
        {
            DoctorAvailabilityModel model = new DoctorAvailabilityModel();
            string fromtime = dic["FromHour"] + ":" + dic["FromMin"] + " " + dic["FromAMPM"];
            DateTime dtFromdate = DateTime.ParseExact(dic["Fromdate"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string fromtimeL = dtFromdate.ToString("MM/dd/yyyy").ToString() + " " + fromtime;
            model.FromDateLocal = DateTime.ParseExact(fromtimeL, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            string totime = dic["ToHour"] + ":" + dic["ToMin"] + " " + dic["ToAMPM"];
            DateTime dtTodate = DateTime.ParseExact(dic["Todate"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string totimeL = dtTodate.ToString("MM/dd/yyyy").ToString() + " " + totime;
            model.ToDateLocal = DateTime.ParseExact(totimeL, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var FlagDateAdd = 0;
            string tdate = dtFromdate.ToString("MM/dd/yyyy").ToString() + " " + totime;
            DateTime tdatetime = DateTime.ParseExact(tdate, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            if (tdatetime <= model.FromDateLocal)
            {
                tdatetime = tdatetime.AddDays(1);
                FlagDateAdd = 1;
            }
            TimeZoneInfo cstZone;
            if (Convert.ToInt32(dic["AppointmentType"]) != (Int32)AppointmentType.HomeVisit)
            {
                cstZone = TimeZoneInfo.FindSystemTimeZoneById(dic["TimeZoneId"]);
                model.FromDateUTC = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(fromtimeL, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture), cstZone);
                model.ToDateUTC = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(totimeL, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture), cstZone);
            }
            else
            {
                model.FromDateUTC = DateTime.ParseExact(fromtimeL, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                model.ToDateUTC = DateTime.ParseExact(totimeL, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
            //binddropdwon(model);
            model.AvailabilityType = Convert.ToInt32(dic["AppointmentType"]);
            model.ClinicID = Convert.ToInt32(dic["ClinicID"]);
            model.CountryId = Convert.ToInt32(dic["CountryId"]);
            model.State = Convert.ToInt32(dic["State"]);
            model.City = Convert.ToInt32(dic["City"]);
            model.PostCode = dic["PostCode"];
            //if (dic["TimeZoneId"] != "")
            //{
            //    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(dic["TimeZoneId"]);
            //    TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(model.FromDateUTC));
            //    model.TimeZoneId = GetTimeZoneOffset(offset);
            //}
            //else
            //{
            //    model.TimeZoneId = dic["TimeZoneId"];
            //}
            model.TimeZoneId = dic["TimeZoneId"];
            model.DoctorID = Convert.ToInt32(dic["DoctorID"]);
            DoctorAvailability dModel = new DoctorAvailability();
            dModel = model.GetEditModel(model, model.DoctorID);
            //get old inserted data
            var DoctorAvailabilityexistdata = _doctorService.GetDoctorAvailabilityListByTypeId(model.AvailabilityType, Convert.ToInt32(model.DoctorID));
            string[] availabileIdArr = new string[DoctorAvailabilityexistdata.Count];
            int k = 0;
            foreach (var Availability in DoctorAvailabilityexistdata)
            {
                if ((Availability.FromDateLocal >= model.FromDateLocal && Availability.FromDateLocal < model.ToDateLocal) ||
                    (Availability.ToDateLocal < model.FromDateLocal && Availability.ToDateLocal >= model.ToDateLocal) ||
                    (Availability.FromDateLocal <= model.FromDateLocal && Availability.ToDateLocal >= model.ToDateLocal))
                {
                    availabileIdArr[k] = Availability.AvailabilityId.ToString();
                    k++;
                }
            }
            if (availabileIdArr.Length > 0 && availabileIdArr[0] != null)
            {
                var doctorAvailabilityModel = new DoctorAvailabilityModel();
                var doctordetail = _doctorService.GetDoctorById(Convert.ToInt32(dic["DoctorID"])).FirstOrDefault();
                if (doctordetail != null)
                {
                    doctorAvailabilityModel.FullName = doctordetail.FirstName + " " + doctordetail.SurName;
                    doctorAvailabilityModel.ProfilePic = _userService.Find(doctordetail.LoginId).Avatar;
                    doctorAvailabilityModel.OrgName = _organizationService.GetOrganizationById(doctordetail.OrgId).OrganizationName + "/" + _clinicservice.GetClinicById(doctordetail.OrgId).ClinicName;
                }
                //var cmodel = _clinicservice.GetClinicsListbyDoctorid(Convert.ToInt32(dic["DoctorID"]));
                doctorAvailabilityModel.DoctorID = Convert.ToInt32(dic["DoctorID"]);
                //binddropdwon(doctorAvailabilityModel);
                TempData["ErrorMessage"] = "This time Duration Doctor Availability Already Exists";
                return;
            }
            var DoctorAvailability = _doctorService.AddAvailability(dModel);
            AvailabilityDays admodel = new AvailabilityDays();
            int daydiff = (dtTodate - dtFromdate).Days;
            daydiff = daydiff + 1;
            var frominnerdate = DateTime.ParseExact(fromtimeL, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var toinnerdate = tdatetime;
            var ids = dic["DayIds"];
            string[] arrayIds = new string[7];
            if (ids != string.Empty)
            {
                arrayIds = ids.Split(',');
            }
            for (int dayc = 0; dayc < daydiff; dayc++)
            {
                int pos = -1;
                if (ids != string.Empty)
                {
                    pos = Array.IndexOf(arrayIds, (Convert.ToInt32(frominnerdate.DayOfWeek)).ToString());
                }
                if (pos > -1 || ids == string.Empty)
                {
                    admodel.FromDateLocal = frominnerdate;
                    admodel.ToDateLocal = toinnerdate;
                    TimeZoneInfo cstZoneInner;
                    if (Convert.ToInt32(dic["AppointmentType"]) != (Int32)AppointmentType.HomeVisit)
                    {
                        cstZoneInner = TimeZoneInfo.FindSystemTimeZoneById(dic["TimeZoneId"]);
                        admodel.FromDateUTC = TimeZoneInfo.ConvertTimeToUtc(frominnerdate, cstZoneInner);
                        admodel.ToDateUTC = TimeZoneInfo.ConvertTimeToUtc(toinnerdate, cstZoneInner);
                    }
                    else
                    {
                        admodel.FromDateUTC = frominnerdate;
                        admodel.ToDateUTC = toinnerdate;
                    }
                    admodel.AvailabilityId = DoctorAvailability.AvailabilityId;
                    admodel.DayID = (Int32)frominnerdate.DayOfWeek;
                    admodel.TimeZoneString = dic["TimeZoneId"];
                    _doctorService.AddAvailabilityDays(admodel);
                }
                frominnerdate = frominnerdate.AddDays(1);
                toinnerdate = toinnerdate.AddDays(1);
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
                    Time = dt.Substring(0, 5);
                }
                else
                {
                    Time = dt.Substring(0, 5);
                    Time = "+" + Time;
                }
            }
            return Time;
        }

    }
}