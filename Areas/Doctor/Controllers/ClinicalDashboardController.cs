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

namespace MyOnlineClinic.Web.Areas.Doctor.Controllers
{
    public class ClinicalDashboardController : BaseController
    {
        IClinicalService _clinicalservice;
        IBloodPressureHeartRateService _bloodPressureHeartRateService;
        IBloodGlucoseService _bloodGlucoseService;
        IOxygenSaturationService _oxygenSaturationService;
        IHeightResultService _heightResultService;
        ITemperatureResultService _temperatureResultService;
        IWaistResultService _waistResultService;
      private readonly IWeightResultService _weightResultService;

        public ClinicalDashboardController(IClinicalService ClinicalService, IBloodPressureHeartRateService bloodPressureHeartRateService,
            IBloodGlucoseService bloodGlucoseService, IOxygenSaturationService oxygenSaturationService, IHeightResultService heightResultService,
             ITemperatureResultService temperatureResultService, IWaistResultService waistResultService, IWeightResultService weightResultService)
        {
            _clinicalservice = ClinicalService;
            _bloodPressureHeartRateService = bloodPressureHeartRateService;
            _bloodGlucoseService = bloodGlucoseService;
            _oxygenSaturationService = oxygenSaturationService;
            _heightResultService = heightResultService;
            _temperatureResultService = temperatureResultService;
            _waistResultService = waistResultService;
            _weightResultService = weightResultService;
        }
        //
        // GET: /Doctor/ClinicalDashboard/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Doctor/ClinicalDashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        //
        // GET: /Doctor/ClinicalDashboard/Create
        public ActionResult Add()
        {
            RegisterViewModel model = new RegisterViewModel();
            var regionlist = _clinicalservice.GetRegionListbyid(1);
            model.RadioTypeList = _clinicalservice.GetRadioLogyTypeList();
            model.RegionListbyId = _clinicalservice.GetRegionListbyid(1);            
            return View(model);
        }
        //public JsonResult GetRegionList(int Id)
        //{
        //    return View();
        //}
        //,int Appointmentid, int PatientUserid, int DoctorUderId
        //        public JsonResult AddFeedback(string strfeedback)
        //{
        //            return View();
        //        }
        [HttpPost]
        public JsonResult SaveBloodPressure(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate + " " + model.MeasureTime + " " + model.Period;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);

                var bloodPressureHeartRate = new BloodPressureHeartRate
                {
                    SystolicValue = model.SystolicValue,
                    DiastolicValue = model.DiastolicValue,
                    PulseValue = model.PulseValue,
                    MeasureDateTime = dtMeasureDateTime,
                    ReadingType = 1
                };

