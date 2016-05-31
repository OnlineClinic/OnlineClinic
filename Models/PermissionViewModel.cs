using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class PermissionViewModel
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public int ModuleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<LookUpModule> ModulesList { get; set; }
        public PermissionViewModel()
        { }
        public PermissionViewModel(LookUpPermission model)
        {
            this.Id = model.Id;
            this.PermissionName = StringCipher.Decrypt(model.PermissionName);
            this.IsActive = model.IsActive;
            this.IsDeleted = model.IsDeleted;
            this.ModuleId = model.ModuleId;
        }
        public LookUpPermission GetEditModel(PermissionViewModel model)
        {
            LookUpPermission permissionModel = new LookUpPermission();
            permissionModel.Id = model.Id;
            permissionModel.PermissionName = model.PermissionName;//StringCipher.Encrypt(model.PermissionName);
            permissionModel.IsActive = model.IsActive;
            permissionModel.IsDeleted = model.IsDeleted;
            permissionModel.ModuleId = model.ModuleId;
            return permissionModel;
        }
    }
    public class permissionsInModulesViewModel
    {
        public string ModuleName { get; set; }
        public string CssClass { get; set; }
        public string ImageUr { get; set; }
       
    }    
}