using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface ICartProductRepository : IGenericRepository<CartProduct>
    {
        Task<CartProduct> GetByIdAsync(int cartId, int productId);
        Task<IEnumerable<CartProduct>> GetProductsByCartIdAsync(int cartId);
        Task UpdateQuantityAsync(int cartId, int productId, int quantity);
        Task RemoveAllByCartIdAsync(int cartId);
        Task<bool> ProductExistsInCartAsync(int cartId, int productId);
        Task<int> GetTotalQuantityAsync(int cartId);
    }
}
