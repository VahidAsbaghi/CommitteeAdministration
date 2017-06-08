﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Models;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// indicator real values controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    //[Authorize(Roles = "SuperAdmin,Manager,Technician")]
    public class IndicatorRealValuesController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private int _indexRealIndicator = -1;
        private static IndicatorRealValueViewModel _model;

        private User GetCurrentUser()
        {
            return _mainContainer.UserRepository.FirstOrDefault(user => user.UserName == HttpContext.User.Identity.Name);
        }
        // GET: IndicatorRealValues        
        /// <summary>
        ///main list of all
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var indicatorRealValues = _mainContainer.IndicatorRealValueRepository.AllIncluding(i => i.Indicator);
            return View(await indicatorRealValues.ToListAsync());
        }

        // GET: IndicatorRealValues/Details/5        
        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var indicatorRealValue = await _mainContainer.IndicatorRealValueRepository.FirstOrDefaultAsync(realValueT=>realValueT.Id==id);
            if (indicatorRealValue == null)
            {
                return HttpNotFound();
            }
            return View(indicatorRealValue);
        }

        // GET: IndicatorRealValues/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject");
            return View();
        }

        // POST: IndicatorRealValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified indicator real value.
        /// </summary>
        /// <param name="indicatorRealValue">The indicator real value.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Value,Time,IndicatorId")] IndicatorRealValue indicatorRealValue)
        {
            if (ModelState.IsValid)
            {
                indicatorRealValue.Time=DateTime.Now;
                _mainContainer.IndicatorRealValueRepository.Add(indicatorRealValue);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject", indicatorRealValue.IndicatorId);
            return View(indicatorRealValue);
        }

        // GET: IndicatorRealValues/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var indicatorRealValue = await _mainContainer.IndicatorRealValueRepository.FirstOrDefaultAsync(realValueT => realValueT.Id == id);
            if (indicatorRealValue == null)
            {
                return HttpNotFound();
            }
            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject", indicatorRealValue.IndicatorId);
            return View(indicatorRealValue);
        }

        // POST: IndicatorRealValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified indicator real value.
        /// </summary>
        /// <param name="indicatorRealValue">The indicator real value.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Value,Time,IndicatorId")] IndicatorRealValue indicatorRealValue)
        {
            if (ModelState.IsValid)
            {
                indicatorRealValue.Time=DateTime.Now;
                _mainContainer.Entry(indicatorRealValue).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject", indicatorRealValue.IndicatorId);
            return View(indicatorRealValue);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            
        }
        #region Add Real Values
        public ActionResult AddRealValues()
        {
            var model = new IndicatorRealValueViewModel()
            {
                Committees = new SelectList(
                    _mainContainer.CommitteeRepository.All(), "Id", "Name")
            };
            return View(model);
        }
        /// <summary>
        /// Adds the real value partial view. this controler action is used to set real value of indicators by technicians
        /// </summary>
        /// <param name="committeeId">The committee identifier.</param>
        /// <returns></returns>
        public Task<PartialViewResult> AddRealValuePartial(int committeeId)
        {
            //IMPORTANT: in view we have a condition that should always pass non-null and unique real values to view
            var model=new IndicatorRealValueViewModel();
            var committee =
                _mainContainer.CommitteeRepository.FirstOrDefault(
                    committeeT => committeeT.Id == committeeId);
            //model.Criteria = _mainContainer.CriterionRepository.Where(criterionT => criterionT.Committee.Id == committee.Id).ToList();
            //model.SubCriterions= _mainContainer.SubCriterionRepository.All().ToList();
            //model.Indicators= _mainContainer.IndicatorRepository.All().ToList();
            //model.IdealValues = _mainContainer.IndicatorIdealValueRepository.All().ToList();
            model = MyMockModelBuilder(committee);
            _model = model;
           return Task.Factory.StartNew(() => PartialView(model));
        }
        /// <summary>
        /// Returns the committee condition.
        /// </summary>
        /// <param name="indicatorRealValueModel">The indicator real value model.</param>
        /// <returns></returns>
        public JsonResult ReturnCommitteeCondition(int id,double realValue)//IndicatorRealValueViewModel indicatorRealValueModel)
        {
          //  _model=MyMockModelBuilder()
            var realValueModel = _model.RealValues.FirstOrDefault(realValueT => realValueT.Id == id);
               // _mainContainer.IndicatorRealValueRepository.FirstOrDefault(realValueT => realValueT.Id == id);
            realValueModel.Value = realValue;
            var idealValue =_model.IdealValues.FirstOrDefault(
                idealValueT => idealValueT.Indicator.Id == realValueModel.Indicator.Id);
            //_mainContainer.IndicatorIdealValueRepository.FirstOrDefault(
            //    idealValueT => idealValueT.Indicator.Id == realValueModel.Indicator.Id);
            var condition = CalculateCommitteeCondition(idealValue,realValueModel);
            return Json(condition, JsonRequestBehavior.AllowGet);// Task<JsonResult>.Factory.StartNew(()=>Json(condition, JsonRequestBehavior.AllowGet));
        }
        private CommitteeConditionReturnModel CalculateCommitteeCondition(IndicatorIdealValue idealValue,IndicatorRealValue realValue)
        {
            
          
                var value = realValue.Value.GetValueOrDefault(0);
               
                double conditionValue;
                if (idealValue.LowerThan.GetValueOrDefault(false))
                {
                    conditionValue = 200 - (value/idealValue.Value.GetValueOrDefault(1))*100;
                }
                else
                {
                     conditionValue = (value / idealValue.Value.GetValueOrDefault(1)) * 100;
                }
            var condition = new CommitteeConditionReturnModel() { 
            Condition = conditionValue>85?CommitteeCondition.VeryGood : 
                        conditionValue>70?CommitteeCondition.Good : conditionValue>55?CommitteeCondition.Bad : 
                        conditionValue>0?CommitteeCondition.ExtremelyBad : CommitteeCondition.None,
                    RealValueId = realValue.Id
                };
            
            return condition;
        }
        /// <summary>
        /// Saves the real values.
        /// </summary>
        /// <param name="indicatorRealValue">The indicator real value.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRealValues(IndicatorRealValueViewModel indicatorRealValue)
        {
            foreach (var realValue in indicatorRealValue.RealValues)
            {
                if (realValue?.Value != null)
                {
                    realValue.Time=DateTime.Now;
                    _mainContainer.IndicatorRealValueRepository.Add(realValue);
                    _mainContainer.IndicatorModificationRepository.Add(new IndicatorModification()
                    {
                        AddRealValue = true,
                        User = GetCurrentUser(),
                        Indicator = realValue.Indicator,
                        Time = DateTime.Now
                    });
                }
            }
            _mainContainer.SaveChanges();

            return View("AddRealValues");
        }
        private IndicatorRealValueViewModel MyMockModelBuilder(Committee committee)
        {
            var random=new Random();
            int idSubCriterion = 0;
            int idIndicator = 0;
            int idCriterion = 0;
            var model = new IndicatorRealValueViewModel();
            model.Criteria=new List<Criterion>();
            model.SubCriterions=new List<SubCriterion>();
            model.Indicators=new List<Indicator>();
            model.IdealValues=new List<IndicatorIdealValue>();
            model.RealValues=new List<IndicatorRealValue>();
            for (int i = 0; i < 2; i++)
            {
                var criterion = new Criterion()
                {
                    Coefficient = 0.1,
                    Committee = committee,
                    CommitteeId = committee.Id,
                    Subject = i + " Criterion",
                    Id = idCriterion
                };
                model.Criteria.Add(criterion);
                idCriterion++;
                for (int j = 0; j < 3; j++)
                {
                    var subCriterion = new SubCriterion()
                    {
                        Coefficient = 0.2,
                        Criterion = criterion,
                        CriterionId = criterion.Id,
                        Id = idSubCriterion,
                        Subject = idSubCriterion + " SubCriterion"
                    };
                    model.SubCriterions.Add(subCriterion);
                    idSubCriterion++;
                    for (int k = 0; k < 3; k++)
                    {
                        var indicator = new Indicator()
                        {
                            Coefficient = 0.1,
                            Id = idIndicator,
                            SubCriterion = subCriterion,
                            SubCriterionId = subCriterion.Id,
                            Subject = idIndicator + " Indicator"
                        };
                        model.Indicators.Add(indicator);
                        
                        var idealValue = new IndicatorIdealValue()
                        {
                            Id = idIndicator,
                            Indicator = indicator,
                            IndicatorId = indicator.Id,
                            MoreThan = true,
                            Time = DateTime.Now,
                            Value = 0.1
                        };
                        model.IdealValues.Add(idealValue);
                        var realValue = new IndicatorRealValue()
                        {
                            Id = idIndicator,
                            Indicator = indicator,
                            IndicatorId = indicator.Id,
                            Time = DateTime.Now,
                            Value = random.NextDouble()
                        };
                        model.RealValues.Add(realValue);
                        idIndicator++;
                    }
                }
            }
            return model;
        }
        #endregion
    }
}
