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
    public class DeviceViewModel
    {
        [Required(ErrorMessage = "Please enter device name")]
        public string DeviceName { get; set; }

        [Required(ErrorMessage = "Please enter device number")]
        public string DeviceNumber { get; set; }

        public int PatientDeviceId { get; set; }

        [Required(ErrorMessage = "Please enter device details")]
        public string DeviceDetails { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string DeviceId { get; set; }
        public string PatientId { get; set; }
        public DateTime AssigenDate { get; set; }
        public int AssignDeviceId { get; set; }
        public int AssignPatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientSurName { get; set; }
        public string PatientEmail { get; set; }
        public string AssigendDeviceName { get; set; }
        [Required(ErrorMessage = "Please enter device image")]
        public string ProfilePic { get; set; }
        public string deviceLogo { get; set; }
        public string AssgienddeviceNumber { get; set; }

        public List<Patient> PatientList { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DeviceViewModel()
        {

        }
        public DeviceViewModel(DeviceViewModel Model)
        {
            this.DeviceName = DeviceName;
            this.DeviceNumber = DeviceNumber;
            this.PatientDeviceId = PatientDeviceId;
            this.DeviceDetails = DeviceDetails;
            this.ProfilePic = ProfilePic;
            this.CreatedDate = CreatedDate;
            this.Name = Name;
            this.Email = Email;
        }
        public PatientDevice AddDeviceData(PatientDevice device)
        {

            device.DeviceName = StringCipher.Encrypt(DeviceName);
            device.DeviceNumber = StringCipher.Encrypt(DeviceNumber);
            device.DeviceDetails = StringCipher.Encrypt(DeviceDetails);
            device.DeviceImage = ProfilePic;

            return device;
        }

        public PatientDevice EditDeviceData(DeviceViewModel model)
        {
            var rModel = new PatientDevice
          {
              PatientDeviceId = model.PatientDeviceId,
              DeviceName = StringCipher.Encrypt(model.DeviceName),
              DeviceNumber = StringCipher.Encrypt(model.DeviceNumber),
              DeviceDetails = StringCipher.Encrypt(model.DeviceDetails)
          };
            return rModel;
        }


    }
}