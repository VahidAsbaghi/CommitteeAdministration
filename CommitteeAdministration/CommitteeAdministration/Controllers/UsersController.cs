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
    /// Users Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly IMainContainer _mainContainer= ModelContainer.Instance.Resolve<IMainContainer>();
        // GET: Users        
        /// <summary>
        /// List all Users
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var users = _mainContainer.UserRepository.AllIncluding(u => u.Committee).Include(u => u.ContactInfo);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5        
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
            var user = await _mainContainer.UserRepository.FirstOrDefaultAsync(t=>t.Id==id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.CommitteeRefId = new SelectList(_mainContainer.CommitteeRepository.All(), "Id", "Name");
            ViewBag.Id = new SelectList(_mainContainer.ContactInfoRepository.All(), "UserId", "City");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Gender,Name,LastName,CreatedTime,IsActive,LastModificationDate,CommitteeRefId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.UserRepository.Add(user);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CommitteeRefId = new SelectList(_mainContainer.CommitteeRepository.All(), "Id", "Name", user.CommitteeRefId);
            ViewBag.Id = new SelectList(_mainContainer.ContactInfoRepository.All(), "UserId", "City", user.Id);
            return View(user);
        }

        // GET: Users/Edit/5        
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
            var user = await _mainContainer.UserRepository.FirstOrDefaultAsync(usert=>usert.Id==id);//.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommitteeRefId = new SelectList(_mainContainer.CommitteeRepository.All(), "Id", "Name", user.CommitteeRefId);
            ViewBag.Id = new SelectList(_mainContainer.ContactInfoRepository.All(), "UserId", "City", user.Id);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Gender,Name,LastName,CreatedTime,IsActive,LastModificationDate,CommitteeRefId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                _mainContainer.Entry(user).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CommitteeRefId = new SelectList(_mainContainer.CommitteeRepository.All(), "Id", "Name", user.CommitteeRefId);
            ViewBag.Id = new SelectList(_mainContainer.ContactInfoRepository.All(), "UserId", "City", user.Id);
            return View(user);
        }

        // GET: Users/Delete/5        
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
            var user = await _mainContainer.UserRepository.FirstOrDefaultAsync(u=>u.Id==id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5        
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await _mainContainer.UserRepository.FirstOrDefaultAsync(u => u.Id == id);
            _mainContainer.UserRepository.Delete(user);
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
