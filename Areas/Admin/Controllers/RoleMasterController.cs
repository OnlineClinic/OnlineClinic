using MyOnlineClinic.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    public class RoleMasterController : BaseController
    {
        ILookUpUserRolesService _roleService;
        ILookUpUserRoleTypeService _roleTypeService;

        public RoleMasterController(ILookUpUserRolesService roleService, ILookUpUserRoleTypeService roleTypeService)
        {
            _roleService = roleService;
            _roleTypeService = roleTypeService;
        }


        //
        // GET: /Admin/RoleMaster/
        public ActionResult Index()
        {
            var model = _roleService.GetUserRolesList();
            for (int i = 0; i < model.Count; i++)
            {
                model[i].RoleName = StringCipher.Decrypt(model[i].RoleName);

                model[i].RoleType.RoleTypeName = StringCipher.Decrypt(model[i].RoleType.RoleTypeName);
            }
            return View(model);
        }

        //
        // GET: /Admin/RoleMaster/Details/5
        public ActionResult Add(int? id)
        {
            RoleViewModel model = new RoleViewModel();
            if (id != null)
            {
                var Role = _roleService.GetUserRolesList().Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                model.RoleName = StringCipher.Decrypt(Role.RoleName);
                model.RoleId = Role.Id;
            }
            model.RoleTypeList = _roleTypeService.GetRoleTypeList();
            for (int i = 0; i < model.RoleTypeList.Count;i++ )
            {
                model.RoleTypeList[i].RoleTypeName = StringCipher.Decrypt(model.RoleTypeList[i].RoleTypeName);
            }
                return View(model);
        }

        [HttpPost]
        public ActionResult Add(RoleViewModel model)
        {
            LookUpUserRoles rModel = new LookUpUserRoles();

            if (model.RoleId > 0)
            {
                var role = _roleService.GetUserRolesById(model.RoleId);
                role.RoleName = model.RoleName;
                _roleService.Update(role);
            }
            else
            {
                var userExists = _roleService.CheckDuplicateRoleName(model.RoleName, model.RoleType);
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "Role is already exists.");
                    model.RoleTypeList = _roleTypeService.GetRoleTypeList();
                    return View(model);
                }
                rModel.Id = model.RoleId;
                rModel.RoleName = StringCipher.Encrypt(model.RoleName);
                rModel.UserRoleTypeId = model.RoleType;
                _roleService.Add(rModel);
            }
            return RedirectToAction("Index");
        }


        public ActionResult Inactive(FormCollection collection)
        {
            var ids = collection["serviceIds"];
            _roleService.DeleteUserRoles(ids);
            return RedirectToAction("Index");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _roleService.ActivateUserRoles(ids);
            return RedirectToAction("Index");
        }
    }
}
