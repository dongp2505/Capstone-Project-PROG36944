using Capstone_Project_PROG36944.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        public EmployeeRepository(ApplicationDbContext db) => _db = db;

        public Task<List<Employee>> GetAllAsync()
            => _db.Employees.ToListAsync();

        public Task<Employee?> GetByIdAsync(int id)
            => _db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

        public async Task AddAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _db.Employees.FindAsync(id);
            if (emp != null)
            {
                _db.Employees.Remove(emp);
                await _db.SaveChangesAsync();
            }
        }

        public Task<bool> ExistsAsync(int id)
            => _db.Employees.AnyAsync(e => e.EmployeeId == id);
    }
}
