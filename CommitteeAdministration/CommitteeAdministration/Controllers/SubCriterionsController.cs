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
    /// sub criterion cotroller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize(Roles = "SuperAdmin,Manager")]
    public class SubCriterionsController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();

        // GET: SubCriterions        
        /// <summary>
        /// list all
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var subCriterions = _mainContainer.SubCriterionRepository.AllIncluding(subCriterion => subCriterion.Criterion);
            return View(await subCriterions.ToListAsync());
        }

        // GET: SubCriterions/Details/5        
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
            var subCriterion = await _mainContainer.SubCriterionRepository.FirstOrDefaultAsync(subCriterions=>subCriterions.Id==id);
            if (subCriterion == null)
            {
                return HttpNotFound();
            }
            return View(subCriterion);
        }

        // GET: SubCriterions/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.CriterionId = new SelectList(_mainContainer.CriterionRepository.All(), "Id", "Subject");
            return View();
        }

        // POST: SubCriterions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified sub criterion.
        /// </summary>
        /// <param name="subCriterion">The sub criterion.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Subject,Coefficient,IsDeleted,CriterionId")] SubCriterion subCriterion)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.SubCriterionRepository.Add(subCriterion);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CriterionId = new SelectList(_mainContainer.CriterionRepository.All(), "Id", "Subject", subCriterion.CriterionId);
            return View(subCriterion);
        }

        // GET: SubCriterions/Edit/5        
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
            var subCriterion = await _mainContainer.SubCriterionRepository.FirstOrDefaultAsync(subCriterions => subCriterions.Id == id);
            if (subCriterion == null)
            {
                return HttpNotFound();
            }
            ViewBag.CriterionId = new SelectList(_mainContainer.CriterionRepository.All(), "Id", "Subject", subCriterion.CriterionId);
            return View(subCriterion);
        }

        // POST: SubCriterions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified sub criterion.
        /// </summary>
        /// <param name="subCriterion">The sub criterion.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Subject,Coefficient,IsDeleted,CriterionId")] SubCriterion subCriterion)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.Entry(subCriterion).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CriterionId = new SelectList(_mainContainer.CriterionRepository.All(), "Id", "Subject", subCriterion.CriterionId);
            return View(subCriterion);
        }

        // GET: SubCriterions/Delete/5        
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
            var subCriterion = await _mainContainer.SubCriterionRepository.FirstOrDefaultAsync(subCriterions => subCriterions.Id == id);
            if (subCriterion == null)
            {
                return HttpNotFound();
            }
            return View(subCriterion);
        }

        // POST: SubCriterions/Delete/5        
        /// <summary>
        /// Delete confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var subCriterion = await _mainContainer.SubCriterionRepository.FirstOrDefaultAsync(subCriterions => subCriterions.Id == id);
            _mainContainer.SubCriterionRepository.Delete(subCriterion);
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
