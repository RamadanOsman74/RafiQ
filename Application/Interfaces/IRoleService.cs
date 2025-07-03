using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        Task AddUserToRoleAsync(string userId, string roleName);
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
    }

}
