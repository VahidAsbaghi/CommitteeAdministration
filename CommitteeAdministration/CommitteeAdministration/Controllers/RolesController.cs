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
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;

namespace CommitteeAdministration.Controllers
{
    public class RolesController : Controller
    {
        private readonly IMainContainer _mainContainer;
        //private DataContext db = new DataContext();

        public RolesController(IMainContainer mainContainer)
        {
            _mainContainer = mainContainer;
        }

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            return View(await Task.Run(()=>_mainContainer.RoleRepository.All()));
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await _mainContainer.RoleRepository.FirstOrDefaultAsync(rolet=>rolet.Id==id);//.Roles.FindAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CreationDate")] Role role)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.RoleRepository.Add(role);//.Roles.Add(role);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await _mainContainer.RoleRepository.FirstOrDefaultAsync(rolet => rolet.Id == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CreationDate")] Role role)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.Entry(role).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await _mainContainer.RoleRepository.FirstOrDefaultAsync(rolet => rolet.Id == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Role role = await _mainContainer.RoleRepository.FirstOrDefaultAsync(rolet => rolet.Id == id);
            _mainContainer.RoleRepository.Delete(role);//.Roles.Remove(role);
            await _mainContainer.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
