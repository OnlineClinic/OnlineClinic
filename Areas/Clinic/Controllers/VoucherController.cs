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
namespace MyOnlineClinic.Web.Areas.Clinic.Controllers
{
    public class VoucherController : BaseController
    {
        //
        // GET: /Admin/Voucher/
        IVoucherService _voucherService;
        IDoctorService _doctorService;
        IUserService _userService;
        public VoucherController(IVoucherService voucherservice, IDoctorService doctorService, IUserService userservice)
        {

            _voucherService = voucherservice;
            _doctorService = doctorService;
            _userService = userservice;
        }
        [HttpGet]
        public ActionResult Add(int? id)
        {
            // VoucherViewModel model = new VoucherViewModel();
            // return View(model);
            VoucherViewModel model = new VoucherViewModel();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Add(VoucherViewModel model)
        {

            try
            {
                if (ModelState.IsValid && Convert.ToInt32(model.VoucherNo) <= 50)
                {
                    int vouchercount = Convert.ToInt32(model.VoucherNo);


                    for (int i = 0; i < vouchercount; i++)
                    {
                        string txt = Guid.NewGuid().ToString().Substring(0, 4);
                        if (_voucherService.getVoucherNo(txt) == false)
                        {
                            Voucher cModel = new Voucher();
                            cModel.VoucherNo = StringCipher.Encrypt(txt);
                            cModel.Status = 0;
                            cModel.CreatedBy = base.loginUserModel.LoginId;
                            cModel.CreatedDate = DateTime.Now;
                            cModel.ValidityMonth = 0;
                            cModel.ExpiryDate = DateTime.Now;
                            _voucherService.Add(cModel);
                        }
                    }
                    return RedirectToAction("VoucherAssign");
                }
                else
                {
                    ModelState.AddModelError("VoucherNo", "Max Input length is 50 only ! Try again.");
                    //ViewBag.Errormessage = "Input Length is Greater then 50";
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
            //   VoucherViewModel model = new VoucherViewModel();
            // return View(model);
        }
        public IEnumerable<SelectListItem> AssignerUser
        {
            
            get
            {
                return new[]
            {
                 new SelectListItem { Value = "0", Text = " Select " },
                new SelectListItem { Value = RoleType.Doctor.ToString(), Text = "Doctor" },
                new SelectListItem { Value = RoleType.Patient.ToString(), Text = "Patient" },
               
               
            };
            }

        }
        //string Fromdate,string ToDate,int? Userid,string LoginName,string voucherstatus
        public ActionResult VoucherAssign()
        {

            ViewBag.AssignedUser = AssignerUser;

            var Vouchermodel = _voucherService.GetVoucherList().Select(x => new VoucherViewModel { VoucherNo = x.VoucherNo, VoucherID = x.VoucherID, Status = x.Status, GeneratedDate = x.CreatedDate, GeneratedBy = (Guid)x.CreatedBy, UserName = "" }).ToList().OrderByDescending(tbl => tbl.GeneratedDate).ToList();
            for (int i = 0; i < Vouchermodel.Count; i++)
            {
                var users = _voucherService.GetVoucherAssignById(Vouchermodel[i].VoucherID);
                int roleid = _userService.Find(Vouchermodel[i].GeneratedBy).LookUpRoleId;
                Vouchermodel[i].VoucherNo = StringCipher.Decrypt(Vouchermodel[i].VoucherNo);
                Vouchermodel[i].LoginName = Generatedby(Vouchermodel, i, Vouchermodel[i].LoginName, roleid);
                Vouchermodel[i].voucherstatus = Vouchermodel[i].Status == 0 ? "Not Used" : "Used";
                if (users != null)
                {
                    var userId = users.UserID;
                    var role = users.RoleType;
                    Vouchermodel[i].UserID = userId;
                    Vouchermodel[i].RoleType = role;
                    Vouchermodel[i].AssignDate = users.CreatedDate;
                    string firstname,middlename,surname;firstname=middlename=surname = string.Empty;
                    if (role == (int)RoleType.Doctor)
                    {
                       
                         firstname = StringCipher.Decrypt(_doctorService.DoctorByLoginId(Vouchermodel[i].UserID).Select(x => x.FirstName).FirstOrDefault());
                         middlename = _doctorService.DoctorByLoginId(Vouchermodel[i].UserID).Select(x => x.MiddleName).FirstOrDefault() == null ? string.Empty : StringCipher.Decrypt(_doctorService.DoctorByLoginId(Vouchermodel[i].UserID).Select(x => x.MiddleName).FirstOrDefault());
                         surname = StringCipher.Decrypt(_doctorService.DoctorByLoginId(Vouchermodel[i].UserID).Select(x => x.SurName).FirstOrDefault());              
                    }
                    else if (role == (int)RoleType.Patient)
                    {
                        if (_userService.GetPatientByLoginId(Vouchermodel[i].UserID).FirstName != null)
                        {
                            firstname = StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).FirstName);
                            middlename = _userService.GetPatientByLoginId(Vouchermodel[i].UserID).MiddleName == null ? string.Empty : StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).MiddleName);
                            surname = StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).SurName);
                        }
                    }
                    Vouchermodel[i].UserName = firstname + "" + middlename + " " + "" + surname;
                }
            }
            //}
            return View(Vouchermodel);
        }
        [HttpPost]
        public ActionResult VoucherAssign(SearchParametersViewModel objsearchmodel)
        {
            var Vouchermodel = _voucherService.GetVoucherList().Select(x => new VoucherViewModel { VoucherNo = x.VoucherNo, VoucherID = x.VoucherID, Status = x.Status, GeneratedDate = x.CreatedDate, GeneratedBy = (Guid)x.CreatedBy, UserName = "" }).ToList().OrderByDescending(tbl => tbl.GeneratedDate).ToList();
            for (int i = 0; i < Vouchermodel.Count; i++)
            {
                var users = _voucherService.GetVoucherAssignById(Vouchermodel[i].VoucherID);
                int roleid = _userService.Find(Vouchermodel[i].GeneratedBy).LookUpRoleId;
                Vouchermodel[i].LoginName = Generatedby(Vouchermodel, i, Vouchermodel[i].LoginName, roleid);
                Vouchermodel[i].voucherstatus = Vouchermodel[i].Status == 0 ? "Not Used" : "Used";
                Vouchermodel[i].VoucherNo = StringCipher.Decrypt(Vouchermodel[i].VoucherNo);
                if (users != null)
                {
                    var userId = users.UserID;
                    var role = users.RoleType;
                    Vouchermodel[i].UserID = userId;
                    Vouchermodel[i].RoleType = role;
                    Vouchermodel[i].AssignDate = users.CreatedDate;
                  

                    if (role == (int)RoleType.Doctor)
                    {
                        Vouchermodel[i].UserName = _doctorService.DoctorByLoginId(Vouchermodel[i].UserID).FirstOrDefault().MiddleName == null ? StringCipher.Decrypt(_doctorService.DoctorByLoginId(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault()) : StringCipher.Decrypt(_doctorService.DoctorByLoginId(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.MiddleName + " " + x.SurName).FirstOrDefault());
                    }
                    else if (role == (int)RoleType.Patient)
                    {
                        Vouchermodel[i].UserName = _userService.GetPatientByLoginId(Vouchermodel[i].UserID).MiddleName == null ?

                             StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).FirstName) + " " + StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).SurName) :
                            //StringCipher.Decrypt(_userService.GetPatientById(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault()) :
                            StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).FirstName) + " " + StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).MiddleName) + StringCipher.Decrypt(_userService.GetPatientByLoginId(Vouchermodel[i].UserID).SurName);
                    }

                }
            }
            Vouchermodel = SearchMethod(objsearchmodel, Vouchermodel);
            return View(Vouchermodel);
        }
        private string Generatedby(List<VoucherViewModel> Vouchermodel, int i, string fullname, int roleid)
        {
            if (roleid == (int)RoleType.ADMIN)
            {
                fullname = "MocAdmin";
                //fullname = _userService.GetAdminUsers().Where(x => x.LoginId == Vouchermodel[i].GeneratedBy).Select(z => z.FirstName + " " + z.SurName).FirstOrDefault();
            }
            else if (roleid == (int)RoleType.Doctor)
            {
                //fullname = _doctorService.GetDoctorById(Vouchermodel[i].UserID).FirstOrDefault().MiddleName == null ? _doctorService.GetDoctorById(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault() : _doctorService.GetDoctorById(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.MiddleName + " " + x.SurName).FirstOrDefault();
            }
            else if (roleid == (int)RoleType.Patient)
            {
                //fullname = _userService.GetPatientList().Where(x => x.LoginId == Vouchermodel[i].GeneratedBy).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault();
            }
            else
                if (roleid == (int)RoleType.OrganizationAdmin)
                {
                    fullname = "OrgAdmin";
                    //fullname = entitiesDb.OrganizationUsers.Where(x => x.LoginId == Vouchermodel[i].GeneratedBy).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault();
                }
                else
                    if (roleid == (int)RoleType.ClinicAdmin)
                    {
                        //fullname = entitiesDb.ClinicUsers.Where(x => x.LoginId == Vouchermodel[i].GeneratedBy).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault();
                    }

            return fullname;
        }
        private static List<VoucherViewModel> SearchMethod(SearchParametersViewModel objsearchmodel, List<VoucherViewModel> Vouchermodel)
        {
            if (objsearchmodel.FromDate != null && objsearchmodel.ToDate != null)
            {
                objsearchmodel.FromDate = Convert.ToDateTime(objsearchmodel.ToDate).AddDays(-1);
                objsearchmodel.ToDate = Convert.ToDateTime(objsearchmodel.ToDate).AddDays(1);
            }
            // for Voucher Status
            if (!string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate == null && objsearchmodel.ToDate == null && objsearchmodel.GeneratedBy == null && objsearchmodel.UsedId == null)
            {
                Vouchermodel = Vouchermodel.Where(x => x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus)).ToList();
            }
            //Only From & To Date
            else if (string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate != null && objsearchmodel.ToDate != null && objsearchmodel.GeneratedBy == null && objsearchmodel.UsedId == null)
            {
                //Vouchermodel= Vouchermodel.Where(x=>x.GeneratedDate)
                Vouchermodel = Vouchermodel.Where(x => x.GeneratedDate >= Convert.ToDateTime(objsearchmodel.FromDate)).Where(x => x.GeneratedDate <= Convert.ToDateTime(objsearchmodel.ToDate)).ToList();
            }
            //Generated By && Voucher Status
            else if (!string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate == null && objsearchmodel.ToDate == null && objsearchmodel.GeneratedBy > 0 && objsearchmodel.UsedId == null)
            {
                if (objsearchmodel.GeneratedBy == (int)RoleType.ADMIN)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus) && x.LoginName.Contains("MocAdmin")).ToList();
                }
                else if (objsearchmodel.GeneratedBy == (int)RoleType.OrganizationAdmin)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus) && x.LoginName.Contains("OrgAdmin")).ToList();
                }
            }
            //Generated By && Voucher Status & Assigned To
            else if (!string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate == null && objsearchmodel.ToDate == null && objsearchmodel.GeneratedBy>0 && objsearchmodel.UsedId == null)
            {
                if (objsearchmodel.GeneratedBy == (int)RoleType.ADMIN)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.RoleType == objsearchmodel.UsedId && x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus) && x.LoginName.Contains("MocAdmin")).ToList();
                }
                else if (objsearchmodel.GeneratedBy == (int)RoleType.OrganizationAdmin)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.RoleType == objsearchmodel.UsedId && x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus) && x.LoginName.Contains("OrgAdmin")).ToList();
                }
            }
            //only generatedby
            else if (string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate == null && objsearchmodel.ToDate == null && objsearchmodel.GeneratedBy>0 && objsearchmodel.UsedId == null)
            {
                if (objsearchmodel.GeneratedBy == (int)RoleType.ADMIN)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.LoginName.Contains("MocAdmin")).ToList();
                }
                else if (objsearchmodel.GeneratedBy == (int)RoleType.OrganizationAdmin)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.LoginName.Contains("OrgAdmin")).ToList();
                }
            }
            //Only UserId
            else if (string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate == null && objsearchmodel.ToDate == null && objsearchmodel.GeneratedBy==null && objsearchmodel.UsedId > 0)
            {
                Vouchermodel = Vouchermodel.Where(x => x.RoleType == objsearchmodel.UsedId).ToList();
            }
            //Generated by & From Date & To Date
            else if (string.IsNullOrEmpty(objsearchmodel.Voucherstatus
                ) && objsearchmodel.FromDate != null && objsearchmodel.ToDate != null && objsearchmodel.GeneratedBy>0 && objsearchmodel.UsedId == null)
            {
                if (objsearchmodel.GeneratedBy == (int)RoleType.ADMIN)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.LoginName.Contains("MocAdmin")).Where(x => x.GeneratedDate >= Convert.ToDateTime(objsearchmodel.FromDate)).Where(x => x.GeneratedDate <= Convert.ToDateTime(objsearchmodel.ToDate)).ToList();
                }
                else if (objsearchmodel.GeneratedBy == (int)RoleType.OrganizationAdmin)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.LoginName.Contains("OrgAdmin")).Where(x => x.GeneratedDate >= Convert.ToDateTime(objsearchmodel.FromDate)).Where(x => x.GeneratedDate <= Convert.ToDateTime(objsearchmodel.ToDate)).ToList();
                }
            }
            //Assigned To & From Date & To Date
            else if (string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate != null && objsearchmodel.ToDate != null && objsearchmodel.GeneratedBy == null && objsearchmodel.UsedId > 0)
            {
                Vouchermodel = Vouchermodel.Where(x => x.RoleType == objsearchmodel.UsedId).Where(x => x.GeneratedDate >= Convert.ToDateTime(objsearchmodel.FromDate)).Where(x => x.GeneratedDate <= Convert.ToDateTime(objsearchmodel.ToDate)).ToList();
            }
            //Assigned To & Generated By
            else if (string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate == null && objsearchmodel.ToDate == null && objsearchmodel.GeneratedBy>0 && objsearchmodel.UsedId > 0)
            {

            }
            //Assigned to & Voucher Status
            else if (!string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate == null && objsearchmodel.ToDate == null && objsearchmodel.GeneratedBy == null && objsearchmodel.UsedId > 0)
            {
                Vouchermodel = Vouchermodel.Where(x => x.RoleType == objsearchmodel.UsedId && x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus)).ToList();
            }
            //All condition
            else if (!string.IsNullOrEmpty(objsearchmodel.Voucherstatus) && objsearchmodel.FromDate != null && objsearchmodel.ToDate != null && objsearchmodel.GeneratedBy>0 && objsearchmodel.UsedId > 0)
            {
                if (objsearchmodel.GeneratedBy == (int)RoleType.ADMIN)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.RoleType == objsearchmodel.UsedId && x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus) && x.LoginName.Contains("MocAdmin")).Where(x => x.GeneratedDate >= Convert.ToDateTime(objsearchmodel.FromDate)).Where(x => x.GeneratedDate <= Convert.ToDateTime(objsearchmodel.ToDate)).ToList();
                }
                else if (objsearchmodel.GeneratedBy == (int)RoleType.OrganizationAdmin)
                {
                    Vouchermodel = Vouchermodel.Where(x => x.RoleType == objsearchmodel.UsedId && x.Status == Convert.ToInt32(objsearchmodel.Voucherstatus) && x.LoginName.Contains("OrgAdmin")).Where(x => x.GeneratedDate >= Convert.ToDateTime(objsearchmodel.FromDate)).Where(x => x.GeneratedDate <= Convert.ToDateTime(objsearchmodel.ToDate)).ToList();
                }
            }

            return Vouchermodel;
        }
        //public JsonResult GetSearchlist(int? id)
        //{
        //    var Vouchermodel = _voucherService.GetVoucherList().Select(x => new VoucherViewModel { VoucherNo = x.VoucherNo, VoucherID = x.VoucherID, Status = x.Status, GeneratedDate = x.CreatedDate, GeneratedBy = (Guid)x.CreatedBy, UserName = "" }).ToList().OrderByDescending(tbl => tbl.GeneratedDate).ToList();
        //    for (int i = 0; i < Vouchermodel.Count; i++)
        //    {
        //        var users = _voucherService.GetVoucherAssignById(Vouchermodel[i].VoucherID);
        //        if (users != null)
        //        {
        //            var userId = users.UserID;
        //            var role = users.RoleType;
        //            Vouchermodel[i].UserID = userId;
        //            Vouchermodel[i].RoleType = role;
        //            Vouchermodel[i].AssignDate = users.CreatedDate;
        //            if (role == (int)RoleType.Doctor)
        //            {
        //                Vouchermodel[i].UserName = _doctorService.GetDoctorById(Vouchermodel[i].UserID).FirstOrDefault().MiddleName == null ? _doctorService.GetDoctorById(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault() : _doctorService.GetDoctorById(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.MiddleName + " " + x.SurName).FirstOrDefault();
        //            }
        //            else if (role == (int)RoleType.Patient)
        //            {
        //                Vouchermodel[i].UserName = _userService.GetPatientById(Vouchermodel[i].UserID).FirstOrDefault().MiddleName == null ? _userService.GetPatientById(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.SurName).FirstOrDefault() : _userService.GetPatientById(Vouchermodel[i].UserID).Select(x => x.FirstName + " " + x.MiddleName + " " + x.SurName).FirstOrDefault();
        //            }
        //        }
        //    }
        //    //Fromdate == string.Empty && ToDate == string.Empty && LoginName == string.Empty && voucherstatus == string.Empty && Userid == 0
        //    var model = Vouchermodel.Where(x => x.Status == 0).ToList();
        //    var model1 = Vouchermodel.Where(x => x.GeneratedDate >= Convert.ToDateTime("2016-04-03")).Where(x => x.GeneratedDate <= Convert.ToDateTime("2016-04-06")).ToList();       
        //    //return PartialView("_PartialVoucherList", model);
        //  return Json(model, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult UpdateVoucher(Guid? id)
        {
            if (id !=null)
            {
                VoucherAssign voucher = new VoucherAssign();
                voucher.RoleType = (int)RoleType.Patient;
                voucher.VoucherID = Convert.ToInt32(Session["VoucherID"]);
                voucher.UserID = (Guid)id;
                voucher.CreatedBy = base.loginUserModel.LoginId;
                voucher.CreatedDate = DateTime.Now;
                //voucher.VoucherAssignID = 145;
                voucher.IsApproved = true;
                voucher.IsActive = true;
                voucher.IsDeleted = false;
                // voucher.ExpiryDate = DateTime.Now;
                _voucherService.AddVoucher(voucher);

            }
            return RedirectToAction("Findpatient", "Patient", new { area = "Admin" });

        }
        public ActionResult DoctorVoucher(Guid? id, int? Month, int VoucherID)
        {
            if (id !=null)
            {
                VoucherAssign voucher = new VoucherAssign();
                voucher.RoleType = (int)RoleType.Doctor;
                voucher.VoucherID = VoucherID;
                voucher.UserID = (Guid)id;
                voucher.CreatedBy = base.loginUserModel.LoginId;
                voucher.CreatedDate = DateTime.Now;
                voucher.IsActive = true;
                voucher.IsDeleted = false;
                //voucher.VoucherAssignID = 1;
                voucher.IsApproved = true;
                _voucherService.AddVoucher(voucher);
                //var model = _voucherService.GetVoucherDetailsById(VoucherID);
                //if (model != null)
                //{
                //    model.ValidityMonth = Convert.ToInt32(Month);
                //    DateTime expirydate = DateTime.Now.AddMonths(Convert.ToInt32(Month));
                //    model.ExpiryDate = expirydate;
                //    model.Status = 1;
                //    _voucherService.Update(model);

                //}
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}