                _bloodPressureHeartRateService.Add(bloodPressureHeartRate);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Blood Pressure Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBloodPressureByPatientID(Guid Id)
        {
            var bp = _clinicalservice.GetBloodPressureByPatientID(Id).Where(x=>x.ReadingType==1).FirstOrDefault();
            return Json(bp, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetBloodPressureByOrganization()
        //{
          
        //    var bp = _clinicalservice.GetBloodPressureByPatientID(Id).Where(x => x.ReadingType == 1).FirstOrDefault();
        //    return Json(bp, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetHeartRateByPatientID(Guid Id)
        {
            var bp = _clinicalservice.GetBloodPressureByPatientID(Id).Where(x => x.ReadingType == 2).FirstOrDefault();
            return Json(bp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOxygenByPatientID(Guid Id)
        {
            var oxy = _clinicalservice.GetOxygenByPatientID(Id).FirstOrDefault();
            return Json(oxy, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHeightByPatientID(Guid Id)
        {
            var height = _clinicalservice.GetHeightByPatientID(Id).FirstOrDefault();
            return Json(height, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWeightByPatientID(Guid Id)
        {
            var height = _clinicalservice.GetWeightByPatientID(Id).FirstOrDefault();
            return Json(height, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBloodGlucoseByPatientID(Guid Id)
        {
            var bg = _clinicalservice.GetBloodGlucoseByPatientID(Id).FirstOrDefault();
            return Json(bg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWaistbyPatient(Guid Id)
        {
            var bg = _clinicalservice.GetWaistbyPatient(Id).FirstOrDefault();
            return Json(bg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTempaeraturebyPatient(Guid Id)
        {
            var bg = _clinicalservice.GetTempaeraturebyPatient(Id).FirstOrDefault();
            return Json(bg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveBloodGlucose(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate + " " + model.MeasureTime + " " + model.Period;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);

                var bloodGlucose = new BloodGlucose
                {

                    BloodSugarValue = model.BloodSugarValue,
                    MealBeforeAfter = model.MealBeforeAfter,
                    MealTime = model.MealTime,
                    MeasureDateTime = dtMeasureDateTime
                };

                _bloodGlucoseService.Add(bloodGlucose);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Blood Glucose Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveOxygen(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate + " " + model.MeasureTime + " " + model.Period;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);

                var oxygenSaturation = new OxygenSaturation
                {

                    OxygenValue = model.OxygenValue,
                    PulseValue = model.PulseValue,
                    MeasureDateTime = dtMeasureDateTime
                };

                _oxygenSaturationService.Add(oxygenSaturation);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Oxygen Saturation Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveHeartRate(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate + " " + model.MeasureTime + " " + model.Period;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);


                var bloodPressureHeartRate = new BloodPressureHeartRate
                {

                    PulseValue = model.PulseValue,
                    MeasureDateTime = dtMeasureDateTime,
                    ReadingType = 2
                };

                _bloodPressureHeartRateService.Add(bloodPressureHeartRate);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Heart Rate Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveHeight(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate + " " + model.MeasureTime + " " + model.Period;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);

                var heightResult = new HeightResult
                {
                    Height = model.Height,
                    MeasureDateTime = dtMeasureDateTime,
                };

                _heightResultService.Add(heightResult);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Height Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveTemperature(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate + " " + model.MeasureTime + " " + model.Period;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);

                var temperatureResult = new TemperatureResult
                {
                    Temperature = model.Temperature,
                    MeasureDateTime = dtMeasureDateTime,
                };

                _temperatureResultService.Add(temperatureResult);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Temperature Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveWaist(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate + " " + model.MeasureTime + " " + model.Period;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);

                var waistResult = new WaistResult
                {
                    Waist = model.Waist,
                    MeasureDateTime = dtMeasureDateTime,
                };

                _waistResultService.Add(waistResult);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Waist Result Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveWeight(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            if (model != null)
            {
                string measureDateTime = model.MeasureDate;
                var dtMeasureDateTime = DateTime.ParseExact(measureDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var weightResult = new WeightResult
                {
                    Weight = model.Weight,
                    MeasureDateTime = dtMeasureDateTime,
                };

                _weightResultService.Add(weightResult);

                jsonObject.Add("success", true);
                jsonObject.Add("message", "Weight Result Saved Successfully");
            }

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        //,int Appointmentid, int PatientUserid, int DoctorUderId
        //public JsonResult AddFeedback(string strfeedback)
        //{
        //    var region = _clinicalservice.GetRegionListbyid(Id);
        //    return Json(region, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult SetStandards(ClinicalDashBoardViewModel model)
        {
            if (model != null)
            {
                model.DoctorId = base.loginUserModel.LoginId;
                var GET = model.InsertStandards(model);
                var StandardExsit = _clinicalservice.CheckStandard(base.loginUserModel.LoginId, model.PatientId);
                if (StandardExsit)
                {
                    _clinicalservice.Update(GET);
                }
                else
                {

                    _clinicalservice.Addstandard(GET);
                }

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStandards()
        { //base.loginUserModel.LoginId=()
            var model = _clinicalservice.GetStandardByPatientID(base.loginUserModel.LoginId).FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStandardsbyAdmin(Guid DoctorId, Guid PatientLoginId, Guid OrganizationId, Guid ClinicId)
        {       
           // ClinicalDashBoardViewModel Standard = new ClinicalDashBoardViewModel();
            if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                var model = _clinicalservice.GetStandardList().Where(x => x.PatientId == PatientLoginId && x.DoctorId == DoctorId).FirstOrDefault();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
                if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    var model = _clinicalservice.GetStandardList().Where(x => x.PatientId == PatientLoginId).FirstOrDefault();
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetStandardsbyPatient(Guid LoginId)
        { //base.loginUserModel.LoginId=()
            var model = _clinicalservice.GetStandardByPatientID(LoginId).FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getNewConsult()
        {
            var ConsultModel = _clinicalservice.ConsultByPatientId(base.loginUserModel.LoginId).FirstOrDefault();
            return Json(ConsultModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Pathology(ClinicalDashBoardViewModel model)
        {
            if (model != null)
            {
                model.DoctorId = base.loginUserModel.LoginId;
                var get = model.InsertPathology(model);

                _clinicalservice.AddPathology(get);

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddFeedback(string strfeedback, string Status)
        {
            bool addstatus = false;
            if (Status != string.Empty)
            {
                var getfeedback = _clinicalservice.GetFeedbackByID(base.loginUserModel.LoginId);
                getfeedback.DoctorUserId = base.loginUserModel.LoginId;
                getfeedback.ModifiedDate = DateTime.Now;
                getEditFeedback(strfeedback, getfeedback);
                _clinicalservice.UpdateFeedback(getfeedback);
                addstatus = true;
            }
            else
            {
                FeedBack fillFeedback = new FeedBack();
                fillFeedback.DoctorUserId = base.loginUserModel.LoginId;
                getEditFeedback(strfeedback, fillFeedback);
                _clinicalservice.AddFeedback(fillFeedback);
                addstatus = true;
            }
            return Json(addstatus, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AddPersonalNote(string strNote, string Status)
        {
            bool addstatus = false;
            if (Status != string.Empty)
            {
                var getNotes = _clinicalservice.GetPersonalNoteByID(base.loginUserModel.LoginId);
                getNotes.DoctorUserId = base.loginUserModel.LoginId;
                getNotes.ModifiedDate = DateTime.Now;
                getEditNotes(strNote, getNotes);
                _clinicalservice.UpdatePersonalNote(getNotes);
                addstatus = true;
            }
            else
            {
                PersonalNote fillPersonal = new PersonalNote();
                fillPersonal.DoctorUserId = base.loginUserModel.LoginId;
                getEditNotes(strNote, fillPersonal);
                _clinicalservice.AddPersonalNote(fillPersonal);
                addstatus = true;
            }
            return Json(addstatus, JsonRequestBehavior.AllowGet);

        }
        public PatientMedicationHistory InsertStandards(RequestPrescriptionViewModel model)
        {
            var set = new PatientMedicationHistory();
            set.NoNeeded = model.NoNeeded;
            set.Medication_AmtId = model.Medication_AmtId;
            set.Medicineperiod = model.Medicineperiod;
            set.ModifiedDate = model.Modyifydate;
            set.CreatedBy = model.CreatedBy;
            set.CreatedDate = model.CreateDate;
            set.CreatedDateUTC = model.CreatedDateUTC;
            set.Dose = model.Dose;
            set.Frequency = model.Frequency;
            set.HasPrint = model.HasPrint;
            set.IpAddress = model.Ipaddress;
            set.IsActive = model.Isactive;
            set.IsApproved = model.IsApproved;
            set.IsBrandSubstitue = model.IsBrandSubstitue;
            set.IsCompleted = model.IsCompleted;
            set.Consideration = model.Consideration;
            set.DoctorLoginid = model.DoctorLoginid;
            set.PatientLoginid = model.PatientLoginid;
            return set;
        }
        public JsonResult AddMedication(int? AppointmentId, int Medication_AmtId, string strenght, string Consideration, string Dose, string AuthorityScriptNumber, string AuthorityNumber, string PRN, string Types, string Frequency, string Rout,int Quantity)
        {
            var set = new PatientMedicationHistory();
            //set.NoNeeded = model.NoNeeded;
            set.Medication_AmtId =0;
           // set.Medicineperiod = model.Medicineperiod;
          //  set.ModifiedDate = model.Modyifydate;
           // set.CreatedBy = model.CreatedBy;
          //  set.CreatedDate = model.CreateDate;
           // set.CreatedDateUTC = modCreatedDateUTC;
            set.strenght = strenght;
            set.Dose = Dose;
          //  set.Frequency = model.Frequency;
           // set.HasPrint = model.HasPrint;
         //   set.IpAddress = model.Ipaddress;
          //  set.IsActive = model.Isactive;
           // set.IsApproved = model.IsApproved;
           // set.IsBrandSubstitue = model.IsBrandSubstitue;
          //  set.IsCompleted = model.IsCompleted;
            set.Consideration = Consideration;
            set.DoctorLoginid = base.loginUserModel.LoginId;
            set.type = Types;
            set.Frequency = Frequency;
            set.Frequency = Rout;
            set.quantity = Quantity;
          //  set.PatientLoginid = '';   
  
            //if (model != null)
            //{
            //    model.DoctorLoginid = base.loginUserModel.LoginId;
            //    var GET = model.InsertStandards(model);
                //var StandardExsit = _clinicalservice.CheckStandard(base.loginUserModel.LoginId, model.PatientId);
                //if (StandardExsit)
                //{
                  //_clinicalservice.Update(GET);
                //}
                //else
                //{
                _clinicalservice.AddMedication(set);

                //}
           // }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        private static void getEditFeedback(string strfeedback, FeedBack fillFeedback)
        {
            fillFeedback.EnteredFeedBack = StringCipher.Encrypt(strfeedback);
            fillFeedback.AppointmentId = 0;

            //fillFeedback.PatientUserId = '';
            fillFeedback.CreatedDate = DateTime.Now;

            fillFeedback.IsActive = true;
            fillFeedback.IsDeleted = false;
            fillFeedback.IsApproved = false;
        }
        private static void getEditNotes(string strfeedback, PersonalNote fillNotes)
        {
            fillNotes.Notes = StringCipher.Encrypt(strfeedback);
            fillNotes.AppointmentId = 0;

            //fillNotes.PatientUserId = 0;
            fillNotes.CreatedDate = DateTime.Now;

            fillNotes.IsActive = true;
            fillNotes.IsDeleted = false;
            fillNotes.IsApproved = false;
        }
        //RadiologyTypeId: $('#hdnRadiologyTypeId').val(), RegionId: 0, RequestPrint: $('#RequestToPrint').val(), ClinicalNotes: $('#ClinicalNotes').val(), CopyResult: $('#CopyResult').val()
        public JsonResult AddRadiology(int RadiologyTypeId, int RegionId, string RequestPrint, string ClinicalNotes, string CopyResult)
        {
            var radiology = new Radiology();
            radiology.AppointmentId = 0;
            // radiology.PatientUserId = 0;
            radiology.DoctorUserId = base.loginUserModel.LoginId;
            radiology.RadiologyTypeId = RadiologyTypeId;
            radiology.RegionId = RegionId;
            radiology.RequestPrint = StringCipher.Encrypt(RequestPrint);
            radiology.ClinicalNotes = StringCipher.Encrypt(ClinicalNotes);
            radiology.CreatedDate = DateTime.Now;
            radiology.ModifiedDate = DateTime.Now;
            radiology.CopyResult = StringCipher.Encrypt(CopyResult);
            radiology.IsCompleted = 0;
            radiology.RptNumber = 0;
            radiology.PrintToAdmin = 0;
            radiology.RequestNo = 0;
            radiology.IsSentToPatient = 0;
            _clinicalservice.AddRadiology(radiology);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRadioLogyList()
        {
            var radiologylist = _clinicalservice.RadiologyListByDoctorId(base.loginUserModel.LoginId).Select(x => new RadiologyViewModel
            {
                RadiologyId = x.RadiologyId == 0 ? 1 : x.RadiologyTypeId,
                RegionId = x.RegionId == 0 ? 1 : x.RegionId,
                DoctorId = x.DoctorUserId,
                RequestPrint = StringCipher.Decrypt(x.RequestPrint),
                CopyResult = StringCipher.Decrypt(x.CopyResult),
                ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                RadiologyTypeId = x.RadiologyTypeId,

            }).ToList();
            for (int i = 0; i < radiologylist.Count; i++)
            {
                radiologylist[i].RadiologyTypeName = StringCipher.Decrypt(_clinicalservice.RadiologyTypeName(radiologylist[i].RadiologyTypeId));
                radiologylist[i].RegionName = StringCipher.Decrypt(_clinicalservice.RegionName(radiologylist[i].RegionId));
            }
            return Json(radiologylist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDrugList()
        {
           
            var org = _clinicalservice.GetDrugList().Select(x =>
                 new RequestPrescriptionViewModel
                 {
                     MedicationTerm = StringCipher.Decrypt(x.MedicationTerm),
                     Medication_AmtId = x.Medication_AmtId,
                 }).ToList();
          

            return Json(org, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMedication(Guid Loginid)
        {
            var medication = entitiesDb.PatientMedicationHistory.Where(x => x.PatientLoginid == Loginid).FirstOrDefault();
            return Json(medication, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRadioLogyListByPatientID()
        {
            var radiologylist = _clinicalservice.RadiologyListByPatientId(base.loginUserModel.LoginId).Select(x => new RadiologyViewModel
            {
                RadiologyId = x.RadiologyId == 0 ? 1 : x.RadiologyTypeId,
                RegionId = x.RegionId == 0 ? 1 : x.RegionId,
                DoctorId = x.DoctorUserId,
                RequestPrint = StringCipher.Decrypt(x.RequestPrint),
                CopyResult = StringCipher.Decrypt(x.CopyResult),
                ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                RadiologyTypeId = x.RadiologyTypeId,
              
            }).ToList();
            for (int i = 0; i < radiologylist.Count; i++)
            {
                radiologylist[i].RadiologyTypeName = StringCipher.Decrypt(_clinicalservice.RadiologyTypeName(radiologylist[i].RadiologyTypeId));
                radiologylist[i].RegionName = StringCipher.Decrypt(_clinicalservice.RegionName(radiologylist[i].RegionId));
            }
            return Json(radiologylist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRadioLogyListByAdmin(Guid DoctorId,Guid PatientLoginId, Guid OrganizationId, Guid ClinicId)
        {
            List<RadiologyViewModel> listRadiology = new List<RadiologyViewModel>();
            if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                listRadiology = _clinicalservice.GetRadiologyList().Where(x => x.PatientUserId == PatientLoginId && x.DoctorUserId == DoctorId).Select(x => new RadiologyViewModel
                {
                    RadiologyId = x.RadiologyId == 0 ? 1 : x.RadiologyTypeId,
                    RegionId = x.RegionId == 0 ? 1 : x.RegionId,
                    DoctorId = x.DoctorUserId,
                    RequestPrint = StringCipher.Decrypt(x.RequestPrint),
                    CopyResult = StringCipher.Decrypt(x.CopyResult),
                    ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                    RadiologyTypeId = x.RadiologyTypeId,

                }).ToList();
            }
            else
                if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    listRadiology = _clinicalservice.GetRadiologyList().Where(x => x.PatientUserId == PatientLoginId).Select(x => new RadiologyViewModel
                    {
                        RadiologyId = x.RadiologyId == 0 ? 1 : x.RadiologyTypeId,
                        RegionId = x.RegionId == 0 ? 1 : x.RegionId,
                        DoctorId = x.DoctorUserId,
                        RequestPrint = StringCipher.Decrypt(x.RequestPrint),
                        CopyResult = StringCipher.Decrypt(x.CopyResult),
                        ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                        RadiologyTypeId = x.RadiologyTypeId,

                    }).ToList();
                }
                for (int i = 0; i < listRadiology.Count; i++)
                {
                    listRadiology[i].RadiologyTypeName = StringCipher.Decrypt(_clinicalservice.RadiologyTypeName(listRadiology[i].RadiologyTypeId));
                    listRadiology[i].RegionName = StringCipher.Decrypt(_clinicalservice.RegionName(listRadiology[i].RegionId));
                }          
            
            return Json(listRadiology, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathologyList()
        {
            var PathologyList = _clinicalservice.PathologyListByDoctorId(base.loginUserModel.LoginId).Select(x => new ClinicalDashBoardViewModel
            {
                PathologyId = x.PathologyId,
                TestName = StringCipher.Decrypt(x.TestName),
                FastingNonFastingValue = StringCipher.Decrypt(x.FastingNonFastingValue),
                ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                InstructionForPatient = StringCipher.Decrypt(x.InstructionForPatient),
                CopyResultTo = StringCipher.Decrypt(x.CopyResultTo),
                Urgent= StringCipher.Decrypt(x.Urgent)
            }).ToList();
            return Json(PathologyList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathologyByPatientId(Guid PatientLoginId, Guid OrganizationId, Guid ClinicId)
        {
            List<ClinicalDashBoardViewModel> listPathology = new List<ClinicalDashBoardViewModel>();
            if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" )
            {
                listPathology = _clinicalservice.PathologyListByPatientId(base.loginUserModel.LoginId).Select(x => new ClinicalDashBoardViewModel
               {
                   PathologyId = x.PathologyId,
                   TestName = StringCipher.Decrypt(x.TestName),
                   FastingNonFastingValue = StringCipher.Decrypt(x.FastingNonFastingValue),
                   ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                   InstructionForPatient = StringCipher.Decrypt(x.InstructionForPatient),
                   CopyResultTo = StringCipher.Decrypt(x.CopyResultTo),
                   Urgent = StringCipher.Decrypt(x.Urgent)
               }).ToList();
            }
            else
            {

            }
            return Json(listPathology, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathologyListByAdmin(Guid DoctorId, Guid PatientLoginId, Guid OrganizationId, Guid ClinicId)
        {
            List<ClinicalDashBoardViewModel> listPathology = new List<ClinicalDashBoardViewModel>();
            if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                listPathology = _clinicalservice.GetPathologyList().Where(x => x.PatientLoginId == PatientLoginId && x.DoctorLoginId == DoctorId).Select(x => new ClinicalDashBoardViewModel
                {
                    PathologyId = x.PathologyId,
                    TestName = StringCipher.Decrypt(x.TestName),
                    FastingNonFastingValue = StringCipher.Decrypt(x.FastingNonFastingValue),
                    ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                    InstructionForPatient = StringCipher.Decrypt(x.InstructionForPatient),
                    CopyResultTo = StringCipher.Decrypt(x.CopyResultTo),
                    Urgent = StringCipher.Decrypt(x.Urgent)
                }).ToList();
            }
            else
                if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    listPathology = _clinicalservice.GetPathologyList().Where(x => x.PatientLoginId == PatientLoginId).Select(x => new ClinicalDashBoardViewModel
                    {
                        PathologyId = x.PathologyId,
                        TestName = StringCipher.Decrypt(x.TestName),
                        FastingNonFastingValue = StringCipher.Decrypt(x.FastingNonFastingValue),
                        ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                        InstructionForPatient = StringCipher.Decrypt(x.InstructionForPatient),
                        CopyResultTo = StringCipher.Decrypt(x.CopyResultTo),
                        Urgent = StringCipher.Decrypt(x.Urgent)
                    }).ToList();
                }
            return Json(listPathology, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFeedback()
        {
            var count = _clinicalservice.GetFeedbackByDoctorID(base.loginUserModel.LoginId);
            string strfeedback = string.Empty;
            if (count > 0)
            {
                strfeedback = StringCipher.Decrypt(Convert.ToString(_clinicalservice.GetFeedbackByID(base.loginUserModel.LoginId).EnteredFeedBack));
            }

            else
            {
                strfeedback = string.Empty;

            }

            return Json(strfeedback, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPersonalNote()
        {
            var count = _clinicalservice.GetPersonalNoteByDoctorID(base.loginUserModel.LoginId);
            string strfeedback = string.Empty;
            if (count > 0)
            {
                strfeedback = StringCipher.Decrypt(Convert.ToString(_clinicalservice.GetPersonalNoteByID(base.loginUserModel.LoginId).Notes));
            }
            else
            {
                strfeedback = string.Empty;

            }
            return Json(strfeedback, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRegionList(int Id)
        {
            var region = _clinicalservice.GetRegionListbyid(Id);
            return Json(region, JsonRequestBehavior.AllowGet);
        }
        #region EcgThroat New Consult Dheeraj
        public ActionResult CheckSkin(int id)
        {
            TempData["Id"] = id;
            TempData.Keep("Id");
            return View();

        }
        public JsonResult SaveEcg(ClinicalDashBoardViewModel model)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();

            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

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

                        var convertedFileName = GetFileName(model.EcgThroatFile);
                        var newConvertedFielName = convertedFileName;
                        convertedFileName = Path.Combine(Server.MapPath("~/UploadedFiles/"), convertedFileName);
                        file.SaveAs(convertedFileName);

                        var ecg = new EcgFiles();
                        ecg.Subject = model.Subject;
                        ecg.EcgFielsOriginalName = model.EcgThroatFile;
                        ecg.PatientId = model.PatientId;
                        ecg.EcgFielsConvertedName = newConvertedFielName;
                        _clinicalservice.AddEcgFile(ecg);

                        jsonObject.Add("success", true);
                        jsonObject.Add("message", "Ecg FileSaved Successfully");
                    }

                    return Json(jsonObject, JsonRequestBehavior.AllowGet);
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
        }

        public JsonResult SaveSkinThroat(ClinicalDashBoardViewModel model)
        {
            UploadImage(model);
            if (TempData["Id"] == "1")
            {
                ThroatFiles throat = new ThroatFiles();
                throat.ThroatFilesOriginalName = model.SkinThroatFile;
                throat.DoctorId = base.loginUserModel.LoginId;
                throat.PatientId = model.PatientId;
                throat.ThroatFilesConvertedName = GetFileName(model.SkinThroatFile);
                _clinicalservice.AddThroatFile(throat);
            }
            else if (TempData["Id"] == "2")
            {
                SkinPhotos skin = new SkinPhotos();
                skin.SkinPhotosOriginalName = model.SkinThroatFile;
                skin.SkinPhotosConvertedName = GetFileName(model.SkinThroatFile);
                skin.DoctorId = base.loginUserModel.LoginId;
                skin.PatientId = model.PatientId;
                _clinicalservice.AddSkinPhoto(skin);

            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ClinicalDashBoardViewModel UploadImage(ClinicalDashBoardViewModel model)
        {
            if (model.SkinThroatFile != null)
            {
                var fileName = Path.GetFileName(model.SkinThroatFile);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    if (!string.IsNullOrEmpty(model.SkinThroatFile))
                    {
                        //Delete exising File
                        var file = Server.MapPath(model.SkinThroatFile);
                        imageHelper.Delete(file);
                    }
                    string fName = Convert.ToString(DateTime.Now.ToString("ddMMyyyy-HHmmss-fff")) + extension;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/SkinThroat/"), fName);

                    model.SkinThroatFile = "/UploadedFiles/SkinThroat/" + fName;
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
                if (model.SkinThroatFile != null)
                {
                    model.SkinThroatFile = model.SkinThroatFile;
                }

            }
            return model;
        }

        public JsonResult SaveNewConsult(ClinicalDashBoardViewModel model)
        {
            NewConsultation SaveConsult = new NewConsultation();
            SaveConsult.PatientId = model.PatientId;
            SaveConsult.DoctorId = base.loginUserModel.LoginId;
            SaveConsult.ReasonForContact = model.ReasonForContact;
            SaveConsult.Plans = model.Plans;
            SaveConsult.Objective = model.Objective;
            SaveConsult.Assessment = model.Assessment;
            SaveConsult.Subjective = model.Subjective;
            SaveConsult.DiagnosisId = 1;
            SaveConsult.AppointmentId = 1;
            _clinicalservice.AddNewConsult(SaveConsult);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public static string GetFileName(string fileName)
        {
            return Guid.NewGuid().ToString().Replace('-', '_') + Path.GetExtension(fileName).ToString();
        }
        #endregion
        [HttpPost]
        public JsonResult GetBloodPressure(int? id)
        {
            var listBloodPressure = _bloodPressureHeartRateService.GetBloodPressureHeartRateList().Select(x => new ClinicalDashBoardViewModel
            {
                SystolicValue = x.SystolicValue,
                DiastolicValue = x.DiastolicValue,
                PulseValue = x.PulseValue,
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();
            return Json(listBloodPressure, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetBloodGlucose(int? id)
        {
            var listBloodSugar = _bloodGlucoseService.GetBloodGlucoseList().Select(x => new ClinicalDashBoardViewModel
            {
                BloodSugarValue = x.BloodSugarValue,
                
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();
            return Json(listBloodSugar, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOxygen(int? id)
        {
            var listOxygenSaturation = _oxygenSaturationService.GetList().Select(x => new ClinicalDashBoardViewModel
            {
                OxygenValue = x.OxygenValue,
                PulseValue = x.PulseValue,
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();

            return Json(listOxygenSaturation, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHeartRate(int? id)
        {
            var listBloodPressure = _bloodPressureHeartRateService.GetBloodPressureHeartRateList().Select(x => new ClinicalDashBoardViewModel
            {
                PulseValue = x.PulseValue,                
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();
            return Json(listBloodPressure, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHeight(int? id)
        {
            var listHeight = _heightResultService.GetList().Select(x => new ClinicalDashBoardViewModel
            {
                Height = x.Height,
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();
            return Json(listHeight, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTemperature(int? id)
        {
            var listTemprature = _temperatureResultService.GetList().Select(x => new ClinicalDashBoardViewModel
            {
                Temperature = x.Temperature,
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();
            return Json(listTemprature, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWaist(int? id)
        {
            var listWaistService = _waistResultService.GetList().Select(x => new ClinicalDashBoardViewModel
            {
                Waist = x.Waist,
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();
            return Json(listWaistService, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWeight(int? id)
        {
            var listWeight = _weightResultService.GetList().Select(x => new ClinicalDashBoardViewModel
            {
                Weight = x.Weight,
                MeasureDate = x.MeasureDateTime.ToString("yyyy-MM-dd")
            }).ToList();
            return Json(listWeight, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNewConsultationList()
        {
            var listNewConsultation = _clinicalservice.GetNewConsultationList(new Guid());
            return Json(listNewConsultation, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getNewConsultbyAdmin(Guid DoctorId, Guid PatientLoginId, Guid OrganizationId, Guid ClinicId)
        {
            if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                //var listNewConsultation = string.Empty;
                var listNewConsultation = _clinicalservice.GetNewConsultationList().Where(x => x.DoctorId == DoctorId && x.PatientId == PatientLoginId).Select(x => new RequestPrescriptionViewModel
                {
                    SubmitDate= Convert.ToString(x.CreatedDate).Substring(0,10),
                    Reason = x.ReasonForContact,

                   DiganosisName=_clinicalservice.getDiagnosisNameById(x.DiagnosisId).MedicationTerm,             
                }).ToList();
                
                return Json(listNewConsultation, JsonRequestBehavior.AllowGet);
            }
            else if (OrganizationId.ToString() == "00000000-0000-0000-0000-000000000000" && ClinicId.ToString() == "00000000-0000-0000-0000-000000000000" && DoctorId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                //var listNewConsultation = string.Empty;
                var listNewConsultation = _clinicalservice.GetNewConsultationList().Where(x =>x.PatientId == PatientLoginId).Select(x => new RequestPrescriptionViewModel
                {
                    SubmitDate = Convert.ToString(x.CreatedDate).Substring(0, 10),
                    Reason = x.ReasonForContact,

                    DiganosisName = _clinicalservice.getDiagnosisNameById(x.DiagnosisId).MedicationTerm,
                }).ToList();

                return Json(listNewConsultation, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }  
    }

}
