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
    public class TitleController : BaseController
    {
        private readonly ITitleService _TitleServices;
        private readonly IUserService _UserService;

        public TitleController(ITitleService titleServices, IUserService UserService)
        {
            _TitleServices = titleServices;
            _UserService = UserService;
        }
        //
        // GET: /Admin/Title/
        public ActionResult Index()
        {
            //x.AvailabilityType == (Int32)AppointmentType.VideoConsult ? "VideoConsult" : x.AvailabilityType == (Int32)AppointmentType.ClinicVisit ? "ClinicVisit" : "Home Visit",

            var Titles = _TitleServices.GetTitleList().Select(x => new TitleViewModel { 
                TitleName = x.TitleName, TitleId = x.TitleId, TitleType = x.Type, 
                TypeName = x.Type == (int)RoleType.Doctor ? RoleType.Doctor.ToString() 
                : x.Type==(int)RoleType.Patient ? RoleType.Patient.ToString() 
                : x.Type==(int)RoleType.User ? RoleType.User.ToString() 
                : "ADMIN",
                IsActive = x.IsActive,
            }).ToList();

            for (int i = 0; i < Titles.Count;i++ )
            {
                Titles[i].TitleName = StringCipher.Decrypt(Titles[i].TitleName);
            }
                return View(Titles);
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
            items.Add(new SelectListItem { Text = RoleType.User.ToString(), Value = ((int)RoleType.User).ToString() });
            if (id.HasValue)
            {
                var model = _TitleServices.GetTitleById(Convert.ToInt32(id));
                TitleViewModel cModel = new TitleViewModel(model);
                cModel.TitleTypes = items;
                cModel.TitleName = StringCipher.Decrypt(cModel.TitleName);
                return View(cModel);
            }
            else
            {
                TitleViewModel model = new TitleViewModel();
                model.TitleTypes = items;
                return View(model);
            }
        }

        //
        // POST: /Admin/Title/InsertAndUpdate
        [HttpPost]
        public ActionResult Add(TitleViewModel model)
        {
            try
            {
                LookUpTitle cModel = new LookUpTitle();
                model.IsActive = true;
                cModel = model.GetEditModel(model);

                var userExists = _TitleServices.CheckDuplicateTitleName(model.TitleName, model.TitleType);
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "Title is already exists.");

                    List<SelectListItem> items = new List<SelectListItem>();

                    items.Add(new SelectListItem
                    {
                        Text = RoleType.Doctor.ToString(),
                        Value = ((int)RoleType.Doctor).ToString(),
                        Selected = true
                    });

                    items.Add(new SelectListItem { Text = RoleType.Patient.ToString(), Value = ((int)RoleType.Patient).ToString() });
                    items.Add(new SelectListItem { Text = RoleType.ADMIN.ToString(), Value = ((int)RoleType.ADMIN).ToString() });
                    items.Add(new SelectListItem { Text = RoleType.User.ToString(), Value = ((int)RoleType.User).ToString() });
                    model.TitleTypes = items;
                    return View(model);
                }

                if (model.TitleId > 0)
                {
                    _TitleServices.Update(cModel);
                }
                else
                {
                    _TitleServices.Add(cModel);
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
                _TitleServices.DeleteTitle(Convert.ToString(id));
                return RedirectToAction("Index");
            }

            return View();
        }
        public ActionResult Inactive(FormCollection collection)
        {
            var ids = collection["serviceIds"];
            _TitleServices.DeleteTitle(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _TitleServices.ActivateTitle(ids);
            return RedirectToAction("Index");
        }

    }
}