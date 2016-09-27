using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;

namespace CommitteeManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMainContainer _mainContainer;
        private DataContext db = new DataContext();

        public UsersController(IMainContainer mainContainer)
        {
            _mainContainer = mainContainer;
        }

        // GET: Users
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.Committee).Include(u => u.ContactInfo).Include(u => u.Password);
            return View(user.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CommitteeRefId = new SelectList(db.Committees, "Id", "Name");
            ViewBag.Id = new SelectList(db.ContactInfoes, "UserId", "City");
            ViewBag.Id = new SelectList(db.Passwords, "UserId", "PasswordPhrase");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Gender,Name,LastName,Email,CreatedTime,IsActive,LastModificationDate,CommitteeRefId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommitteeRefId = new SelectList(db.Committees, "Id", "Name", user.CommitteeRefId);
            ViewBag.Id = new SelectList(db.ContactInfoes, "UserId", "City", user.Id);
            ViewBag.Id = new SelectList(db.Passwords, "UserId", "PasswordPhrase", user.Id);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommitteeRefId = new SelectList(db.Committees, "Id", "Name", user.CommitteeRefId);
            ViewBag.Id = new SelectList(db.ContactInfoes, "UserId", "City", user.Id);
            ViewBag.Id = new SelectList(db.Passwords, "UserId", "PasswordPhrase", user.Id);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Gender,Name,LastName,Email,CreatedTime,IsActive,LastModificationDate,CommitteeRefId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommitteeRefId = new SelectList(db.Committees, "Id", "Name", user.CommitteeRefId);
            ViewBag.Id = new SelectList(db.ContactInfoes, "UserId", "City", user.Id);
            ViewBag.Id = new SelectList(db.Passwords, "UserId", "PasswordPhrase", user.Id);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
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
