using Microsoft.Practices.Unity;

namespace CommitteeManagement.Repository
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
                
                return _Instance;
            }
        }
    }
}
