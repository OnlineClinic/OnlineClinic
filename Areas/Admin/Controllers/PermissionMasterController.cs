using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    public class PermissionMasterController : BaseController
    {

        IPermissionsService _permissionsService;
        private readonly IModuleService _moduleService;

        public PermissionMasterController(IPermissionsService permissionsService, IModuleService moduleService)
        {
            _permissionsService = permissionsService;
            _moduleService = moduleService;
        }
     
        //
        // GET: /Admin/RoleMaster/
        public ActionResult Index()
        {
            var model = _permissionsService.GetPermissionList().Select(x => new PermissionViewModel
            {
                PermissionName =StringCipher.Decrypt(x.PermissionName),
                IsActive = x.IsActive,
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                ModuleId = x.ModuleId
            }).ToList();
            return View(model);
        }

        //
        // GET: /Admin/RoleMaster/Details/5
        public ActionResult Add(int? id)
        {

            if (id.HasValue)
            {
                var model = _permissionsService.GetPermissionById(Convert.ToInt32(id));
                model.PermissionName = StringCipher.Decrypt(model.PermissionName);
                PermissionViewModel cModel = new PermissionViewModel(model);
                cModel = BindDropDownList(cModel);
                return View(cModel);
            }
            else
            {
                PermissionViewModel model = new PermissionViewModel();
                model = BindDropDownList(model);
                return View(model);
            }
        }
        private PermissionViewModel BindDropDownList(PermissionViewModel model)
        {
            model.ModulesList = _moduleService.GetModuleList();
            for (int i = 0; i < model.ModulesList.Count;i++ )
            {
                model.ModulesList[i].ModuleName = StringCipher.Decrypt(model.ModulesList[i].ModuleName);
                
            }
                return model;
        }
        [HttpPost]
        public ActionResult Add(PermissionViewModel model)
        {
            try
            {
                LookUpPermission cModel = new LookUpPermission();
                cModel = model.GetEditModel(model);
                if (model.Id > 0)
                {
                  
                    _permissionsService.Update(cModel);
                }
                else
                {
                    var userExists = _permissionsService.CheckDuplicateRoleName(model.PermissionName, model.ModuleId);
                    if (userExists)
                    {
                        ModelState.AddModelError("UserName", "Permission is already exists.");
                        BindDropDownList(model);
                        return View(model);
                    }
                    _permissionsService.Add(cModel);
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
                _permissionsService.DeletePermission(Convert.ToString(id));
                return RedirectToAction("Index");
            }

            return View();
        }
        public ActionResult Inactive(FormCollection collection)
        {
            var ids = collection["serviceIds"];
            _permissionsService.DeletePermission(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _permissionsService.ActivatePermission(ids);
            return RedirectToAction("Index");
        }
	}
}