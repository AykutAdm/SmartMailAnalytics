using SmartMailAnalytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Interfaces.UserInterfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(int page = 1);
        Task<User> GetByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<int> GetUserCountAsync();
    }
}
