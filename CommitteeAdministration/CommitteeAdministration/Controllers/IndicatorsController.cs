using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.Helper;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// Indicator controller to control all works on indicators
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
   // [Authorize(Roles = "SuperAdmin,Manager")]
    public class IndicatorsController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        // GET: Indicators        
        /// <summary>
        /// list of all indicators
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var indicators = _mainContainer.IndicatorRepository.AllIncluding(i => i.SubCriterion);
            return View(await indicators.ToListAsync());
        }

        // GET: Indicators/Details/5        
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
            var indicator = await _mainContainer.IndicatorRepository.FirstOrDefaultAsync(indicatorT=>indicatorT.Id==id);
            if (indicator == null)
            {
                return HttpNotFound();
            }
            return View(indicator);
        }
        /// <summary>
        /// Creates the sub criterion as a partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateIndicatorPartial(int subCriterionId)
        {
            var subCriterion = _mainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == subCriterionId);
            var model = new Indicator()
            {
                Committee = subCriterion.Criterion.Committee,
                SubCriterion = subCriterion,
            };
            return PartialView(model);
        }
        /// <summary>
        /// Creates the sub criterion as a partial view.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateIndicatorPartial(Indicator indicator)
        {
            if (ModelState.IsValid)
            {
                //var subCritereionModel =(SubCriterion)JsonConvert.DeserializeObject<SubCriterion>(subCriterion.ToString());
                indicator.CommitteeId = indicator.Committee.Id;
                indicator.SubCriterionId = indicator.SubCriterion.Id;
                _mainContainer.IndicatorRepository.Add(indicator);
                await _mainContainer.SaveChangesAsync();
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));//RedirectToAction("ViewAll","CriteriaScInd");
            }

            // ViewBag.CriterionId = new SelectList(_mainContainer.CriterionRepository.All(), "Id", "Subject", subCriterion.CriterionId);
            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }

        [HttpGet]
        public ActionResult EditIndicatorPartial(int indicatorId)
        {
            var indicator = _mainContainer.IndicatorRepository.FirstOrDefault(indicatorT => indicatorT.Id == indicatorId);
            return PartialView(indicator);
        }

        [HttpPost]
        public ActionResult EditIndicatorPartial(Indicator postIndicator)
        {
            if (ModelState.IsValid)
            {
                var indicator = _mainContainer.IndicatorRepository.FirstOrDefault(indicatorT => indicatorT.Id == postIndicator.Id);
                indicator.Subject = postIndicator.Subject;
                _mainContainer.IndicatorRepository.Attach(indicator);
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
            }

            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }
        /// <summary>
        /// Deletes the specific indicator in tree view of all data of committee.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeletePartial(int? id)
        {
            if (id == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
            Indicator indicator  =await  _mainContainer.IndicatorRepository.FirstOrDefaultAsync(indicatorT => indicatorT.Id == id);
            if (indicator == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError));//HttpNotFound());
            }
           indicator.IsDeleted = true;
            _mainContainer.IndicatorRepository.Attach(indicator);

            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));// View(criterion);
        }
        // GET: Indicators/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.SubCriterionId = new SelectList(_mainContainer.SubCriterionRepository.All(), "Id", "Subject");
            return View();
        }

        // POST: Indicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified indicator.
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Subject,Coefficient,DeadlinePeriod,IsDeleted,SubCriterionId")] Indicator indicator)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.IndicatorRepository.Add(indicator);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SubCriterionId = new SelectList(_mainContainer.SubCriterionRepository.All(), "Id", "Subject", indicator.SubCriterionId);
            return View(indicator);
        }

        // GET: Indicators/Edit/5        
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
            var indicator = await _mainContainer.IndicatorRepository.FirstOrDefaultAsync(indicatorT => indicatorT.Id == id);
            if (indicator == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubCriterionId = new SelectList(_mainContainer.SubCriterionRepository.All(), "Id", "Subject", indicator.SubCriterionId);
            return View(indicator);
        }

        // POST: Indicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.\        
        /// <summary>
        /// Edits the specified indicator.
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Subject,Coefficient,DeadlinePeriod,IsDeleted,SubCriterionId")] Indicator indicator)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.Entry(indicator).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubCriterionId = new SelectList(_mainContainer.SubCriterionRepository.All(), "Id", "Subject", indicator.SubCriterionId);
            return View(indicator);
        }

        // GET: Indicators/Delete/5        
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var indicator = await _mainContainer.IndicatorRepository.FirstOrDefaultAsync(indicatorT => indicatorT.Id == id);
            if (indicator == null)
            {
                return HttpNotFound();
            }
            return View(indicator);
        }

        // POST: Indicators/Delete/5        
        /// <summary>
        /// Delete confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Indicator indicator = await _mainContainer.IndicatorRepository.FirstOrDefaultAsync(indicatorT => indicatorT.Id == id);
            _mainContainer.IndicatorRepository.Delete(indicator);
            await _mainContainer.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
           
        }
    }
}
