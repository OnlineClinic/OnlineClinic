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
    public class DashBoardController : BaseController
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
       IMembeshipVoucherService  _membeshipVoucherService;
        IPatientService _patientService;
        IDoctorMembershipService _doctorMembershipService;
        private readonly IVoucherService _voucherservice;
        IUnregisteredClinicService _unregisteredClinicService;
        IUregisteredOrganizationService _unregisteredOrganizationService;
        private readonly ITitleService _TitleServices;
        private readonly IGenderService _genderService;
        public DashBoardController(ICountryService countryService, ICityService cityService, IFormsAuthentication formsAuth, IUserService userService, IDoctorService doctorservice, IOrganizationService organizationservice, ITimeZoneService timezoneservice, ILoginHelper loginHelper, IProfessionTypeService professiontypeservice, IClinicService clinicservice, ITitleService TitleServices, IGenderService genderService, IStateService stateservice, IPatientService Patientservice, IUregisteredOrganizationService unregisteredOrganizationService, IUnregisteredClinicService unregisteredClinicService,IVoucherService voucherservice, IDoctorMembershipService doctorMembershipService,IMembeshipVoucherService  membeshipVoucherService)
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
            _patientService = Patientservice;
            _unregisteredOrganizationService = unregisteredOrganizationService;
            _unregisteredClinicService = unregisteredClinicService;
            _voucherservice=voucherservice;
            _doctorMembershipService= doctorMembershipService;
            _membeshipVoucherService=membeshipVoucherService;
        }

        public ActionResult Index()
        {


            return View();
        }
        //
        // POST: /Account/LogOff
        //[HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            _formsAuth.SignOut();
            return RedirectToAction("Index", "Home", new { @area = "" });
        }
        public ActionResult UpdateProfile()
        {


            var cModel = _doctorservice.DoctorByLoginId(base.loginUserModel.LoginId).Select(x => new RegisterViewModel
            {
                TitleId = x.Title,
                FirstName = StringCipher.Decrypt(x.FirstName),
                MiddleName = StringCipher.Decrypt(x.MiddleName),
                SurName = StringCipher.Decrypt(x.SurName),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                GenderId = x.Gender,
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                City = x.City,
                State = x.State,
                CountryId = x.CountryId,
                Profession = x.Profession,
                PostCode = StringCipher.Decrypt(x.PostCode),
                Timezone = StringCipher.Decrypt(x.TimeZoneString),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                OrgId = x.OrgId,
                IsActive = x.IsActive,
                MemberId = x.MemberId,
                LoginId = x.LoginId,
                Createdby = (Guid)x.CreatedBy,
                PhoneCode = x.PhoneCode,
                LanguageSpoken = StringCipher.Decrypt(x.LanguageSpoken),
                ProfessionCategory = x.ProfessionCategory,
                IntrestArea = StringCipher.Decrypt(x.IntrestArea),
                qualification = StringCipher.Decrypt(x.qualification),
                AHPRANo = StringCipher.Decrypt(x.AHPRANo),
                VideoProviderNo = StringCipher.Decrypt(x.VideoProviderNo),
                HomeVisitProviderNo = StringCipher.Decrypt(x.HomeVisitProviderNo),
                ClinicProviderNo = StringCipher.Decrypt(x.ClinicProviderNo),
                PrescriberNo = StringCipher.Decrypt(x.PrescriberNo),
                DoctorProfile = StringCipher.Decrypt(x.DoctorProfile),

            }).FirstOrDefault();

            var commonUtilities = new CommonUtilities();

            cModel.membership = _doctorservice.GetDoctorMemebership(cModel.MemberId);
            if (cModel.membership != null)
            {
                cModel.paymentType = (cModel.membership.MembershipPaidBy == (int)MembershipTakenBy.PayPal ? commonUtilities.GetDisplayName(MembershipTakenBy.PayPal) : commonUtilities.GetDisplayName(MembershipTakenBy.Voucher));
            }

            if (cModel != null)
            {
                cModel.ProfilePic = _userService.Find(cModel.LoginId).Avatar;

                if (_doctorservice.GetDoctorList().Where(y => y.MemberId == cModel.MemberId).Select(y => y.SignaturePic).FirstOrDefault() != null)
                {
                    cModel.SignaturePic = _doctorservice.GetDoctorList().Where(y => y.MemberId == cModel.MemberId).Select(y => y.SignaturePic).FirstOrDefault();
                }
            }
            BindDropDown(cModel);
            return View(cModel);

        }
        [HttpPost]
        public ActionResult Membership(RegisterViewModel model)
        {
            var voucher = _voucherservice.GetVoucherByNumber(StringCipher.Encrypt((model.VoucherNo)));
            if (voucher == null)
            {
                TempData["ErrorMessage"] = "Invalid voucher number";
               // return View();
            }
            else if (voucher != null)
            {
                //  var voucherAssign = _voucherService.GetVoucherDetailsById(voucher.VoucherID);
                var voucherAssign = _voucherservice.GetVoucherDetailsByuserId(voucher.VoucherID);
                if (voucherAssign == null)
                {
                    TempData["ErrorMessage"] = "Invalid voucher number";
                   // RedirectToAction("UpdateProfile");
                    //return View();
                }
                else
                {
                    if (voucherAssign.UserID != model.LoginId)
                    {
                        TempData["ErrorMessage"] = "Invalid voucher number";
                        //RedirectToAction("UpdateProfile");
                       // return View();
                    }
                    else if (voucher.Status == (int)VoucherStatus.Used)
                    {
                        TempData["ErrorMessage"] = "Voucher number already used";
                        return View();
                    }
                    else if (Convert.ToDateTime(voucher.ExpiryDate).Date < DateTime.Now.Date)
                    {
                        TempData["ErrorMessage"] = "Voucher number has expired";
                        return View();
                    }
                    else
                    {
                        var doctorMembership = _doctorMembershipService.GetDoctorMembershipByDoctorId(model.MemberId);

                        if (doctorMembership != null)
                        {
                            if (Convert.ToDateTime(doctorMembership.EndDate) > DateTime.Now.Date)
                            {
                                TempData["ErrorMessage"] = "Your membership is not expired yet";
                                return View();
                            }
                            else
                            {
                                //Renew
                            }
                        }
                        else
                        {
                            doctorMembership = new DoctorMembership();
                            doctorMembership.DoctorId = model.MemberId;
                            doctorMembership.StartDate = (DateTime)model.membership.StartDate;
                            doctorMembership.EndDate = doctorMembership.StartDate.AddMonths(voucher.ValidityMonth);
                            doctorMembership.MembershipPaidBy = (int)MembershipTakenBy.Voucher;
                            _doctorMembershipService.Add(doctorMembership);
                            var memberShipVoucher = new MemberShipVoucher();
                            memberShipVoucher.DoctorMembershipId = doctorMembership.DoctorMembershipId;
                            memberShipVoucher.VoucherID = voucher.VoucherID;
                            _membeshipVoucherService.Add(memberShipVoucher);
                            voucher.Status = (int)VoucherStatus.Used;
                            _voucherservice.Update(voucher);
                            return RedirectToAction("index", "Home", new { area = "" });
                        }
                    }
                }
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(RegisterViewModel model, HttpPostedFileBase ProfilePic, HttpPostedFileBase Signaturepic)
        {

            if (Signaturepic != null && Signaturepic.ContentLength > 0)
            {
                UploadSignature(model, Signaturepic);
            }
            else
            {
                model.SignaturePic = model.SignaturePic;
            }

            if (ProfilePic != null)
            {
                _loginHelper.UploadImage(model, ProfilePic);
                var userid = _userService.Find(model.LoginId);
                userid.Avatar = model.ProfilePic;
                _userService.Update(userid);
            }

            MyOnlineClinic.Entity.Doctor profile = null;
            profile = model.AdminEditDoctor(model);
            _doctorservice.Update(profile);
            BindDropDown(model);
            return Redirect(Request.UrlReferrer.ToString());
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
        public IEnumerable<SelectListItem> CurrencyCode
        {
            get
            {
                return new[]
            {
                 new SelectListItem { Value = "0", Text = "--Select Country Code--" },
                new SelectListItem { Value = "1", Text = "AUD" },
                new SelectListItem { Value = "2", Text = "EUR" },
                new SelectListItem { Value = "3", Text = "INR" },
                new SelectListItem { Value = "4", Text = "SGD" },
                new SelectListItem { Value = "5", Text = "EUR" },
                new SelectListItem { Value = "6", Text = "GBP" },
               
            };
            }

        }
        public RegisterViewModel BindDropDown(RegisterViewModel model)
        {
            var countrylist = _countryService.GetCountryList();
            for (int i = 0; i < countrylist.Count; i++)
            {
                countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
            }
            model.CountryList = countrylist;
            var Citylist = _cityService.GetCityList();
            for (int i = 0; i < Citylist.Count; i++)
            {
                Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
            }
            model.CityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();

            var Statelist = _stateservice.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.StateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            var genderlist = _genderService.GetGenderList().Where(x => x.Type == (int)RoleType.Doctor).ToList();
            for (int i = 0; i < genderlist.Count; i++)
            {
                genderlist[i].GenderName = StringCipher.Decrypt(genderlist[i].GenderName);
            }
            model.GenderList = genderlist;
            var titlelist = _TitleServices.GetTitleList().Where(x => x.Type == (int)RoleType.Doctor).ToList();
            for (int i = 0; i < titlelist.Count; i++)
            {
                titlelist[i].TitleName = StringCipher.Decrypt(titlelist[i].TitleName);
            }
            model.TitleList = titlelist;
            model.ProfessionSubType = model.MemberId > 0 ? _professiontypeservice.GetProfessionSubTypesList() : new List<ProfessionSubType>();

            ViewBag.CurrencyCode = CurrencyCode;
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            ViewBag.Clinic = new List<SelectListItem>();
            model.OrganizationList = _organizationservice.GetOrganizationList();
            for (int i = 0; i < model.OrganizationList.Count; i++)
            {
                model.OrganizationList[i].OrganizationName = StringCipher.Decrypt(model.OrganizationList[i].OrganizationName);
            }
            model.ProfessionTypes = _professiontypeservice.GetProfessionTypesList();
            if (model.MemberId > 0)
            {
                model.clinicmodellist = _clinicservice.GetClinicList().Select(y => new DoctorClinicModel { ClinicId = y.ClinicId, ClinicName = y.ClinicName, Checked = false }).ToList();
                model.clinicProviderlist = model.clinicmodellist;
                var fh = _userService.GetClinicList(model.MemberId, Convert.ToInt32(RoleType.Doctor)).ToList();
                for (int i = 0; i < model.clinicmodellist.Count; i++)
                {
                    foreach (var item in fh)
                    {
                        if (Convert.ToInt32(item.ClinicId) == model.clinicmodellist[i].ClinicId)
                        {
                            model.clinicmodellist[i].Checked = true;
                            model.clinicProviderlist[i].ClinicVoucherNo = StringCipher.Decrypt(item.ClinicProviderNo);
                        }
                    }
                }
                List<DoctorConsultFee> listDoctorConsultFee = new List<DoctorConsultFee>();
                listDoctorConsultFee.Add(new DoctorConsultFee { VeryLongFee = null });
                var telemedicine = entitiesDb.DoctorConsult.Where(x => x.DoctorId == model.MemberId && x.AppointmentType == (int)AppointmentType.VideoConsult).ToList();
                if (telemedicine.Count == 0)
                {
                    model.ClinicVisitFeeList = listDoctorConsultFee;
                }
                else
                {
                    model.ClinicVisitFeeList = telemedicine;
                }
                var ClinicVisit = entitiesDb.DoctorConsult.Where(x => x.DoctorId == model.MemberId && x.AppointmentType == (int)AppointmentType.ClinicVisit).ToList();
                if (ClinicVisit.Count == 0)
                {
                    model.TelemedicineFeeList = listDoctorConsultFee;
                }
                else
                {
                    model.TelemedicineFeeList = ClinicVisit;
                }

            }
            else
            {
                model.clinicmodellist = _clinicservice.GetClinicList().Select(y => new DoctorClinicModel { ClinicId = y.ClinicId, ClinicName = y.ClinicName, Checked = false }).ToList();
                model.clinicProviderlist = model.clinicmodellist;
                List<DoctorConsultFee> listDoctorConsultFee = new List<DoctorConsultFee>();
                listDoctorConsultFee.Add(new DoctorConsultFee { VeryLongFee = null });
                model.ClinicVisitFeeList = listDoctorConsultFee;
                model.TelemedicineFeeList = listDoctorConsultFee;
            }
            return model;
        }
        [HttpPost]
        public JsonResult ConsultFee(RegisterViewModel model)
        {
            DoctorConsultFee consult = new DoctorConsultFee();
            var fee = _doctorservice.GetConsultFeeById(model.MemberId).FirstOrDefault();
            if (fee != null)
            {
                DoctorConsultFee feeDelete = entitiesDb.DoctorConsult.Where(s => s.DoctorId == model.MemberId && s.AppointmentType == (int)AppointmentType.VideoConsult).FirstOrDefault<DoctorConsultFee>();
                if (feeDelete != null)
                {
                    entitiesDb.Entry(feeDelete).State = System.Data.Entity.EntityState.Deleted;
                    entitiesDb.SaveChanges();

                    if (model.TMAStatdardFee != null || model.TMALongFee != null || model.TMAVeryLongFee != null)
                    {

                        consult.DoctorId = model.MemberId;
                        consult.StatdardFee = model.TMAStatdardFee;
                        consult.LongFee = model.TMALongFee;
                        consult.VeryLongFee = model.TMAVeryLongFee;
                        consult.CurrencyType = model.TMACurrencyType;
                        consult.AppointmentType = (int)AppointmentType.VideoConsult;
                        _doctorservice.AddConsultfee(consult);
                    }
                }
                DoctorConsultFee feetoDelete = entitiesDb.DoctorConsult.Where(s => s.DoctorId == model.MemberId && s.AppointmentType == (int)AppointmentType.ClinicVisit).FirstOrDefault<DoctorConsultFee>();
                if (feetoDelete != null)
                {
                    entitiesDb.Entry(feetoDelete).State = System.Data.Entity.EntityState.Deleted;
                    entitiesDb.SaveChanges();

                    if (model.ClinicLongFee != null || model.ClinicVeryLongFee != null)
                    {
                        consult.DoctorId = model.MemberId;
                        consult.LongFee = model.ClinicLongFee;
                        consult.VeryLongFee = model.ClinicVeryLongFee;
                        consult.CurrencyType = model.TMACurrencyType;
                        consult.AppointmentType = (int)AppointmentType.ClinicVisit;
                        _doctorservice.AddConsultfee(consult);
                    }
                }
                DoctorConsultFee feetoDeletes = entitiesDb.DoctorConsult.Where(s => s.DoctorId == model.MemberId && s.AppointmentType == (int)AppointmentType.RepeatPrescription).FirstOrDefault<DoctorConsultFee>();
                if (feetoDeletes != null)
                {
                    entitiesDb.Entry(feetoDeletes).State = System.Data.Entity.EntityState.Deleted;
                    entitiesDb.SaveChanges();
                    if (model.tmaRepeatPrescription != null)
                    {
                        consult.DoctorId = model.MemberId;
                        consult.RepeatPrescription = model.tmaRepeatPrescription;
                        consult.CurrencyType = model.TMACurrencyType;
                        consult.AppointmentType = (int)AppointmentType.RepeatPrescription;
                        _doctorservice.AddConsultfee(consult);

                    }
                }

            }
            else
            {


                if (model.TMAStatdardFee != null || model.TMALongFee != null || model.TMAVeryLongFee != null)
                {

                    consult.DoctorId = model.MemberId;
                    consult.StatdardFee = model.TMAStatdardFee;
                    consult.LongFee = model.TMALongFee;
                    consult.VeryLongFee = model.TMAVeryLongFee;
                    consult.CurrencyType = model.TMACurrencyType;
                    consult.AppointmentType = (int)AppointmentType.VideoConsult;
                    _doctorservice.AddConsultfee(consult);
                }
                if (model.ClinicLongFee != null || model.ClinicVeryLongFee != null)
                {
                    consult.DoctorId = model.MemberId;
                    consult.LongFee = model.ClinicLongFee;
                    consult.VeryLongFee = model.ClinicVeryLongFee;
                    consult.CurrencyType = model.TMACurrencyType;
                    consult.AppointmentType = (int)AppointmentType.ClinicVisit;
                    _doctorservice.AddConsultfee(consult);
                }
                if (model.tmaRepeatPrescription != null)
                {
                    consult.DoctorId = model.MemberId;
                    consult.RepeatPrescription = model.tmaRepeatPrescription;
                    consult.CurrencyType = model.TMACurrencyType;
                    consult.AppointmentType = (int)AppointmentType.RepeatPrescription;
                    _doctorservice.AddConsultfee(consult);

                }

            }




            return Json(consult, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult AccountDetails(RegisterViewModel model)
        {
            var Account = _doctorservice.GetDoctorAccountDetails(model.LoginId).FirstOrDefault();
            if (Account != null)
            {
                Account.AccountName = model.AccountName;
                Account.BankName = model.BankName;
                Account.Bsb = model.Bsb;
                Account.AccountNumber = model.AccountNumber;
                Account.DoctorLoginId = model.LoginId;
                Account.PaypelEmailId = model.PaypelEmailId;
                _doctorservice.Update(Account);
            }
            else
            {
                DoctorAccountDetails Acc = new DoctorAccountDetails();
                Acc.AccountName = model.AccountName;
                Acc.BankName = model.BankName;
                Acc.Bsb = model.Bsb;
                Acc.AccountNumber = model.AccountNumber;
                Acc.DoctorLoginId = model.LoginId;
                Acc.PaypelEmailId = model.PaypelEmailId;
                _doctorservice.Add(Acc);
            }
            return Json("", JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAccountDetails(Guid LoginId)
        {
            var Account = _doctorservice.GetDoctorAccountDetails(LoginId);
            return Json(Account, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetConsultFee(int id)
        {
            var vedio = _doctorservice.GetConsultFeeById(id).Where(x=>x.AppointmentType==(int)AppointmentType.VideoConsult);
            var Clinic = _doctorservice.GetConsultFeeById(id).Where(x => x.AppointmentType == (int)AppointmentType.ClinicVisit);
            var a = vedio.Union(Clinic);
            return Json(a, JsonRequestBehavior.AllowGet);

        }


    }
}