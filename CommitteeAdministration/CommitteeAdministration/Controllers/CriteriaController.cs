﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CommitteeAdministration.Helper;
using CommitteeAdministration.ViewModels;
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
   // [Authorize(Roles = "SuperAdmin,Manager")]
    public class CriteriaController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        /// <summary>
        /// Possible actions on criterion. Used to Insert CriterionModification Row 
        /// </summary>
        private enum CriterionAction
        {
            Add,
            Delete,
            Edit
        }
        /// <summary>
        /// main list view of all criteria
        /// </summary>
        /// <returns></returns>
        // GET: Criteria
        public async Task<ActionResult> Index()
        {
            return View(await _mainContainer.CriterionRepository.All().Where(criterion=> !criterion.IsDeleted.Value || criterion.IsDeleted == null).ToListAsync());
        }
        ///HttpGet
        /// <summary>
        /// Creates the criterion using a partial view.
        /// used in the viewall view that shows all data of committee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateCriterionPartial(int committeeId)
        {
           
            var committee = _mainContainer.CommitteeRepository.FirstOrDefault(committeeT => committeeT.Id == committeeId);
            var model = new CreateCriterionViewModel {Committee = committee};
            return PartialView(model);
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
        public ActionResult CreateCriterionPartial(CreateCriterionViewModel criterion)
        {
            if (ModelState.IsValid)
            {
                var criterionT=new Criterion
                {
                    Coefficient = criterion.Coefficient,
                    Committee = criterion.Committee,
                    CommitteeId = criterion.Committee.Id,
                    Subject = criterion.Subject
                };

                _mainContainer.CriterionRepository.Add(criterionT);
                
                CriterionModification(criterionT, CriterionAction.Add);
                _mainContainer.SaveChangesAsync();
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
            }

            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }
        /// <summary>
        /// Edits the criterion in a partial view.
        /// </summary>
        /// <param name="criterionId">The criterion identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditCriterionPartial(int criterionId)
        {
            var criterion = _mainContainer.CriterionRepository.FirstOrDefault(criterionT => criterionT.Id == criterionId);
            return PartialView(criterion);
        }
        [HttpPost]
        public ActionResult EditCriterionPartial(Criterion postCriterion)
        {
            if (ModelState.IsValid)
            {
                var criterion = _mainContainer.CriterionRepository.FirstOrDefault(criterionT => criterionT.Id == postCriterion.Id);
                criterion.Subject = postCriterion.Subject;
                _mainContainer.CriterionRepository.Attach(criterion);
                return Json(new HttpStatusCodeResult(HttpStatusCode.OK));
            }
            
            return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
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
            var criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT=>criterionT.Id==id && (!criterionT.IsDeleted.Value || criterionT.IsDeleted==null));
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
                CriterionModification(criterion,CriterionAction.Add);
                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(criterion);
        }
        /// <summary>
        /// Criterion modification.
        /// </summary>
        /// <param name="criterion">The criterion.</param>
        /// <param name="action">The action.</param>
        private void CriterionModification(Criterion criterion,CriterionAction action)
        {
            var loginUser =
                _mainContainer.UserRepository.FirstOrDefault(user => user.UserName == HttpContext.User.Identity.Name);
            var criterionModification=new CriterionModification()
            {
                Add=action==CriterionAction.Add,Criterion = criterion,Time = DateTime.Now,
                User = loginUser,UserId = loginUser.Id,
                Delete = action==CriterionAction.Delete,
                Update = action==CriterionAction.Edit
            };
            _mainContainer.CriterionModificationRepository.Add(criterionModification);
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
            var criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT => criterionT.Id == id && (!criterionT.IsDeleted.Value || criterionT.IsDeleted == null));
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
                CriterionModification(criterion, CriterionAction.Edit);
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
        [HttpPost]
        public async Task<ActionResult> DeletePartial(int? id)
        {
            if (id == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
            Criterion criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT => criterionT.Id == id);
            if (criterion == null)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError));//HttpNotFound());
            }
            criterion.IsDeleted = true;
            _mainContainer.CriterionRepository.Attach(criterion);
            var subCriterions =
                _mainContainer.SubCriterionRepository.Where(subCriterion => subCriterion.Criterion.Id == criterion.Id);
            var indicators=new List<Indicator>();
            foreach (var subCriterion in subCriterions)
            {
                subCriterion.IsDeleted = true;
                _mainContainer.SubCriterionRepository.Attach(subCriterion);
                indicators.AddRange(_mainContainer.IndicatorRepository.Where(indicator=>indicator.SubCriterion.Id==subCriterion.Id));
            }
            foreach (var indicator in indicators)
            {
                indicator.IsDeleted = true;
                _mainContainer.IndicatorRepository.Attach(indicator);
            }
           
            
            return Json(new HttpStatusCodeResult(HttpStatusCode.OK));// View(criterion);
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
            var criterion = await _mainContainer.CriterionRepository.FirstOrDefaultAsync(criterionT => criterionT.Id == id);
            criterion.IsDeleted = true;
            _mainContainer.Entry(criterion).State = EntityState.Modified;
            CriterionModification(criterion, CriterionAction.Delete);
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
