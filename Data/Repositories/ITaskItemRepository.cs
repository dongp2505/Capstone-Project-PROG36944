using Capstone_Project_PROG36944.Models;

namespace Capstone_Project_PROG36944.Data.Repositories
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
