using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommitteeAdministration.ActionFilters;
using CommitteeAdministration.Helper;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// Controller of committe model
    /// the access of this controller is only assigned to super admin
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize(Roles = "SuperAdmin")]
    public class CommitteesController : Controller
    {
        private readonly IMainContainer _mainContainer= ModelContainer.Instance.Resolve<IMainContainer>();

        // GET: Committees        
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(await _mainContainer.CommitteeRepository.All().ToListAsync());
        }

        // GET: Committees/Details/5        
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
            var committee = await _mainContainer.CommitteeRepository.FirstOrDefaultAsync(committeeT=>committeeT.Id==id);
            if (committee == null)
            {
                return HttpNotFound();
            }
            return View(committee);
        }

        // GET: Committees/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Committees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified committee.
        /// </summary>
        /// <param name="committee">The committee.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] Committee committee)
        {
            if (!ModelState.IsValid) return View(committee);
            _mainContainer.CommitteeRepository.Add(committee);
            await _mainContainer.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Committees/Edit/5        
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
            var committee = await _mainContainer.CommitteeRepository.FirstOrDefaultAsync(committeeT => committeeT.Id == id);
            if (committee == null)
            {
                return HttpNotFound();
            }
            return View(committee);
        }

        // POST: Committees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified committee.
        /// </summary>
        /// <param name="committee">The committee.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name")] Committee committee)
        {
            if (!ModelState.IsValid) return View(committee);
            _mainContainer.Entry(committee).State = EntityState.Modified;
            await _mainContainer.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Committees/Delete/5        
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
            var committee = await _mainContainer.CommitteeRepository.FirstOrDefaultAsync(committeeT => committeeT.Id == id);
            if (committee == null)
            {
                return HttpNotFound();
            }
            return View(committee);
        }

        // POST: Committees/Delete/5        
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var committee = await _mainContainer.CommitteeRepository.FirstOrDefaultAsync(committeeT => committeeT.Id == id);
            _mainContainer.CommitteeRepository.Delete(committee);
            await _mainContainer.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //   // db.Dispose();
            //}
            //base.Dispose(disposing);
        }
    }
}
