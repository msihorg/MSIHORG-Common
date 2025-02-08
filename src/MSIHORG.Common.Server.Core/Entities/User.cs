using MSIHORG.Common.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Server.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneConfirmed { get; set; }

        // Public get, private set
        private readonly HashSet<UserRole> _userRoles = new();
        public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles.ToList().AsReadOnly();


        // Methods to manage the collection
        // Return Result type for better error handling
        public Result AddRole(UserRole role)
        {
            if (role == null)
                return Result.Failure("Role cannot be null");

            if (_userRoles.Any(ur => ur.RoleId == role.RoleId))
                return Result.Failure("Role already exists for this user");

            if (_userRoles.Count >= 5)
                return Result.Failure("User cannot have more than 5 roles");

            _userRoles.Add(role);
            return Result.Success();
        }

        public Result RemoveRole(UserRole role)
        {
            if (role == null)
                return Result.Failure("Role cannot be null");

            var existingRole = _userRoles.FirstOrDefault(ur => ur.RoleId == role.RoleId);
            if (existingRole == null)
                return Result.Failure("Role not found for this user");

            if (IsLastAdminRole(existingRole))
                return Result.Failure("Cannot remove the last admin role");

            _userRoles.Remove(existingRole);
            return Result.Success();
        }

        private bool IsLastAdminRole(UserRole role)
        {
            return role.Role?.Name == "Admin" &&
                   _userRoles.Count(ur => ur.Role?.Name == "Admin") == 1;
        }
    }
}
