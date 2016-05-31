using MyOnlineClinic.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Web.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Drawing;
using System.Net;
using MyOnlineClinic.Emailer;
using System.Configuration;
using System.Globalization;
namespace MyOnlineClinic.Web.Areas.Patients.Controllers
{
    public class PatientHistoryController : BaseController
    {
        private readonly IPatientHistoryService _patientHistoryService;
        private readonly ILoginHelper _loginHelper;
        public PatientHistoryController(IPatientHistoryService patientHistoryService, ILoginHelper loginHelper)
        {
            _patientHistoryService = patientHistoryService;
            _loginHelper = loginHelper;
        }
        //
        // GET: /Patients/PatientHistory/
        public ActionResult Index()
        {
            PatientHistoryViewModel model = new PatientHistoryViewModel();
            GetallHistory(model);
            return View(model);
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