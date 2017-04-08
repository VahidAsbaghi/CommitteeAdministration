using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Services;
using CommitteeAdministration.ViewModels.ConditionViewModels;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private readonly ICommitteeStatus _committeeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        // GET: Admin
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult MainPage()
        //{
        //    return View();
        //}
        public ActionResult AdminMain()
        {
            var mainModel=new MainViewModel() {Committees = new SelectList(_mainContainer.CommitteeRepository.All(),"Id","Name")};
            return View(mainModel);
        }

        public ActionResult Charts()
        {
            return View();
        }

        public ActionResult CommitteeCondition()
        {
            return View();
        }
        /// <summary>
        /// Shows a bar chart of all indicators of one committee status.
        /// </summary>
        /// <param name="committeeId">The committee identifier.</param>
        /// <returns></returns>
        public ViewResult ShowIndicatorsStatus(int committeeId)
        {
            var committee = _mainContainer.CommitteeRepository.FirstOrDefault(committeeT => committeeT.Id == committeeId);
            var criteria =
                _mainContainer.CriterionRepository.All().Where(criterionT => criterionT.Committee.Id == committeeId).ToList();
            var categories = new List<string>();
            var datas = new List<object>();
            for (int i = 0; i < criteria.Count; i++)
            {
                var criterion = criteria[i];
                var subCriterions =
                    _mainContainer.SubCriterionRepository.All().Where(
                        subCriterionT => subCriterionT.Criterion.Id == criterion.Id);
                foreach (var subCriterion in subCriterions)
                {
                    var condition = _committeeStatus.IndicatorsPercentage(subCriterion, DateTime.Now);

                    foreach (var conditionVar in condition)
                    {
                        if (categories.Count > 10)
                        {
                            break;
                        }
                        categories.Add(conditionVar.Indicator.Subject);
                        datas.Add(conditionVar.ConditionPercentage);
                    }
                }
            }

            var chart =CommitteeStatusController.DrawColumnChart("نمودار وضعیت ستاد در شاخص ها" + committee.Name, "درصد کارایی", categories,
                datas);
            return View("AllIndicatorsStatus_Bar","_MotherElementsLayout", chart);
        }

        public ViewResult GetPersianCalender()
        {
            PersianCalendar persianCalendar=new PersianCalendar();

            return View("PersianCalenderView", persianCalendar);
        }
    }
}