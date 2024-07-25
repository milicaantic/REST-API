using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Praksa2.Data;
using Praksa2.Models;
using System.Threading.Tasks;

namespace Praksa2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

    }
}
