using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommitteeAdministration.ViewModels
{
    public class FirstPageViewModel
    {
        public List<News> News { get; set; }
    }

    public class News
    {
        public string Link { get; set; }
        public string Title { get; set; }
    }
}