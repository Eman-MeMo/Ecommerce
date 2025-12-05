using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        Task<Category> GetCategoryByName(string name);
    }
}
