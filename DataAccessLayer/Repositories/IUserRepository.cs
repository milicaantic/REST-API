using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);

    }
}
