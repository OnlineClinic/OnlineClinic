using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Migrator;
using MyOnlineClinic.Web.Helper;

namespace MyOnlineClinic.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            container.RegisterType<ICountryService, CountryService>();
            container.RegisterType<IEntitiesDB, EntitiesDB>();
            container.RegisterType(typeof(IRepositoryService<>), typeof(RepositoryService<>));
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IFormsAuthentication, FormAuthService>();
            container.RegisterType<ICityService, CityService>();
            container.RegisterType<IOrganizationService, OrganizationService>();
            container.RegisterType<IOrganizationUserService, OrganizationUserService>();
            container.RegisterType<IClinicService, ClinicService>();
            container.RegisterType<IClinicUserService, ClinicUserService>();
            container.RegisterType<ILoginHelper, LoginHelper>();
            container.RegisterType<IOrganizationTypeService, OrganizationTypeService>();
            container.RegisterType<ILookUpModuleService, LookUpModuleService>();
            container.RegisterType<ILookUpUserRolesService, LookUpUserRolesService>();
            container.RegisterType<IPermissionInModuleService, PermissionInModuleService>();
            container.RegisterType<IPermissionsService, PermissionService>();
            container.RegisterType<IAdminUserService, AdminUserService>();
            container.RegisterType<ILookUpUserRoleTypeService, LookUpUserRoleTypeService>();
            container.RegisterType<ITimeZoneService, TimeZonesService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IStateService, StateService>();
            container.RegisterType<ITitleService, TitleService>();
            container.RegisterType<IGenderService, GenderService>();
            container.RegisterType<IProfessionTypeService, ProfessionTypeservice>();
            container.RegisterType<IModuleService, ModuleService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IMembershipService, MembershipService>();
            container.RegisterType<IVoucherService, VoucherService>();
            container.RegisterType<IUregisteredOrganizationService, UnregisteredOrganizationService>();
            container.RegisterType<IUnregisteredClinicService, UnregisteredClinicService>();
            container.RegisterType<IDoctorMembershipService, DoctorMembershipService>();
            container.RegisterType<IMembeshipVoucherService, MembeshipVoucherService>();
            container.RegisterType<IPaymentHelper, PaymentHelper>();
            container.RegisterType<IRosterGeneratedCodeService, RosterGeneratedCodeService>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IPaymentDetailService, PaymentDetailService>();
            container.RegisterType<IPaymentService, PaymentService>();
            container.RegisterType<ICommunication, Communication>();
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IClinicalService, ClinicalService>();
            container.RegisterType<IPatientHistoryService, PatientHistoryService>();
            container.RegisterType<IBloodPressureHeartRateService, BloodPressureHeartRateService>();
            container.RegisterType<IBloodGlucoseService, BloodGlucoseService>();
            container.RegisterType<IOxygenSaturationService, OxygenSaturationService>();
            container.RegisterType<IHeightResultService, HeightResultService>();
            container.RegisterType<ITemperatureResultService, TemperatureResultService>();
            container.RegisterType<IWaistResultService, WaistResultService>();
            container.RegisterType<IWeightResultService, WeightResultService>();
            container.RegisterType<IHomeVisitAppointmentService, HomeVisitAppointmentService>();
            container.RegisterType<IRequestPrescriptionService, RequestPrescriptionService>();
            container.RegisterType<IReportProblemService, ReportProblemService>();
            container.RegisterType<IBloodGlucoseMealTypeService, BloodGlucoseMealTypeService>();
            container.RegisterType<IPaymentHistoryService, PaymentHistoryService>();
            container.RegisterType<IPaymentCaptureService, PaymentCaptureService>();
            container.RegisterType<IPaymentRefundService, PaymentRefundService>();
            container.RegisterType<IManagePrescriptionService, ManageprescriptionService>();
            container.RegisterType<IPatientExaminationVisibilityService, PatientExaminationVisibilityService>();
            container.RegisterType<IStandardService, StandardService>();


        }
    }
}
