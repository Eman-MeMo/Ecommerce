using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetByUserIdAsync(int userId);
        Task<Cart> GetActiveCartForUserAsync(int userId);
        Task ClearCartAsync(int cartId);
        Task<bool> CartExistsForUserAsync(int userId);
        Task<Cart> GetCartWithProductsAsync(int cartId);
    }
}
