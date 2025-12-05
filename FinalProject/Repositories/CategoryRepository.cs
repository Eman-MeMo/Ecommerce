using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(EcommerceContext db) : base(db)
        {
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await db.categories
                .FirstOrDefaultAsync(c => c.name == name);
        }
    }
}
