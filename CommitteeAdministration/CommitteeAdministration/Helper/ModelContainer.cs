using CommitteeAdministration.Controllers;
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
        private static IUnityContainer _Instance;

        static ModelContainer()
        {
            _Instance = new UnityContainer();
        }

        public static IUnityContainer Instance
        {
            get
            {
                _Instance.RegisterType<IMainContainer, MainContainer>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IDataContext, DataContext>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IUserStore<User>, UserStore<User>>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<UserManager<User>>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<AccountController>(new InjectionConstructor());
                _Instance.RegisterType<ManageController>(new InjectionConstructor());
                return _Instance;
            }
        }
    }
}
