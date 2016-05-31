using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyOnlineClinic.Web.Helper
{
    public interface ILoginHelper
    {
        Login GetOrganizationAdminLogin(BaseMember member, string userName, OrganizationViewModel model);
        Login GetOrganiztionUserLogin(BaseMember member, string userName, OrganizationUserViewModel model, RoleType role);
        Login GetPatientDoctorLogin(BaseMember member, string userName, RegisterViewModel model);
        BaseMember GetOrganizationAdmin(OrganizationViewModel model, int orgId);
        BaseMember GetUser(dynamic model, int orgId);
        BaseMember GetClinicUser(dynamic model, int clinicId,int orgId);
        Login GetClinicUserLogin(BaseMember member, string userName, ClinicUserViewModel model, RoleType role);
        dynamic UploadImage(dynamic model, HttpPostedFileBase profilePic);
        void UpdateProfile(UserViewModel model, HttpPostedFileBase profilePic, Guid loginId);
        BaseMember GetAdminUser(dynamic model);
        Login GetAminUserLogin(BaseMember member, string userName, UserViewModel model, RoleType role);

    }
}
