using Capstone_Project_PROG36944.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Data.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly ApplicationDbContext _db;
        public TaskItemRepository(ApplicationDbContext db) => _db = db;

        public Task<List<TaskItem>> GetAllAsync()
            => _db.TaskItems
                .Include(t => t.AssignedEmployee)
                .Include(t => t.Project)
                .ToListAsync();

        public Task<TaskItem?> GetByIdAsync(int id)
            => _db.TaskItems
                .Include(t => t.AssignedEmployee)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TaskItemId == id);

        public async Task AddAsync(TaskItem taskItem)
        {
            _db.TaskItems.Add(taskItem);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            _db.TaskItems.Update(taskItem);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _db.TaskItems.FindAsync(id);
            if (task != null)
            {
                _db.TaskItems.Remove(task);
                await _db.SaveChangesAsync();
            }
        }

        public Task<bool> ExistsAsync(int id)
            => _db.TaskItems.AnyAsync(t => t.TaskItemId == id);
    }
}
