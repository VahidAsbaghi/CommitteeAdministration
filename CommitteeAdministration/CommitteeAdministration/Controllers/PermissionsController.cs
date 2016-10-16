using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CommitteeAdministration.ActionFilters;
using CommitteeAdministration.Helper;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using CommitteeManagement.Repository.Data;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Controllers
{
    /// <summary>
    /// Permission cotroller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize(Roles = "SuperAdmin")]
    public class PermissionsController : Controller
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        // GET: Permissions        
        /// <summary>
        /// List all permissions
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(await Task.Run(()=>_mainContainer.PermissionRepository.All().ToList()));
        }

        // GET: Permissions/Details/5        
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
            var permission = await _mainContainer.PermissionRepository.FirstOrDefaultAsync(permissionT=>permissionT.Id==id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }

        // GET: Permissions/Create        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {            
            var roles = _mainContainer.RoleRepository.All().ToList();
            var model=new AddPermissionViewModel() {Roles = roles,IsContainedRole = new List<bool>()};
            foreach (var role in roles)
            {
                model.IsContainedRole.Add(new bool());
            }
            return View(model);
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Creates the specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( AddPermissionViewModel permission)
        {
            if (ModelState.IsValid)
            {
                var roles= _mainContainer.RoleRepository.All().ToList();
                var rolesContained=new List<Role>();
                for (var i = 0; i < permission.IsContainedRole.Count; i++)
                {
                    var role = permission.IsContainedRole[i];
                    if (role)
                    {
                        rolesContained.Add(roles[i]);
                    }
                }
                _mainContainer.PermissionRepository.Add(new Permission()
                {
                    Add=permission.Add,Criterion = permission.Criterion,Delete = permission.Delete,
                    IndicatorDeadlineAdjust = permission.IndicatorDeadlineAdjust,Indicator = permission.Indicator,
                    RealIndicator = permission.RealIndicator,Update = permission.Update,SubCriterion = permission.SubCriterion,
                    Roles = rolesContained
                });

                await _mainContainer.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(permission);
        }

        // GET: Permissions/Edit/5        
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
            var permission = await _mainContainer.PermissionRepository.FirstOrDefaultAsync(permissionT => permissionT.Id == id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            var roles = _mainContainer.RoleRepository.All().ToList();
            var isContainedRoles= roles.Select(role => new bool()).ToList();
            for (var i = 0; i < roles.Count; i++)
            {
                if (permission.Roles.Contains(roles[i]))
                {
                    isContainedRoles[i] = true;
                }
            }
            return View(new EditPermissionViewModel() {Permission = permission,IsContainedRole = isContainedRoles,Roles = roles});
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.        
        /// <summary>
        /// Edits the specified permission.
        /// </summary>
        /// <param name="editPermission"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditPermissionViewModel editPermission)
        {
            if (ModelState.IsValid)
            {
                var roles = _mainContainer.RoleRepository.All().ToList();
                var containedRoles = roles.Where((t, i) => editPermission.IsContainedRole[i]).ToList();
                editPermission.Permission.Roles = containedRoles;
                var permission = _mainContainer.PermissionRepository.FindById(editPermission.Permission.Id);
                //Copy the content of permission in edit permission view model into the db instance using mapper
                Mapper.Initialize(expression => expression.CreateMap<Permission, Permission>());
                Mapper.Map(editPermission.Permission, permission);
                permission.Roles = containedRoles;

                _mainContainer.Entry(permission).State = EntityState.Modified;
                await _mainContainer.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(editPermission);
        }

        // GET: Permissions/Delete/5        
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
            var permission = await _mainContainer.PermissionRepository.FirstOrDefaultAsync(permissionT => permissionT.Id == id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }

        // POST: Permissions/Delete/5        
        /// <summary>
        /// Delete confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Permission permission = await _mainContainer.PermissionRepository.FirstOrDefaultAsync(permissionT => permissionT.Id == id);
            _mainContainer.PermissionRepository.Delete(permission);
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
