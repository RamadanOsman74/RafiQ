using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddUserToRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId is required.");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name is required.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"No user found with ID: {userId}");

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
                throw new InvalidOperationException($"User is already in role '{roleName}'.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception("Failed to assign role. " + string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name is required.");

            var users = await _userManager.GetUsersInRoleAsync(roleName);

            return users.ToList();
        }
    }
}
