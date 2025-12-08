using Capstone_Project_PROG36944.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _db;
        public ProjectRepository(ApplicationDbContext db) => _db = db;

        public Task<List<Project>> GetAllAsync()
            => _db.Projects.ToListAsync();

        public Task<Project?> GetByIdAsync(int id)
            => _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);

        public async Task AddAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _db.Projects.FindAsync(id);
            if (project != null)
            {
                _db.Projects.Remove(project);
                await _db.SaveChangesAsync();
            }
        }

        public Task<bool> ExistsAsync(int id)
            => _db.Projects.AnyAsync(p => p.ProjectId == id);
    }
}
