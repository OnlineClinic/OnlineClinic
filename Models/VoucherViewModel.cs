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
    public class VoucherViewModel
    {
        public int VoucherID { get; set; }
        [Required(ErrorMessage = "Please enter Length of Voucher No")]
        [MaxLength(50, ErrorMessage = "Length cannot be longer than 50.")]
        public string VoucherNo { get; set; }
        public int? Status { get; set; }
        public Guid GeneratedBy { get; set; }
        public DateTime? GeneratedDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? AssignDate { get; set; }
        public string Assignto { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string voucherstatus { get; set; }
        public Guid UserID { get; set; }
        public string AvailTime { get; set; }
        public int RoleType { get; set; }
        public int ValidityMonth { get; set; }
        public int ExpiryMonth { get; set; }
        public int IsExpired { get; set; }
        public bool IsAssigned { get; set; }
        public List<VoucherViewModel> voucherList { get; set; }
        public VoucherViewModel()
        {
            IsAssigned = false;
        }
        public VoucherViewModel(VoucherViewModel voucher)
        {
            this.VoucherID = voucher.VoucherID;
            this.VoucherNo = voucher.VoucherNo;
            this.Status = voucher.Status;
            this.GeneratedBy = voucher.GeneratedBy;
            this.GeneratedDate = voucher.GeneratedDate;
            this.Assignto = voucher.Assignto;
            this.ValidityMonth = voucher.ValidityMonth;
            this.UserID = voucher.UserID;
            this.ExpiryMonth = voucher.ExpiryMonth;
            this.RoleType = voucher.RoleType;
        }
        public Voucher GetEditModel(Voucher Vouchermodel)
        {
            Vouchermodel.VoucherNo = this.VoucherNo;
           // Vouchermodel.Status = this.Status;
            return Vouchermodel;
        }
        
        
    }
}