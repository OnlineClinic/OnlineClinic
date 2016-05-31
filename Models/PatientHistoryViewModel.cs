using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class PatientHistoryViewModel
    {
        public int MemberId { get; set; }
        public Guid LoginId { get; set; }
        public int PatientHistoryId { get; set; }
        // this is for Allergy
         [Required(ErrorMessage = "Please enter Allergy name")]
        public string AllergyName { get; set; }
         [Required(ErrorMessage = "Please enter What Happens")]
        public string WhatHappens { get; set; }
        public IEnumerable<PatientAllergies> AllergiesList { get; set; }
        public IEnumerable<PatientAllergies> _PatientAllergies { get; set; }
        public PatientAllergies GetPatientAlleries(PatientHistoryViewModel model)
        {
            PatientAllergies patientAllergies = new PatientAllergies
            {
                AllergyName =StringCipher.Encrypt(model.AllergyName),
                WhatHappens = StringCipher.Encrypt(model.WhatHappens),
                LoginId =model.LoginId,
                Id = model.PatientHistoryId,
            };
            return patientAllergies;
        }
        //  this is for Medication History
        public string MedicationHistoryName { get; set; }
        public string MedicationHistoryStrength { get; set; }
        public string MedicationHistoryDose { get; set; }
        public int MedicationHistoryFrequency { get; set; }
        public string MedicationHistoryYearStarted { get; set; }
        public IEnumerable<PatientMedication> MedicationHistoryList { get; set; }
        public PatientMedication GetPatientMedication(PatientHistoryViewModel model)
        {
            PatientMedication patientMedication = new PatientMedication
            {
                Name = StringCipher.Encrypt(model.MedicationHistoryName),
                Strength= model.MedicationHistoryStrength,
                Dose = model.MedicationHistoryDose,
                Frequency= Convert.ToInt32(model.MedicationHistoryFrequency),
                YearStaeted=model.MedicationHistoryYearStarted,
                Id=model.PatientHistoryId,
                LoginId = model.LoginId
            };
            return patientMedication;
        }
        //  this is for Past Medical
        public string PastMedicalCondition { get; set; }
        public string PastMedicalYear { get; set; }
        public IEnumerable<PatientPastMedical> PastMedicalList { get; set; }
        public IEnumerable<PatientAllergies> _PatientPastMedical { get; set; }
        public PatientPastMedical GetPatientPastMedical(PatientHistoryViewModel model)
        {
            PatientPastMedical patientPastMedical = new PatientPastMedical
            {
                ConditionName = StringCipher.Encrypt(model.PastMedicalCondition),
                DiagnosedYear = model.PastMedicalYear,
                Id = model.PatientHistoryId,
                LoginId = model.LoginId
            };
            return patientPastMedical;
        }
        //  this is for Smoking
        public string SmokingDay { get; set; }
        public string SmokingYear { get; set; }
        public IEnumerable<PatientSmoking> SmokingList { get; set; }
        public IEnumerable<PatientSmoking> _PatientSmoking { get; set; }
        public PatientSmoking GetPatientSmoking(PatientHistoryViewModel model)
        {
            PatientSmoking patientSmoking = new PatientSmoking
            {
                HowMany = StringCipher.Encrypt(model.SmokingDay),
                YearStarted= model.SmokingYear,
                Id = model.PatientHistoryId,
                LoginId = model.LoginId
            };
            return patientSmoking;
        }
        //  this is for Alcohol
        public string AlcoholDayWeek { get; set; }
        public string AlcoholDrinkDay { get; set; }
        public IEnumerable<PatientAlcohal> AlcoholList { get; set; }
        public IEnumerable<PatientAlcohal> _PatientAlcohal { get; set; }
        public PatientAlcohal GetPatientAlcohal(PatientHistoryViewModel model)
        {
            PatientAlcohal patientSmoking = new PatientAlcohal
            {
                InWeek = StringCipher.Encrypt(model.AlcoholDayWeek),
                InDay = StringCipher.Encrypt(model.AlcoholDrinkDay),
                LoginId = model.LoginId,
                Id = model.PatientHistoryId
            };
            return patientSmoking;
        }
        //  this is for Family History
        public string FamilyHistoryCondition { get; set; }
        public string FamilyHistoryFamily { get; set; }
        public IEnumerable<PatientFamily> FamilyHistoryList { get; set; }
        public IEnumerable<PatientFamily> _PatientFamilyHistory { get; set; }
        
        public PatientFamily GetPatientFamilyHistory(PatientHistoryViewModel model)
        {
            PatientFamily patientFamily = new PatientFamily
            {
                ConditionName = StringCipher.Encrypt(model.FamilyHistoryCondition),
                FamilyMember = StringCipher.Encrypt(model.FamilyHistoryFamily),
                Id = model.PatientHistoryId,
                LoginId = model.LoginId
            };
            return patientFamily;
        }
        //  this is for Employment
        public string EmploymentName { get; set; }
        public DateTime EmploymentDate { get; set; }
        public IEnumerable<PatientEmployment> EmploymentList { get; set; }
        public IEnumerable<PatientEmployment> _PatientEmployment { get; set; }
        public PatientEmployment GetPatientEmployment(PatientHistoryViewModel model)
        {
            PatientEmployment patientEmployment = new PatientEmployment
            {
                Employment = StringCipher.Encrypt(model.EmploymentName),
                EmploymentDate = DateTime.Now,
                Id = model.PatientHistoryId,
                LoginId = model.LoginId
            };
            return patientEmployment;
        }
        public List<PatientHistoryViewModel> All { get; set; }
        ////  this is for Other Medical Records
        public string OtherMedicalRecordsSubject { get; set; }
        public string OtherMedicalRecordsFilename { get; set; }
        public IEnumerable<PatientMedicalRecord> OtherMedicalRecordsList { get; set; }
        public IEnumerable<PatientMedicalRecord> _PatientMedicalRecord { get; set; }
        public PatientMedicalRecord GetPatientMedicalRecord(PatientHistoryViewModel model)
        {
            PatientMedicalRecord patientMedicalRecord = new PatientMedicalRecord
            {
                Subject = model.OtherMedicalRecordsSubject,
                FileName= OtherMedicalRecordsFilename,
                LoginId = model.LoginId,
                Id = model.PatientHistoryId,
            };
            return patientMedicalRecord;
        }
     
    }
}
//
 //model.AllergiesList = Allergieslist;
 //           model.MedicationHistoryList = MedicationHistorylist;
 //           model.PastMedicalList = PastMedicallist;
 //           model.SmokingList = Smokinglist;
 //           model.AlcoholList = Alcohollist;
 //           model.FamilyHistoryList = FamilyHistorylist;
 //           model.EmploymentList = Employmentlist;
 //           model.OtherMedicalRecordsList = OtherMedicalRecords;