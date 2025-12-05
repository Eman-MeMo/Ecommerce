using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<User> GetUserByUsernameAsync(string username);
    }
}
