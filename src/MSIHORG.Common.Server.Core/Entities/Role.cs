using MSIHORG.Common.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Server.Core.Entities
{
    public class Role
    {
        private readonly HashSet<UserRole> _userRoles = new();

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }

        // Navigation property
        public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles.ToList().AsReadOnly();

        // Potential validation methods for role management
        public Result UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Failure("Role name cannot be empty");

            Name = newName;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!IsActive)
                return Result.Failure("Role is already inactive");

            IsActive = false;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Activate()
        {
            if (IsActive)
                return Result.Failure("Role is already active");

            IsActive = true;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
