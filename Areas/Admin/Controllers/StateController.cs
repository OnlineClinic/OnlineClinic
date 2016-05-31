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
    public class StateController : BaseController
    {
         private readonly ICountryService _countryService;
         private readonly IStateService _stateService;
         public StateController(ICountryService countryService, IStateService stateService)
        {
            _countryService = countryService;
            _stateService = stateService;
        }
         // GET: /Admin/Country/Create
         public ActionResult Index()
         {
             var state = _stateService.GetStateList()
                .Select(x => new StateViewModel
                {
                    StateName =StringCipher.Decrypt(x.StateName),
                    Id = x.Id,
                    CountryId = x.CountryId,
                    IsActive=x.IsActive
                    
                    
                }).ToList();
             return View(state);
         }


         public ActionResult Add(int? id)
         {
             if (id.HasValue)
             {
                 var model = _stateService.GetStateById(Convert.ToInt32(id));
                 model.StateName = StringCipher.Decrypt(model.StateName);
                 StateViewModel cModel = new StateViewModel(model);
                 BindDropDown(cModel);
                 return View(cModel);
             }
             else
             {
                 StateViewModel model = new StateViewModel();
                 BindDropDown(model);
                 return View(model);
             }
         }
         //
         // POST: /Admin/Country/Create
         [HttpPost]
         public ActionResult Add(StateViewModel model)
         {
             try
             {
                 LookUpState cModel = new LookUpState();
                 cModel.CountryId = model.CountryId;
                 model.IsActive = true;
                cModel = model.GetEditModel(cModel);
                var userExists = _stateService.CheckDuplicateStateName(StringCipher.Encrypt(model.StateName));
                 if(userExists)
                 {

                 }
                 if (cModel.Id > 0)
                 {
                     _stateService.Update(cModel);
                 }
                 else {
                     cModel.CountryId = model.CountryId;
                     _stateService.Add(cModel);
                 
                 }
                 return RedirectToAction("Index");
             }
             catch
             {
                 return View(model);
             }

         }
       
         public StateViewModel BindDropDown(StateViewModel model)
         {
             model.Countries = _countryService.GetCountryList();
             for (int i = 0; i < model.Countries.Count;i++ )
             {
                 model.Countries[i].CountryName = StringCipher.Decrypt(model.Countries[i].CountryName);
             }
                 return model;
         }

         public ActionResult Inactive(FormCollection collection)
         {
             var ids = collection["serviceIds"];
             _stateService.DeleteState(ids);
             return RedirectToAction("Index");
         }
         public ActionResult Acitvate(FormCollection collection)
         {
             var ids = collection["ActivateIds"];
             _stateService.ActivateState(ids);
             return RedirectToAction("Index");
         }
	}
}