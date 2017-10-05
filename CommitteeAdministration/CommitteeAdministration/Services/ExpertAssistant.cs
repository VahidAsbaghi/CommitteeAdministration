using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommitteeAdministration.Helper;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Services
{
    public class ExpertAssistant
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        public void AlarmExpert(User user)
        {
            //var committee=
        }
    }
}