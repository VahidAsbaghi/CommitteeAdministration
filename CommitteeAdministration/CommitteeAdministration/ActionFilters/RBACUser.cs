//using RBACModel;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;

namespace CommitteeAdministration.ActionFilters
{
    /// <summary>
    /// Role Base Access Control User
    /// </summary>
    public class RBACUser
    {
        private readonly IMainContainer _mainContainer;
        public string UserId { get; set; }
        public bool IsSysAdmin { get; set; }
        public string Username { get; set; }
        private List<UserRole> Roles = new List<UserRole>();

        public RBACUser(string username,IMainContainer mainContainer)
        {
            _mainContainer = mainContainer;
            this.Username = username;
            this.IsSysAdmin = false;
            GetDatabaseUserRolesPermissions();
        }

        private void GetDatabaseUserRolesPermissions()
        {
                      
            var user = _mainContainer.UserRepository.Where(u => u.UserName == Username).FirstOrDefault();
            if (user == null) return;
            UserId = user.Id;
            foreach (var role in user.NewRoles)
            {
                var userRole = new UserRole { RoleId = role.Id, RoleName = role.Name };
                foreach (var permission in role.Permissions)
                {
                    userRole.Permissions.Add(new RolePermission
                    {
                        PermissionId = permission.Id, PermissionType = permission.Add.GetValueOrDefault(false)?Enums.PermissionType.Add : 
                            permission.Delete.GetValueOrDefault(false)?Enums.PermissionType.Delete : 
                                permission.Update.GetValueOrDefault(false)?Enums.PermissionType.Update:Enums.PermissionType.None,
                        PermissionObject = permission.Indicator.GetValueOrDefault(false)?Enums.PermissionObject.Indicator : 
                            permission.Criterion.GetValueOrDefault(false)?Enums.PermissionObject.Criterion:
                                permission.IndicatorDeadlineAdjust.GetValueOrDefault(false)?Enums.PermissionObject.IndicatorDeadline : 
                                    permission.RealIndicator.GetValueOrDefault(false)?Enums.PermissionObject.RealIndicator : 
                                        permission.SubCriterion.GetValueOrDefault(false)?Enums.PermissionObject.SubCriterion : Enums.PermissionObject.None
                    });
                }
                Roles.Add(userRole);

                if (!this.IsSysAdmin) ;
                //this.IsSysAdmin = role.IsSysAdmin;
            }
            
        }

        public bool HasPermission(Enums.PermissionType permissionType,int? id,Enums.PermissionObject permissionObject)
        {
            bool permissionOk = false;
            if (id==null)
            {
                foreach (var permissions in from role in Roles select role.Permissions into permissions from permission in permissions.Where(permission => permission.PermissionObject == permissionObject && permission.PermissionType == permissionType) select permissions)
                {
                    permissionOk = true;
                }
            }
            else
            {
                //TODO now we assume id of permission object is null because of complexity
                foreach (var role in Roles)
                {
                    foreach (var permission in role.Permissions)
                    {
                        switch (permissionObject)
                        {
                                case Enums.PermissionObject.Criterion:
                            
                                if (permission.PermissionObject == Enums.PermissionObject.Criterion &&
                                    permission.PermissionType == permissionType && permission.PermissionId == id)
                                {
                                    permissionOk = true;
                                }
                                break;
                            
                                
                            case Enums.PermissionObject.IndicatorDeadline:
                                break;
                            case Enums.PermissionObject.SubCriterion:
                                break;
                            case Enums.PermissionObject.Indicator:
                                break;
                            case Enums.PermissionObject.RealIndicator:
                                break;
                            case Enums.PermissionObject.None:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(permissionObject), permissionObject, null);
                        }
                    }
                }
            }
            return permissionOk;
            
        }

        public bool HasRole(string roleName)
        {
            return (Roles.Where(p => p.RoleName == roleName).ToList().Count > 0);
        }

        public bool HasRoles(string roles)
        {
            var bFound = false;
            var _roles = roles.ToLower().Split(';');
            foreach (UserRole role in this.Roles)
            {
                try
                {
                    bFound = _roles.Contains(role.RoleName.ToLower());
                    if (bFound)
                        return true;
                }
                catch (Exception)
                {
                }
            }
            return bFound;
        }
    }

    public class UserRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RolePermission> Permissions = new List<RolePermission>();
    }

    public class RolePermission
    {
        public int PermissionId { get; set; }
        public Enums.PermissionType PermissionType { get; set; }
        public Enums.PermissionObject PermissionObject { get; set; }
    }
}