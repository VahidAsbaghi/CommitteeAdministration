using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Helper;
using CommitteeAdministration.Models;
using CommitteeAdministration.Services;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// this view model is used to add/edit criterion-subCriterion-Indicator usng wizard step by step
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CriteriaScIndController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private readonly CommitteeStatus _committeeStatus = ModelContainer.Instance.Resolve<CommitteeStatus>();
        /// <summary>
        /// Gets the current user.Retrieve user info using httpcontext
        /// </summary>
        /// <returns></returns>
        private User GetCurrentUser()
        {
            return _mainContainer.UserRepository.FirstOrDefault(user => user.UserName == HttpContext.User.Identity.Name);
        }
        // GET: CriteriaScInd
        //public ActionResult Index()
        //{
        //    return View();
        //}
       
        public ActionResult CommitteeStatusMain()
        {
            var model = new CommitteeStatusViewModel()
            {
                Committees = new SelectList(
                    _mainContainer.CommitteeRepository.All(), "Id", "Name")
            };
            return View(model);
        }
        public Task<PartialViewResult> CommitteeStatus(int committeeId)
        {
            var model = new CommitteeStatusViewModel();
            var committee =
                _mainContainer.CommitteeRepository.FirstOrDefault(
                    committeeT => committeeT.Id == committeeId);
            model.CriteriaList =
                _committeeStatus.GetCriteriaCondition(DateTime.Now, committee);
            model.SubCriterionList=new List<SubCriterionConditionModel>();
            model.Indicators=new List<IndicatorsConditionModel>();
            foreach (var criterionCondition in model.CriteriaList)
            {
                model.SubCriterionList.AddRange(_committeeStatus.GetSubCriterionsCondition(criterionCondition.Criterion,DateTime.Now));
            }
            foreach (var subCriterionCondition in model.SubCriterionList)
            {
                model.Indicators.AddRange(_committeeStatus.IndicatorsPercentage(subCriterionCondition.SubCriterion,DateTime.Now));
            }
            
            return Task.Factory.StartNew(() => PartialView(model));
        }
        [HttpGet]
        public ActionResult ViewAll()
        {
            var model = new ViewAllViewModel()
            {
                Committees = new SelectList(
                    _mainContainer.CommitteeRepository.All(),"Id","Name")
            };
            return View(model);
        }
        [HttpGet]
        public JsonResult AllData(ViewAllViewModel model)
        {
            var committee =
                _mainContainer.CommitteeRepository.FirstOrDefault(
                    committeeT => committeeT.Id == model.SelectedCommitteeId);
            model.CriteriaList =
                _mainContainer.CriterionRepository.Where(criterionT => criterionT.Committee.Id == committee.Id).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
            //return new JsonResult()
            //{
            //    ContentEncoding = Encoding.Unicode,ContentType = "json",Data = model,JsonRequestBehavior = JsonRequestBehavior.AllowGet

            //};
        }
        [HttpGet]
        public Task<PartialViewResult> ViewAllTree(int committeeId)
        {
            var model=new ViewAllViewModel();
            var committee =
                _mainContainer.CommitteeRepository.FirstOrDefault(
                    committeeT => committeeT.Id == committeeId);
            model.CriteriaList =
                _mainContainer.CriterionRepository.Where(criterionT => criterionT.Committee.Id == committee.Id && (!criterionT.IsDeleted.HasValue || !criterionT.IsDeleted.Value)).ToList();
            model.SubCriterionList = _mainContainer.SubCriterionRepository.Where(subCriterion=>(!subCriterion.IsDeleted.HasValue || !subCriterion.IsDeleted.Value) && subCriterion.Committee.Id == committee.Id).ToList();
            model.Indicators = _mainContainer.IndicatorRepository.Where(indicator=> (!indicator.IsDeleted.HasValue || !indicator.IsDeleted.Value) && indicator.Committee.Id == committee.Id).ToList();
            model.SelectedCommitteeId = committeeId;
            return Task.Factory.StartNew(()=>PartialView(model));
        }
        [HttpGet]
        public ActionResult CriterionCoefficientsPartial(int committeeId)
        {
            var model=new CriterionCoefficient();
            model.Coefficients=new List<Coefficient>();
            var criteria = _mainContainer.CriterionRepository.Where(criterion => criterion.Committee.Id == committeeId);
            foreach (var criterion in criteria)
            {
                model.Coefficients.Add(new Coefficient() {CoefficientValue = criterion.Coefficient.GetValueOrDefault(0),Id = criterion.Id});
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult CriterionCoefficientsPartial(CriterionCoefficient criterionCoefficient)
        {
            if (ModelState.IsValid)
            {
                foreach (var coefficient in criterionCoefficient.Coefficients)
                {
                    var criterion =
                        _mainContainer.CriterionRepository.FirstOrDefault(criterionT => criterionT.Id == coefficient.Id);
                    criterion.Coefficient = coefficient.CoefficientValue;
                    var criterionModification = new CriterionModification
                    {
                        User = GetCurrentUser(),
                        Update = true,
                        Criterion = criterion,
                        CriterionId = criterion.Id,
                        Time = DateTime.Now
                    };
                    _mainContainer.CriterionRepository.Attach(criterion);
                    _mainContainer.CriterionModificationRepository.Add(criterionModification);
                }
                _mainContainer.SaveChanges();
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }

        [HttpGet]
        public ActionResult SubCriterionCoefficientsPartial(int criterionId)
        {
            var model = new SubCriterionCoefficient {Coefficients = new List<Coefficient>()};
           
            var subCriteria = _mainContainer.SubCriterionRepository.Where(subCriterion => subCriterion.Criterion.Id == criterionId);
            foreach (var subCriterion in subCriteria)
            {
                model.Coefficients.Add(new Coefficient() { CoefficientValue = subCriterion.Coefficient, Id = subCriterion.Id });
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult SubCriterionCoefficientsPartial(SubCriterionCoefficient subCriterionCoefficient)
        {
            if (ModelState.IsValid)
            {
                foreach (var coefficient in subCriterionCoefficient.Coefficients)
                {
                    var subCriterion =
                        _mainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == coefficient.Id);
                    subCriterion.Coefficient = coefficient.CoefficientValue;
                    var subCriterionModification = new SubCriterionModification()
                    {
                        User = GetCurrentUser(),
                        Update = true,
                        SubCriterion = subCriterion,
                        SubCriterionId = subCriterion.Id,
                        Time = DateTime.Now
                    };
                    _mainContainer.SubCriterionRepository.Attach(subCriterion);
                    _mainContainer.SubCriterionModificationRepository.Add(subCriterionModification);
                }
                _mainContainer.SaveChanges();
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }
        [HttpGet]
        public ActionResult IndicatorCoefficientsPartial(int subCriterionId)
        {
            var model = new IndicatorCoefficient { Coefficients = new List<Coefficient>() };
          
            var indicators = _mainContainer.IndicatorRepository.Where(indicator => indicator.SubCriterion.Id == subCriterionId);
            foreach (var indicator in indicators)
            {
                model.Coefficients.Add(new Coefficient() { CoefficientValue = indicator.Coefficient.GetValueOrDefault(0), Id = indicator.Id });
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult IndicatorCoefficientsPartial(IndicatorCoefficient indicatorCoefficient)
        {
            if (ModelState.IsValid)
            {
                foreach (var coefficient in indicatorCoefficient.Coefficients)
                {
                    var indicator =
                        _mainContainer.IndicatorRepository.FirstOrDefault(indicatorT => indicatorT.Id == coefficient.Id);
                    indicator.Coefficient = coefficient.CoefficientValue;
                    var indicatorModification = new IndicatorModification
                    {
                        User = GetCurrentUser(),
                        UpdateIndicator = true,
                        Indicator = indicator,
                        IndicatorId = indicator.Id,
                        Time = DateTime.Now
                    };
                    _mainContainer.IndicatorRepository.Attach(indicator);
                    _mainContainer.IndicatorModificationRepository.Add(indicatorModification);
                }
                _mainContainer.SaveChanges();
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
            }
            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }
        public ActionResult FirstPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// the first partial view post method
        /// </summary>
        /// <param name="criteriaSubInd">The criteria sub ind.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FirstPartial(AddCriteriaSubCriterionIndicators criteriaSubInd)
        {
            if (ModelState.IsValid)
            {
                criteriaSubInd.NavigationViewName=NavigationViewName.AddCriterion;
            }
            return  View("Create",criteriaSubInd);
        }
        // GET: Permissions/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {            
            return View();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddCriteriaSubCriterionIndicators criteriaSubInd)
        {
            if (ModelState.IsValid)
            {
                
                await _mainContainer.SaveChangesAsync();
                //return RedirectToAction();
            }

            return View(criteriaSubInd);
        }
        ///GET
        /// <summary>
        /// Adds the criterion.
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCriterion()
        {
            ViewBag.IsRedirect = false;
      
            var model = new AddCriterionViewModel
            {
                CommitteeName =new SelectList( _mainContainer.CommitteeRepository.All().ToList(),"Id","Name")
            };
            return PartialView(model);
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        ///  <summary>
        /// Add Criterion then add another criterion
        ///  </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewCriterion(AddCriterionViewModel criterion)
        {
            ViewBag.IsRedirect = false;
            criterion.CommitteeName = new SelectList(_mainContainer.CommitteeRepository.All().ToList(), "Id", "Name");
            if (!ModelState.IsValid)
            {              
                return View("Create", new AddCriteriaSubCriterionIndicators { NavigationViewName = NavigationViewName.AddCriterion});
            }
            var committee =
              GetCommittee(criterion.ReturnedCommitteeId);

            AddCriterionRepository(criterion, committee);

            _mainContainer.SaveChanges();

            AddCriterionModification(criterion.Subject,committee);
           
            await _mainContainer.SaveChangesAsync();
            return View("Create",new AddCriteriaSubCriterionIndicators {NavigationViewName = NavigationViewName.AddCriterion,Committee = committee});
        }
        /// <summary>
        /// Adds the criterion and go ahead.add criterion and go to add sub-criterion
        /// </summary>
        /// <param name="criterion">The criterion.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCriterionAndGoAhead(AddCriterionViewModel criterion)
        {
            ViewBag.IsRedirect = true;
            
            if (!ModelState.IsValid)
            {
                criterion.CommitteeName = new SelectList(_mainContainer.CommitteeRepository.All().ToList(), "Id", "Name");
                return View("AddCriterion",criterion);
            }
            var committee =
                GetCommittee(criterion.ReturnedCommitteeId);

            AddCriterionRepository(criterion, committee);

            _mainContainer.SaveChanges();

            AddCriterionModification(criterion.Subject, committee);

            await _mainContainer.SaveChangesAsync();
            
          var  criterionView =
                _mainContainer.CriterionRepository.FirstOrDefault(
                    criterionT => criterionT.Subject == criterion.Subject &&
                                  criterionT.CommitteeId == criterion.ReturnedCommitteeId);
            return View("Create", new AddCriteriaSubCriterionIndicators { NavigationViewName = NavigationViewName.AddSubCriterion,Criterion = criterionView});
        }
        /// <summary>
        /// Adds the criterion modification.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="committee">The committee.</param>
        private void AddCriterionModification(string subject,Committee committee)
        {
            var modify = new CriterionModification
            {
                Add = true,
                User = GetCurrentUser(),
                Time = DateTime.Now,
                Criterion = _mainContainer.CriterionRepository.All()
                    .FirstOrDefault(
                        criterionT => criterionT.Subject == subject && criterionT.Committee.Id == committee.Id)
            };

            _mainContainer.CriterionModificationRepository.Add(modify);          
        }
        /// <summary>
        /// Adds the criterion into repository repository.
        /// </summary>
        /// <param name="criterion">The criterion.</param>
        /// <param name="committee">The committee.</param>
        private void AddCriterionRepository(AddCriterionViewModel criterion,Committee committee)
        {
            _mainContainer.CriterionRepository.Add(new Criterion()
            {
                Coefficient = criterion.Coefficient,
                Committee = committee,
                Subject = criterion.Subject
            });
        }
        /// <summary>
        /// Gets the committee.
        /// </summary>
        /// <param name="committeeId">The committee identifier.</param>
        /// <returns></returns>
        private Committee GetCommittee(int committeeId)
        {
            return 
                _mainContainer.CommitteeRepository.All().FirstOrDefault(
                    committeeT => committeeT.Id == committeeId);

        }

        #region SubCriterion Region    
        ///GET    
        /// <summary>
        /// Adds the sub criterion.
        /// </summary>
        /// <returns></returns>
        public ActionResult AddSubCriterion()
        {

            return PartialView();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified SubCriterion and go to add another sub-criterion
        /// </summary>
        /// <param name="subCriterion"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewSubCriterion(AddSubCriterionViewModel subCriterion)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", new AddCriteriaSubCriterionIndicators() { NavigationViewName = NavigationViewName.AddSubCriterion, Criterion = subCriterion.Criterion });
            }
            AddSubCriterionRepository(subCriterion);
            _mainContainer.SaveChanges();
            AddSubCriterionModification(subCriterion);
            await _mainContainer.SaveChangesAsync();
            return View("Create",new AddCriteriaSubCriterionIndicators() {NavigationViewName = NavigationViewName.AddSubCriterion,Criterion = subCriterion.Criterion}); 
        }
        /// <summary>
        /// Adds the sub criterion and go ahead.add and go ahead to add indicator
        /// </summary>
        /// <param name="subCriterion">The sub criterion.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubCriterionAndGoAhead(AddSubCriterionViewModel subCriterion)
        {
            if (!ModelState.IsValid)
            {
                return View("AddSubCriterion", subCriterion);
            }
            AddSubCriterionRepository(subCriterion);
            _mainContainer.SaveChanges();
            AddSubCriterionModification(subCriterion);
            await _mainContainer.SaveChangesAsync();


            return View("Create", new AddCriteriaSubCriterionIndicators() { NavigationViewName = NavigationViewName.AddIndicator,
                SubCriterion = _mainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT=>subCriterionT.Criterion.Id==subCriterion.Criterion.Id&&subCriterionT.Subject==subCriterion.Subject)});
        }
        /// <summary>
        /// Adds the sub criterion repository.
        /// </summary>
        /// <param name="subCriterion">The sub criterion.</param>
        private void AddSubCriterionRepository(AddSubCriterionViewModel subCriterion)
        {
            _mainContainer.SubCriterionRepository.Add(new SubCriterion()
            {
                Coefficient = subCriterion.Coefficient,
                Criterion =_mainContainer.CriterionRepository.FirstOrDefault(criterionT=>criterionT.Id==subCriterion.Criterion.Id),
                Subject = subCriterion.Subject
            });
        }
        /// <summary>
        /// Adds the sub criterion modification.
        /// </summary>
        /// <param name="subCriterion">The sub criterion.</param>
        private void AddSubCriterionModification(AddSubCriterionViewModel subCriterion)
        {
            _mainContainer.SubCriterionModificationRepository.Add(new SubCriterionModification()
            {
                Add = true,
                SubCriterion =
                    _mainContainer.SubCriterionRepository.FirstOrDefault(
                        subCriterionT =>
                            subCriterionT.Subject == subCriterion.Subject &&
                            subCriterionT.Criterion.Id == subCriterion.Criterion.Id),
                Time = DateTime.Now,
                User = GetCurrentUser()
            });
        }
        #endregion

        #region Indicator Region   
        ///GET     
        /// <summary>
        /// Adds the indicator.
        /// </summary>
        /// <returns></returns>
        public ActionResult AddIndicator()//int criterionId)
        {
            return PartialView();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified indicator. and go to add another indicator for specific sub-criterion
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewIndicator(AddIndicatorViewModel indicator)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", new AddCriteriaSubCriterionIndicators() { NavigationViewName = NavigationViewName.AddIndicator, SubCriterion = indicator.SubCriterion });
            }
            AddIndicatorRepository(indicator);
            _mainContainer.SaveChanges();
            AddIndicatorModification(indicator);
            await _mainContainer.SaveChangesAsync();
            return View("Create",new AddCriteriaSubCriterionIndicators() {NavigationViewName = NavigationViewName.AddIndicator,SubCriterion = indicator.SubCriterion}); 
        }
        ///POST
        /// <summary>
        /// Adds the indicator and go ahead.add indicator and go to add another criterion
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddIndicatorAndGoAhead(AddIndicatorViewModel indicator)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("AddIndicator", indicator);
            }
            AddIndicatorRepository(indicator);
            _mainContainer.SaveChanges();
            AddIndicatorModification(indicator);
            await _mainContainer.SaveChangesAsync();
            return  View("Create", new AddCriteriaSubCriterionIndicators() { NavigationViewName = NavigationViewName.AddCriterion,Committee =
                _mainContainer.CriterionRepository.FirstOrDefault(criterionT=>criterionT.Id==indicator.SubCriterion.CriterionId).Committee});
        }
        ///POST
        /// <summary>
        /// Adds the indicator and end.
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddIndicatorAndEnd(AddIndicatorViewModel indicator)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("AddIndicator", indicator);
            }
            AddIndicatorRepository(indicator);
            await _mainContainer.SaveChangesAsync();
            AddIndicatorModification(indicator);

            return View("Create");
        }
        /// <summary>
        /// Adds the indicator repository.
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        private void AddIndicatorRepository(AddIndicatorViewModel indicator)
        {
            _mainContainer.IndicatorRepository.Add(new Indicator()
            {
                Coefficient = indicator.Coefficient,
                SubCriterionId = indicator.SubCriterion.Id,
                Subject = indicator.Subject,
                DeadlinePeriod = indicator.DeadLinePeriod
            });
        }
        /// <summary>
        /// Adds the indicator modification.
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        private void AddIndicatorModification(AddIndicatorViewModel indicator)
        {
            _mainContainer.IndicatorModificationRepository.Add(new IndicatorModification()
            {
                User = GetCurrentUser(),
                AddIndicator = true,
                Indicator = _mainContainer.IndicatorRepository.FirstOrDefault(
                    indicatorT =>
                        indicatorT.Subject == indicator.Subject && indicatorT.SubCriterion.Id == indicator.SubCriterion.Id),
                Time = DateTime.Now
            });
        }
        #endregion
    }
}