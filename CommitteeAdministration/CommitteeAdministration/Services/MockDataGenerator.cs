﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using CommitteeAdministration.Helper;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CommitteeAdministration.Services.IMockDataGenerator" />
    public class MockDataGenerator
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        

        
    }
}