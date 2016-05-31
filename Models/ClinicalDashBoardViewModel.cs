using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class ClinicalDashBoardViewModel
    {
        public Guid DoctorId { get; set; }

        public Guid PatientId { get; set; }

        public decimal BloodPressureSystolicMinValue { get; set; }

        public decimal BloodPressureSystolicMaxValue { get; set; }

        public decimal BloodPressureDiastolicMinValue { get; set; }

        public decimal BloodPressureDiastolicMaxValue { get; set; }

        public string BloodPressureUnit { get; set; }

        public decimal PulseRateMin { get; set; }

        public decimal PulseRateMax { get; set; }

        public string PulseUnit { get; set; }

        public decimal BloodGlucoseBeforeBreakfastMinValue { get; set; }

        public decimal BloodGlucoseBeforeBreakfastMaxValue { get; set; }

        public decimal BloodGlucoseAfterBreakfastMinValue { get; set; }

        public decimal BloodGlucoseAfterBreakfastMaxValue { get; set; }

        public decimal BloodGlucoseBeforeLunchMinValue { get; set; }

        public decimal BloodGlucoseBeforeLunchMaxValue { get; set; }

        public decimal BloodGlucoseAfterLunchMinValue { get; set; }

        public decimal BloodGlucoseAfterLunchMaxValue { get; set; }

        public decimal BloodGlucoseBeforeDinnerMinValue { get; set; }

        public decimal BloodGlucoseBeforeDinnerMaxValue { get; set; }

        public decimal BloodGlucoseBeforeBedMinValue { get; set; }

        public decimal BloodGlucoseBeforeBedMaxValue { get; set; }

        public string BloodGlucoseUnit { get; set; }

        public decimal TemperatureMinValue { get; set; }

        public decimal TemperatureMaxValue { get; set; }

        public string TemperatureUnit { get; set; }

        public decimal BloodOxygenMinValue { get; set; }

        public decimal BloodOxygenMaxValue { get; set; }

        public decimal BloodOxygenUnit { get; set; }

        public decimal HeightMinValue { get; set; }

        public decimal HeightMaxValue { get; set; }

        public string HeightUnit { get; set; }

        public decimal WeightMinValue { get; set; }

        public decimal WeightMaxValue { get; set; }

        public string WeightUnit { get; set; }

        public decimal BMIMinValue { get; set; }

        public string BMIMaxValue { get; set; }

        //Pathology
        public int AppointmentId { get; set; }

        public string TestName { get; set; }

        public string FastingNonFastingValue { get; set; }

        public string ClinicalNotes { get; set; }

        public string InstructionForPatient { get; set; }

        public string RequestNo { get; set; }

        public string Urgent { get; set; }

        public string CopyResultTo { get; set; }

        public bool IsCompleted { get; set; }

        public bool PrintToAdmin { get; set; }

        public bool IsSentToPatient { get; set; }
        public int PathologyId { get; set; }
        //


        //Heart Rate BP
        public decimal SystolicValue { get; set; }

        public decimal DiastolicValue { get; set; }

        public decimal PulseValue { get; set; }

        public string MeasureDate { get; set; }

        public string MeasureTime { get; set; }

        public int ReadingType { get; set; }

        public string Period { get; set; }
        //


        //ecg and throat image
        public string EcgThroatFile { get; set; }
        public string SkinThroatFile { get; set; }
        public string Subject { get; set; }

        //New Consultation

        public string ReasonForContact { get; set; }

        public int DiagnosisId { get; set; }

        public string Subjective { get; set; }

        public string Objective { get; set; }

        public string Assessment { get; set; }

        public string Plans { get; set; }

        public decimal BloodSugarValue { get; set; }

        public int MealBeforeAfter { get; set; }

        public int MealTime { get; set; }

        public decimal OxygenValue { get; set; }

        public decimal Height { get; set; }

        public decimal Temperature { get; set; }

        public decimal Waist { get; set; }

        public decimal Weight { get; set; }    

        public SetStandard InsertStandards(ClinicalDashBoardViewModel model)
        {
            var set = new SetStandard();
            set.DoctorId = model.DoctorId;
            set.PatientId = model.PatientId;
            set.BloodPressureSystolicMinValue = model.BloodPressureSystolicMinValue;
            set.BloodPressureSystolicMaxValue = model.BloodPressureSystolicMaxValue;
            set.BloodPressureDiastolicMinValue = model.BloodPressureDiastolicMinValue;
            set.BloodPressureDiastolicMaxValue = model.BloodPressureDiastolicMaxValue;
            set.BloodPressureUnit = model.BloodPressureUnit;
            set.PulseRateMin = model.PulseRateMin;
            set.PulseRateMax = model.PulseRateMax;
            set.PulseUnit = model.PulseUnit;
            set.BloodGlucoseBeforeBreakfastMinValue = model.BloodGlucoseBeforeBreakfastMinValue;
            set.BloodGlucoseBeforeBreakfastMaxValue = model.BloodGlucoseBeforeBreakfastMaxValue;
            set.BloodGlucoseAfterBreakfastMinValue = model.BloodGlucoseAfterBreakfastMinValue;
            set.BloodGlucoseAfterBreakfastMaxValue = model.BloodGlucoseAfterBreakfastMaxValue;
            set.BloodGlucoseBeforeLunchMinValue = model.BloodGlucoseBeforeLunchMinValue;
            set.BloodGlucoseBeforeLunchMaxValue = model.BloodGlucoseBeforeLunchMaxValue;
            set.BloodGlucoseAfterLunchMinValue = model.BloodGlucoseAfterLunchMinValue;
            set.BloodGlucoseAfterLunchMaxValue = model.BloodGlucoseAfterLunchMaxValue;
            set.BloodGlucoseBeforeDinnerMinValue = model.BloodGlucoseBeforeDinnerMinValue;
            set.BloodGlucoseBeforeDinnerMaxValue = model.BloodGlucoseBeforeDinnerMaxValue;
            set.BloodGlucoseBeforeBedMinValue = model.BloodGlucoseBeforeBedMinValue;
            set.BloodGlucoseBeforeBedMaxValue = model.BloodGlucoseBeforeBedMaxValue;
            set.BloodGlucoseUnit = model.BloodGlucoseUnit;
            set.TemperatureMinValue = model.TemperatureMinValue;
            set.TemperatureMaxValue = model.TemperatureMaxValue;
            set.TemperatureUnit = model.TemperatureUnit;
            set.BloodOxygenMinValue = model.BloodOxygenMinValue;
            set.BloodOxygenMaxValue = model.BloodOxygenMaxValue;
            //set.BloodOxygenUnit = model.BloodOxygenUnit;
            //set.BMIUnit= model.WeightUnit;
            //set.BMIMinValue = model.BMIMinValue;
            //set.BMIMaxValue = model.BMIMaxValue;
            return set;
        }

        public Pathology InsertPathology(ClinicalDashBoardViewModel model)
        {
            Pathology set = new Pathology();
            set.ClinicalNotes =StringCipher.Encrypt( model.ClinicalNotes);
            set.FastingNonFastingValue = StringCipher.Encrypt(model.FastingNonFastingValue);
            set.InstructionForPatient = StringCipher.Encrypt(model.InstructionForPatient);
            set.TestName =StringCipher.Encrypt( model.TestName);
            set.CopyResultTo = StringCipher.Encrypt(model.CopyResultTo);
            set.Urgent = StringCipher.Encrypt(model.Urgent);
            set.PatientLoginId = model.PatientId;
            set.DoctorLoginId = model.DoctorId;
            return set;

        }
    }
}