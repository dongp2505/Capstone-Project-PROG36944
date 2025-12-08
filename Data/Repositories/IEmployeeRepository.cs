using Capstone_Project_PROG36944.Models;

namespace Capstone_Project_PROG36944.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
    }
}
