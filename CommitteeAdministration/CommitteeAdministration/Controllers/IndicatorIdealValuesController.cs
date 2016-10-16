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
    /// Ideal value of indicator controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class IndicatorIdealValuesController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();

        // GET: IndicatorIdealValues        
        /// <summary>
        /// list view of all
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var indicatorIdealValues = _mainContainer.IndicatorIdealValueRepository.AllIncluding(i => i.Indicator);
            return View(await indicatorIdealValues.ToListAsync());
        }

        // GET: IndicatorIdealValues/Details/5        
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
            var indicatorIdealValue = await _mainContainer.IndicatorIdealValueRepository.FirstOrDefaultAsync(idealValueT=>idealValueT.Id==id);
            if (indicatorIdealValue == null)
            {
                return HttpNotFound();
            }
            return View(indicatorIdealValue);
        }

        // GET: IndicatorIdealValues/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject");
            return View();
        }

        // POST: IndicatorIdealValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified indicator ideal value.
        /// </summary>
        /// <param name="indicatorIdealValue">The indicator ideal value.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Value,Time,LowerThan,MoreThan,IndicatorId")] IndicatorIdealValue indicatorIdealValue)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.IndicatorIdealValueRepository.Add(indicatorIdealValue);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject", indicatorIdealValue.IndicatorId);
            return View(indicatorIdealValue);
        }

        // GET: IndicatorIdealValues/Edit/5        
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
            IndicatorIdealValue indicatorIdealValue = await _mainContainer.IndicatorIdealValueRepository.FirstOrDefaultAsync(idealValueT => idealValueT.Id == id);
            if (indicatorIdealValue == null)
            {
                return HttpNotFound();
            }
            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject", indicatorIdealValue.IndicatorId);
            return View(indicatorIdealValue);
        }

        // POST: IndicatorIdealValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified indicator ideal value.
        /// </summary>
        /// <param name="indicatorIdealValue">The indicator ideal value.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Value,Time,LowerThan,MoreThan,IndicatorId")] IndicatorIdealValue indicatorIdealValue)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.Entry(indicatorIdealValue).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IndicatorId = new SelectList(_mainContainer.IndicatorRepository.All(), "Id", "Subject", indicatorIdealValue.IndicatorId);
            return View(indicatorIdealValue);
        }

        // GET: IndicatorIdealValues/Delete/5        
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
            IndicatorIdealValue indicatorIdealValue = await _mainContainer.IndicatorIdealValueRepository.FirstOrDefaultAsync(idealValueT => idealValueT.Id == id);
            if (indicatorIdealValue == null)
            {
                return HttpNotFound();
            }
            return View(indicatorIdealValue);
        }

        // POST: IndicatorIdealValues/Delete/5        
        /// <summary>
        /// Deletes confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var indicatorIdealValue = await _mainContainer.IndicatorIdealValueRepository.FirstOrDefaultAsync(idealValueT => idealValueT.Id == id);
            _mainContainer.IndicatorIdealValueRepository.Delete(indicatorIdealValue);
            await _mainContainer.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            
        }
    }
}
