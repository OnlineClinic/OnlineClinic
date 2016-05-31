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
    public class CityController : BaseController
    {
        private readonly ICityService _cityServices;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;

        public CityController(ICityService cityServices, ICountryService countryService, IStateService stateService)
        {
            _cityServices = cityServices;
            _countryService = countryService;
            _stateService = stateService;
        }

        //
        // GET: /Admin/City/
        public ActionResult Index()
        {
            var cities = _cityServices.GetCityList().Select(x => new CityViewModel { CityName = StringCipher.Decrypt(x.LookUpCityName), CityId = x.LookUpCityId, IsActive = x.IsActive }).ToList();
            return View(cities);
        }

        // GET: /Admin/Country/Create
        public ActionResult Add(int? id)
        {
            if (id.HasValue)
            {
                var model = _cityServices.GetCityById(Convert.ToInt32(id));
                model.LookUpCityName = StringCipher.Decrypt(model.LookUpCityName);
                CityViewModel cModel = new CityViewModel(model);
                BindDropDown(cModel);
                return View(cModel);
            }
            else
            {
                CityViewModel model = new CityViewModel();
                BindDropDown(model);
                return View(model);
            }
        }

        //
        // POST: /Admin/Country/Create
        [HttpPost]
        public ActionResult Add(CityViewModel model)
        {
            try
            {
                var userExists = _cityServices.GetCityList().Where
                                  (
                                    x => StringCipher.Decrypt(x.LookUpCityName) == model.CityName
                                        //&& ((model.StateId != 0 && x.LookUpProvinceId == model.StateId) || model.StateId == 0)
                                    && ((model.CityId != 0 && x.LookUpCityId != model.CityId) || model.CityId == 0)
                                  );

                if (userExists != null && userExists.ToList().Count > 0)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("CityName", "City is already exists.");
                    CityViewModel model1 = new CityViewModel();
                    BindDropDown(model1);
                    return View(model1);
                }
                if (model.CityId > 0)
                {
                    LookUpCity cModel = _cityServices.GetCityById(model.CityId);

                    if (cModel != null)
                    {
                        cModel.LookUpCityName = StringCipher.Encrypt(model.CityName);
                        cModel.LookUpProvinceId = model.StateId;
                        cModel.CountryId = Convert.ToInt32(model.CountryId);
                        _cityServices.Update(cModel);
                    }
                }
                else
                {
                    LookUpCity cModel = model.GetEditModel(model);
                    cModel.IsActive = true;
                    _cityServices.Add(cModel);
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
                _cityServices.DeleteCity(Convert.ToString(id));
                return RedirectToAction("Index");
            }

            return View();
        }
        public CityViewModel BindDropDown(CityViewModel model)
        {
            model.Countries = _countryService.GetCountryList();
            for (int i = 0; i < model.Countries.Count; i++)
            {
                model.Countries[i].CountryName = StringCipher.Decrypt(model.Countries[i].CountryName);
            }
            model.States = model.CityId > 0 ? _stateService.GetStateList().Where(t => t.CountryId == model.CountryId).ToList() : new List<LookUpState>();

            for (int i = 0; i < model.States.Count; i++)
            {
                model.States[i].StateName = StringCipher.Decrypt(model.States[i].StateName);
            }

            return model;
        }
        public ActionResult GetState(int Id)
        {
            var State = _stateService.GetStateListbyid(Id).ToList();
            for (int i = 0; i < State.Count; i++)
            {
                State[i].StateName = StringCipher.Decrypt(State[i].StateName);
            }
            // Returns string "Electronic" or "Mail"
            return Json(State, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCity(int Id)
        {
            var City = _cityServices.GetCityListbyid(Id).ToList();
            for (int i = 0; i < City.Count; i++)
            {
                City[i].LookUpCityName = StringCipher.Decrypt(City[i].LookUpCityName);
            }
            // Returns string "Electronic" or "Mail"
            return Json(City, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Inactive(FormCollection collection)
        {
            var ids = collection["serviceIds"];
            _cityServices.DeleteCity(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _cityServices.ActivateCity(ids);
            return RedirectToAction("Index");
        }

    }
}
