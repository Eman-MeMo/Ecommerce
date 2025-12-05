using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories
{
    public class ProductRepository:GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(EcommerceContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByCartIdAsync(int cartId)
        {
            return await db.products
                .Where(p => db.cartProducts
                    .Any(cp => cp.CartId == cartId && cp.ProductId == p.id))
                .ToListAsync();
        }
        public async Task<Product> GetProductByName(string name)
        {
            return await db.products
                .FirstOrDefaultAsync(p => p.name == name);
        }
    }
}
