using CommitteeAdministration.Areas.Management.Controllers;
using CommitteeAdministration.Controllers;
using CommitteeAdministration.Services;
using CommitteeAdministration.Services.Contract;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Helper
{
    //This is a simplified version of the code shown in the videos
    //The instance of UnityContainer is created in the constructor 
    //rather than checking in the Instance property and performing a lock if needed
    public static class ModelContainer
    {
        private static readonly IUnityContainer _Instance;

        static ModelContainer()
        {
            _Instance = new UnityContainer();
        }
        /// <summary>
        /// Gets the instance. this instance registers all services uses in this projct
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static IUnityContainer Instance
        {
            get
            {
                _Instance.RegisterType<IMainContainer, MainContainer>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IDataContext, DataContext>(new PerThreadLifetimeManager());
                _Instance.RegisterType<IUserStore<User>, UserStore<User>>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<UserManager<User>>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<AccountController>(new InjectionConstructor());
                _Instance.RegisterType<ManageController>(new InjectionConstructor());
                //_Instance.RegisterType<UsersController>(new InjectionConstructor());
                _Instance.RegisterType<CommitteesController>(new InjectionConstructor());
                _Instance.RegisterType<ManagementController>(new InjectionConstructor());
                _Instance.RegisterType<ContactInfoesController>(new InjectionConstructor());
                _Instance.RegisterType<CriteriaController>(new InjectionConstructor());
                _Instance.RegisterType<HomeController>(new InjectionConstructor());
                _Instance.RegisterType<IndicatorIdealValuesController>(new InjectionConstructor());
                _Instance.RegisterType<IndicatorRealValuesController>(new InjectionConstructor());
                _Instance.RegisterType<IndicatorsController>(new InjectionConstructor());
                _Instance.RegisterType<PermissionsController>(new InjectionConstructor());
                _Instance.RegisterType<ProfileController>(new InjectionConstructor());
                _Instance.RegisterType<RolesController>(new InjectionConstructor());
                _Instance.RegisterType<SubCriterionsController>(new InjectionConstructor());
                _Instance.RegisterType<CommitteeStatusController>(new InjectionConstructor());
                _Instance.RegisterType<ICommitteeStatus,CommitteeStatus>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IRealValueAlarm, RealValueAlarm>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IChartDrawer, ChartDrawer>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<ICustomEmailService, CustomEmailService>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IMessageTerminal, MessageTerminal>(new HierarchicalLifetimeManager());

                _Instance.RegisterType<IUserInfoManager, UserInfoManager>(new HierarchicalLifetimeManager());
                return _Instance;
            }
        }
    }
}
