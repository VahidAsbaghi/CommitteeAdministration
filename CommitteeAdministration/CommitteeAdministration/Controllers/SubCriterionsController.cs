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
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// sub criterion cotroller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
   // [Authorize(Roles = "SuperAdmin,Manager")]
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
        private User GetCurrentUser()
        {
            return _mainContainer.UserRepository.FirstOrDefault(user => user.UserName == HttpContext.User.Identity.Name);
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
        /// <summary>
        /// Creates the sub criterion as a partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateSubCriterionPartial(int criterionId)
        {
            var criterion = _mainContainer.CriterionRepository.FirstOrDefault(criterionT => criterionT.Id == criterionId);
            var model = new CreateSubCriterionViewModel
            {
                Committee = criterion.Committee,
                Criterion = criterion,
            };
            return PartialView(model);
        }
        /// <summary>
        /// Creates the sub criterion as a partial view.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSubCriterionPartial(CreateSubCriterionViewModel subCriterion)
        {
            if (ModelState.IsValid)
            {
                if (subCriterion.Coefficient == null)
                    return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
                var subCriterionT = new SubCriterion
                {
                    Committee = subCriterion.Committee,
                    CommitteeId = subCriterion.Committee.Id,
                    Criterion = subCriterion.Criterion,
                    CriterionId = subCriterion.Criterion.Id,
                    Coefficient = subCriterion.Coefficient.Value,
                    Subject = subCriterion.Subject
                };

                _mainContainer.SubCriterionRepository.Add(subCriterionT);
                SubCriterionModification subCriterionModification = new SubCriterionModification()
                {
                    Add = true,
                    SubCriterion = subCriterionT,
                    Time = DateTime.Now,
                    User = GetCurrentUser(),
                    
                };
                _mainContainer.SubCriterionModificationRepository.Add(subCriterionModification);
                await _mainContainer.SaveChangesAsync();
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));//RedirectToAction("ViewAll","CriteriaScInd");
            }

            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }
        [HttpGet]
        public ActionResult EditSubCriterionPartial(int subCriterionId)
        {
            var subCriterion =
                _mainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == subCriterionId);
            return PartialView(subCriterion);
        }
        [HttpPost]
        public ActionResult EditSubCriterionPartial(SubCriterion postSubCriterion)
        {
            if (ModelState.IsValid)
            {
                var subCriterion = _mainContainer.SubCriterionRepository.FirstOrDefault(subCriterionT => subCriterionT.Id == postSubCriterion.Id);
                subCriterion.Subject = postSubCriterion.Subject;
                _mainContainer.SubCriterionRepository.Attach(subCriterion);
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
            }

            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }
        /// <summary>
        /// Deletes the specific subcriterion in tree view of all data of committee.
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
            SubCriterion subCriterion = await _mainContainer.SubCriterionRepository.FirstOrDefaultAsync(subcriterionT => subcriterionT.Id == id);
            if (subCriterion == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError));//HttpNotFound());
            }
            subCriterion.IsDeleted = true;
            _mainContainer.SubCriterionRepository.Attach(subCriterion);

            var indicators =
                _mainContainer.IndicatorRepository.Where(indicator => indicator.SubCriterion.Id == subCriterion.Id);
            
            foreach (var indicator in indicators)
            {
                indicator.IsDeleted = true;
                _mainContainer.IndicatorRepository.Attach(indicator);
            }
            
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));// View(criterion);
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
