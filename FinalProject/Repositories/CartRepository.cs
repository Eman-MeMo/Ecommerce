using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(EcommerceContext db) : base(db)
        {
        }
        public async Task<bool> CartExistsForUserAsync(int userId)
        {
           return await db.carts
                .AnyAsync(c => c.UserId == userId);
        }

        public async Task ClearCartAsync(int cartId)
        {
            await db.cartProducts
                .Where(cp => cp.CartId == cartId)
                .ForEachAsync(cp => db.cartProducts.Remove(cp));
        }

        public Task<Cart> GetActiveCartForUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Cart> GetByUserIdAsync(int userId)
        {
            return await db.carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public Task<Cart> GetCartWithProductsAsync(int cartId)
        {
            return db.carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.id == cartId);
        }
    }
}
