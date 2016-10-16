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
    /// main controller of criterion 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CriteriaController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        /// <summary>
        /// main list view of all criteria
        /// </summary>
        /// <returns></returns>
        // GET: Criteria
        public async Task<ActionResult> Index()
        {
            return View(await _mainContainer.CriterionRepository.All().ToListAsync());
        }

        // GET: Criteria/Details/5        
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
            var criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT=>criterionT.Id==id);
            if (criterion == null)
            {
                return HttpNotFound();
            }
            return View(criterion);
        }

        // GET: Criteria/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Criteria/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified criterion.
        /// </summary>
        /// <param name="criterion">The criterion.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Subject,Coefficient,IsDeleted")] Criterion criterion)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.CriterionRepository.Add(criterion);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(criterion);
        }

        // GET: Criteria/Edit/5        
        /// <summary>
        /// Edits the specified criterion with identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT => criterionT.Id == id);
            if (criterion == null)
            {
                return HttpNotFound();
            }
            return View(criterion);
        }

        // POST: Criteria/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified criterion.
        /// </summary>
        /// <param name="criterion">The criterion.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Subject,Coefficient,IsDeleted")] Criterion criterion)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.Entry(criterion).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(criterion);
        }

        // GET: Criteria/Delete/5        
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
            Criterion criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT => criterionT.Id == id);
            if (criterion == null)
            {
                return HttpNotFound();
            }
            return View(criterion);
        }

        // POST: Criteria/Delete/5        
        /// <summary>
        /// Deletes confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Criterion criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT => criterionT.Id == id);
            _mainContainer.CriterionRepository.Delete(criterion);
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
