using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommitteeManagement.Model;
using CommitteeManagement.Repository.Data;

namespace CommitteeAdministration.Controllers
{
    public class ContactInfoesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: ContactInfoes
        public async Task<ActionResult> Index()
        {
            var contactInfoes = db.ContactInfoes.Include(c => c.User);
            return View(await contactInfoes.ToListAsync());
        }

        // GET: ContactInfoes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = await db.ContactInfoes.FindAsync(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            return View(contactInfo);
        }

        // GET: ContactInfoes/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: ContactInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,City,Region,Address1,Address2,PhotoLink,ModifiedTime")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                db.ContactInfoes.Add(contactInfo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", contactInfo.UserId);
            return View(contactInfo);
        }

        // GET: ContactInfoes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = await db.ContactInfoes.FindAsync(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", contactInfo.UserId);
            return View(contactInfo);
        }

        // POST: ContactInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,City,Region,Address1,Address2,PhotoLink,ModifiedTime")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactInfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", contactInfo.UserId);
            return View(contactInfo);
        }

        // GET: ContactInfoes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactInfo contactInfo = await db.ContactInfoes.FindAsync(id);
            if (contactInfo == null)
            {
                return HttpNotFound();
            }
            return View(contactInfo);
        }

        // POST: ContactInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ContactInfo contactInfo = await db.ContactInfoes.FindAsync(id);
            db.ContactInfoes.Remove(contactInfo);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
