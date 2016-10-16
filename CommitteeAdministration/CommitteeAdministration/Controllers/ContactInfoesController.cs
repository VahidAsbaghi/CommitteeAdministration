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
    /// controller of contact info to do all db related jobs with contact infos
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize(Roles = "SuperAdmin")]
    public class ContactInfoesController : Controller
    {        
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        // GET: ContactInfoes        
        /// <summary>
        /// the main controller that is shows details of contact info in the related view
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var contactInfoes = _mainContainer.ContactInfoRepository.AllIncluding(contactInfo=>contactInfo.User);
            return View(await contactInfoes.ToListAsync());
        }

        // GET: ContactInfoes/Details/5        
        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = await _mainContainer.ContactInfoRepository.FirstOrDefaultAsync(contactInfoT=>contactInfoT.UserId==id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            return View(contactInfo);
        }

        // GET: ContactInfoes/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(_mainContainer.UserRepository.All(), "Id", "Name");
            return View();
        }

        // POST: ContactInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified contact information.
        /// </summary>
        /// <param name="contactInfo">The contact information.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,City,Region,Address1,Address2,PhotoLink,ModifiedTime")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.ContactInfoRepository.Add(contactInfo);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(_mainContainer.UserRepository.All(), "Id", "Name", contactInfo.UserId);
            return View(contactInfo);
        }

        // GET: ContactInfoes/Edit/5        
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = await _mainContainer.ContactInfoRepository.FirstOrDefaultAsync(contactInfoT => contactInfoT.UserId == id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(_mainContainer.UserRepository.All(), "Id", "Name", contactInfo.UserId);
            return View(contactInfo);
        }

        // POST: ContactInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified contact information.
        /// </summary>
        /// <param name="contactInfo">The contact information.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,City,Region,Address1,Address2,PhotoLink,ModifiedTime")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.Entry(contactInfo).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(_mainContainer.UserRepository.All(), "Id", "Name", contactInfo.UserId);
            return View(contactInfo);
        }

        // GET: ContactInfoes/Delete/5        
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = await _mainContainer.ContactInfoRepository.FirstOrDefaultAsync(contactInfoT => contactInfoT.UserId == id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            return View(contactInfo);
        }

        // POST: ContactInfoes/Delete/5        
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ContactInfo contactInfo = await _mainContainer.ContactInfoRepository.FirstOrDefaultAsync(contactInfoT => contactInfoT.UserId == id);
            _mainContainer.ContactInfoRepository.Delete(contactInfo);
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
