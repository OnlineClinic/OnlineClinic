using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    [AuthorizeRole("Admin")]
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        //
        // GET: /Admin/Country/
        public ActionResult Index()
        {
            var country = _countryService.GetCountryList()
                .Select(x => new CountryViewModel {CountryName=StringCipher.Decrypt(x.CountryName),CountryCode=x.CountryCode,CountryId=x.CountryId,
                    IsActive=x.IsActive}).ToList();
            return View(country);
        }

        //
        // GET: /Admin/Country/Details/5
        public ActionResult Details(int id)
        {
            var details = _countryService.GetCountryById(id);
            return View(details);
        }

        //
        // GET: /Admin/Country/Create
        public ActionResult Add(int? id)
        {
             if (id.HasValue)
            {
                var model = _countryService.GetCountryById(Convert.ToInt32(id));
                CountryViewModel cModel = new CountryViewModel(model);
                return View(cModel);
            }
            else {
                CountryViewModel model = new CountryViewModel();
                return View(model);
            }
        }

        //
        // POST: /Admin/Country/Create
        [HttpPost]
        public ActionResult Add(CountryViewModel model)
        {
            try
            {
                LookUpCountry cModel = new LookUpCountry();
                model.IsActive = true;
                cModel= model.GetEditModel(cModel);
                if (cModel.CountryId > 0)
                {
                    _countryService.Update(cModel);
                }
                else{_countryService.Add(cModel);}
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }

        }

        public ActionResult Inactive(FormCollection collection)
        {
            var ids = collection["serviceIds"];
            _countryService.DeleteCountry(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _countryService.ActivateCountry(ids);
            return RedirectToAction("Index");
        }
    }
}
