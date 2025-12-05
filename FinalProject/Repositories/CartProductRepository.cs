using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories
{
    public class CartProductRepository : GenericRepository<CartProduct>, ICartProductRepository
    {
        public CartProductRepository(EcommerceContext db) : base(db)
        {
        }
        public async Task<CartProduct> GetByIdAsync(int cartId, int productId)
        {
            return await db.cartProducts
                .FirstOrDefaultAsync(cp => cp.CartId == cartId && cp.ProductId == productId);
        }

        public async Task<IEnumerable<CartProduct>> GetProductsByCartIdAsync(int cartId)
        {
            return await db.cartProducts
                .Where(cp => cp.CartId == cartId)
                .ToListAsync();
        }

        public Task<int> GetTotalQuantityAsync(int cartId)
        {
            return db.cartProducts
                .Where(cp => cp.CartId == cartId)
                .SumAsync(cp => cp.quantity);
        }

        public Task<bool> ProductExistsInCartAsync(int cartId, int productId)
        {
            return db.cartProducts
                .AnyAsync(cp => cp.CartId == cartId && cp.ProductId == productId);
        }

        public async Task RemoveAllByCartIdAsync(int cartId)
        {
            await db.cartProducts
                .Where(cp => cp.CartId == cartId)
                .ForEachAsync(cp => db.cartProducts.Remove(cp));
        }

        public async Task UpdateQuantityAsync(int cartId, int productId, int quantity)
        {
            await db.cartProducts
                .Where(cp => cp.CartId == cartId && cp.ProductId == productId)
                .ForEachAsync(cp => cp.quantity = quantity);
        }
    }
}
