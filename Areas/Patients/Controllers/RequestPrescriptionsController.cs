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

namespace MyOnlineClinic.Web.Areas.Patients.Controllers
{
    public class RequestPrescriptionsController : BaseController
    {
        private readonly IRequestPrescriptionService _requestPrescriptionService;
        private readonly IDoctorService _doctorService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly IClinicService _clinicService;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IVoucherService _voucherService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentHelper _paymentHelper;
        public RequestPrescriptionsController(IRequestPrescriptionService requestPrescriptionService, IDoctorService doctorService, IUserService userService, IOrganizationService organizarionSerivce, IClinicService clinicService, ICountryService countryService, IStateService stateService, ICityService cityService,
        IVoucherService voucherService, IPaymentService paymentService, IPaymentHelper paymentHelper)
        {
            _requestPrescriptionService = requestPrescriptionService;
            _doctorService = doctorService;
            _userService = userService;
            _organizationService = organizarionSerivce;
            _clinicService = clinicService;
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;
            _voucherService = voucherService;
            _paymentService = paymentService;
            _paymentHelper = paymentHelper;
        }
        public ActionResult Index()
        {
            //RequestPrescriptionViewModel model = new RequestPrescriptionViewModel();
            var model = _requestPrescriptionService.GetRequestPrescriptionListByPatientId(base.loginUserModel.LoginId).Select(x => new RequestPrescriptionViewModel
            {
                PatientLoginid = x.PatientLoginid,
                DoctorLoginid = x.DoctorLoginid,
                AppointmentId = x.AppointmentId,
                Medication_AmtId = x.Medication_AmtId,
                strenght = x.strenght,
                type = x.type,
                Dose = x.Dose,
                YearStarted = x.YearStarted,
                Rout = x.Rout,
                PRN = x.PRN,
                Consideration = x.Consideration,
                NoNeeded = x.NoNeeded,
                PbsRpbsValue = x.PbsRpbsValue,
                IsBrandSubstitue = x.IsBrandSubstitue,
                IsCompleted = x.IsCompleted,
                RPTNumber = x.RPTNumber,
                ScriptId = x.ScriptId,
                quantity = x.quantity,
                Medicineperiod = x.Medicineperiod,
                RequestType = x.RequestType,
                AmtMedicationName = _requestPrescriptionService.GetAmtMedicationById(x.Medication_AmtId).MedicationTerm,
                Frequency = x.Frequency,
                PatientId = _userService.GetPatientByLoginId(x.PatientLoginid).MemberId,
                PatientMedicationHistoryId = x.PatientMedicationHistoryId


            }).ToList();

            return View(model);
        }



        public JsonResult FindTreatingDoctors()
        {
            var patientid = _userService.GetPatientByLoginId(base.loginUserModel.LoginId);
            var TreatingDoctor = _doctorService.GetTreatingDoctorsList().Where(x => x.PatientId == patientid.MemberId).Select(x => new RequestPrescriptionViewModel
            {
                PatientId = x.PatientId,
                DoctorId = x.DoctorId,
                DoctorName = _doctorService.GetSingleDoctor(x.DoctorId).FirstName + " " + _doctorService.GetSingleDoctor(x.DoctorId).SurName,
               // ClinicName = StringCipher.Decrypt(_clinicService.GetClinicById(x.ClinicId).ClinicName)
            }).ToList();

            return Json(TreatingDoctor, JsonRequestBehavior.AllowGet);

        }
        public JsonResult FindOthersDoctors(string name)
        {
            var patientid = _userService.GetPatientByLoginId(base.loginUserModel.LoginId);
            var FindOtherDoctor = _doctorService.GetDoctorList().Where(x => x.Profession == (int)Profession.GP && x.FirstName.Contains(name) || x.SurName.Contains(name)).Select(x => new RequestPrescriptionViewModel
                {
                    DoctorId = x.MemberId,
                    DoctorName = StringCipher.Decrypt(x.FirstName + " " + x.SurName),
                    OrganizatioName = StringCipher.Decrypt(_organizationService.GetOrganizationById(x.OrgId).OrganizationName)

                }).ToList();

            return Json(FindOtherDoctor, JsonRequestBehavior.AllowGet);



        }

        public JsonResult AddRequestedPrescription(string MedicaineId, int Doctorid)
        {
            RequestPrescription Request = new RequestPrescription();
            var DoctorLoginid = _doctorService.GetDoctorById(Doctorid).FirstOrDefault().LoginId;
            string[] id = MedicaineId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < id.Length; i++)
            {
                Request.PatientLoginid = base.loginUserModel.LoginId;
                Request.DoctorLoginId = DoctorLoginid;
                Request.MedicineId = Convert.ToInt32(id[i]);
                Request.SeenbyAdmin = false;
                Request.SeenbyAdmin = false;
                _requestPrescriptionService.Add(Request);


            }
            return Json(Request, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PayWithCreditCard(int? id)
        {
            var patientid = base.loginUserModel.LoginId;
            var patient = _userService.GetPatientByLoginId(patientid);
            CreditCardViewModel model = new CreditCardViewModel();

            model.PatientId = patient.MemberId;
            return View("Payment", model);
        }
        [HttpPost]
        public ActionResult PayWithCreditCard(CreditCardViewModel model)
        {

            var response = _paymentHelper.PayWithCreditCard(model);

            if (response != null && response.Status == "approved")
            {
                var cModel = new MyOnlineClinic.Entity.Payment();
                cModel.Amount = 12;
                cModel.Status = 1;
                cModel.Response = response.ResponseString;
                cModel.PaymentDate = DateTime.UtcNow;
                cModel.PaymentSource = 2;
                cModel.LoginId = base.loginUserModel.LoginId;
                cModel.AppointmentId = model.AppointmentId;
                _paymentService.Add(cModel);
                return RedirectToAction("BookAppointment");
            }


            return View();
        }
    }
}