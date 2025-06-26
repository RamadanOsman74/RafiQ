using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<IEnumerable<DisplayUserDto>> GetAllUsersAsync();
        Task<DisplayUserDto> GetUserByEmailAsync(string email);
        Task<bool> DeleteUserAsync(string userId);
    }
}
