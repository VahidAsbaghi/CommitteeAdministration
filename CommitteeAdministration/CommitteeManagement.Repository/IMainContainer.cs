using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Repository.Data;
using CommitteeManagement.Repository.Repositories;

namespace CommitteeManagement.Repository
{
    /// <summary>
    /// Main container of unit of work
    /// </summary>
    /// <seealso cref="CommitteeManagement.Repository.Data.IRepositoryContainerBase" />
    /// <seealso cref="System.IDisposable" />
    public interface IMainContainer:IRepositoryContainerBase,IDisposable
    {
        /// <summary>
        /// Gets or sets the user repository.
        /// </summary>
        /// <value>
        /// The user repository.
        /// </value>
        UserRepository UserRepository { get; set; }
        /// <summary>
        /// Gets or sets the committee repository.
        /// </summary>
        /// <value>
        /// The committee repository.
        /// </value>
        CommitteeRepository CommitteeRepository { get; set; }
        /// <summary>
        /// Gets or sets the contact information repository.
        /// </summary>
        /// <value>
        /// The contact information repository.
        /// </value>
        ContactInfoRepository ContactInfoRepository { get; set; }
        /// <summary>
        /// Gets or sets the visitor repository.
        /// </summary>
        /// <value>
        /// The visitor repository.
        /// </value>
        VisitorRepository VisitorRepository { get; set; }
        /// <summary>
        /// Gets or sets the sub criterion modification repository.
        /// </summary>
        /// <value>
        /// The sub criterion modification repository.
        /// </value>
        SubCriterionModificationRepository SubCriterionModificationRepository { get; set; }
        /// <summary>
        /// Gets or sets the sub criterion repository.
        /// </summary>
        /// <value>
        /// The sub criterion repository.
        /// </value>
        SubCriterionRepository SubCriterionRepository { get; set; }
        /// <summary>
        /// Gets or sets the session repository.
        /// </summary>
        /// <value>
        /// The session repository.
        /// </value>
        SessionRepository SessionRepository { get; set; }
        /// <summary>
        /// Gets or sets the role repository.
        /// </summary>
        /// <value>
        /// The role repository.
        /// </value>
        RoleRepository RoleRepository { get; set; }
        /// <summary>
        /// Gets or sets the permission repository.
        /// </summary>
        /// <value>
        /// The permission repository.
        /// </value>
        PermissionRepository PermissionRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator real value repository.
        /// </summary>
        /// <value>
        /// The indicator real value repository.
        /// </value>
        IndicatorRealValueRepository IndicatorRealValueRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator modification repository.
        /// </summary>
        /// <value>
        /// The indicator modification repository.
        /// </value>
        IndicatorModificationRepository IndicatorModificationRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator ideal value repository.
        /// </summary>
        /// <value>
        /// The indicator ideal value repository.
        /// </value>
        IndicatorIdealValueRepository IndicatorIdealValueRepository { get; set; }
        /// <summary>
        /// Gets or sets the indicator repository.
        /// </summary>
        /// <value>
        /// The indicator repository.
        /// </value>
        IndicatorRepository IndicatorRepository { get; set; }
        /// <summary>
        /// Gets or sets the criterion modification repository.
        /// </summary>
        /// <value>
        /// The criterion modification repository.
        /// </value>
        CriterionModificationRepository CriterionModificationRepository { get; set; }
        /// <summary>
        /// Gets or sets the criterion repository.
        /// </summary>
        /// <value>
        /// The criterion repository.
        /// </value>
        CriterionRepository CriterionRepository { get; set; }
        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
