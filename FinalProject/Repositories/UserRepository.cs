using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(EcommerceContext db) : base(db)
        {
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await db.Set<User>().FirstOrDefaultAsync(u => u.username == username);
        }
    }
}
