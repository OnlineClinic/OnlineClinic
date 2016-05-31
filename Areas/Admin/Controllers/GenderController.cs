using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    [AuthorizeRole("Admin")]
    public class GenderController : BaseController
    {
        private readonly IGenderService _GenderServices;

        public GenderController(GenderService genderServices)
        {
            _GenderServices = genderServices;
        }
        //
        // GET: /Admin/Gender/
        public ActionResult Index()
        {
            var Gender = _GenderServices.GetGenderList().Select(x => new GenderViewModel { GenderName = x.GenderName, GenderId = x.GenderId, GenderType = x.Type, TypeName = x.Type == (int)RoleType.Doctor ? RoleType.Doctor.ToString() :x.Type == (int)RoleType.ADMIN ? RoleType.ADMIN.ToString(): RoleType.Patient.ToString(),IsActive=x.IsActive }).ToList();
            for (int i = 0; i < Gender.Count;i++ )
            {
                Gender[i].GenderName = StringCipher.Decrypt(Gender[i].GenderName);
            }
                return View(Gender);
        }

        // GET: /Admin/Title/Get
        public ActionResult Add(int? id)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = RoleType.Doctor.ToString(),
                Value = ((int)RoleType.Doctor).ToString(),
                Selected = true
            });

            items.Add(new SelectListItem { Text = RoleType.Patient.ToString(), Value = ((int)RoleType.Patient).ToString() });
            items.Add(new SelectListItem { Text = RoleType.ADMIN.ToString(), Value = ((int)RoleType.ADMIN).ToString() });
            if (id.HasValue)
            {
                var model = _GenderServices.GetGenderById(Convert.ToInt32(id));
                GenderViewModel cModel = new GenderViewModel(model);
                cModel.GenderTypes = items;
                cModel.GenderName = StringCipher.Decrypt(cModel.GenderName);
                return View(cModel);
            }
            else
            {
                GenderViewModel model = new GenderViewModel();
                model.GenderTypes = items;
                return View(model);
            }
        }
        //
        // POST: /Admin/Title/InsertAndUpdate
        [HttpPost]
        public ActionResult Add(GenderViewModel model)
        {
            try
            {
                LookUpGender cModel = new LookUpGender();
                model.IsActive = true;
                cModel = model.GetEditModel(model);
                var userExists = _GenderServices.CheckDuplicateGenderName(model.GenderName, model.GenderType);
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "Gender is already exists.");
                    List<SelectListItem> items = new List<SelectListItem>();

                    items.Add(new SelectListItem
                    {
                        Text = RoleType.Doctor.ToString(),
                        Value = ((int)RoleType.Doctor).ToString(),
                        Selected = true
                    });

                    items.Add(new SelectListItem { Text = RoleType.Patient.ToString(), Value = ((int)RoleType.Patient).ToString() });
                    items.Add(new SelectListItem { Text = RoleType.ADMIN.ToString(), Value = ((int)RoleType.ADMIN).ToString() });
                    model.GenderTypes = items;
                    return View(model);
                }
                if (model.GenderId > 0)
                {
                    _GenderServices.Update(cModel);
                }
                else
                {
                    _GenderServices.Add(cModel);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


        // GET: /Admin/City/Delete
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                _GenderServices.DeleteGender(Convert.ToString(id));
                return RedirectToAction("Index");
            }

            return View();
        }
        public ActionResult Inactive(FormCollection collection)
        {
            var ids = collection["serviceIds"];
          _GenderServices.DeleteGender(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _GenderServices.ActivateGender(ids);
            return RedirectToAction("Index");
        }
	}
}