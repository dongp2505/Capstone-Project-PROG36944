using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project_PROG36944.Data;
using Capstone_Project_PROG36944.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EmployeesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesAPIController> _logger;

        public EmployeesAPIController(ApplicationDbContext context, ILogger<EmployeesAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EmployeesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            _logger.LogInformation("API GET /EmployeesAPI called by user {User}", User?.Identity?.Name);
            var list = await _context.Employees.ToListAsync();

            _logger.LogInformation("API GET /EmployeesAPI returning {Count} employees", list.Count);
            return list;
        }

        // GET: api/EmployeesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            _logger.LogInformation("API GET /EmployeesAPI/{Id} called", id);

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                _logger.LogWarning("API GET /EmployeesAPI/{Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("API GET /EmployeesAPI/{Id} returned employee", id);
            return employee;
        }

        // PUT: api/EmployeesAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                _logger.LogWarning("API PUT mismatch: route id {RouteId}, model id {ModelId}", id, employee.EmployeeId);
                return BadRequest();
            }

            _logger.LogInformation("API PUT /EmployeesAPI/{Id} updating employee", id);

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("API PUT /EmployeesAPI/{Id} updated successfully", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeExists(id))
                {
                    _logger.LogWarning("API PUT concurrency: employee {Id} no longer exists", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "API PUT concurrency error for employee {Id}", id);
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmployeesAPI
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _logger.LogInformation("API POST /EmployeesAPI creating new employee");

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("API POST new employee created with id {Id}", employee.EmployeeId);

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/EmployeesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            _logger.LogInformation("API DELETE /EmployeesAPI/{Id} called", id);

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("API DELETE /EmployeesAPI/{Id} employee not found", id);
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("API DELETE employee {Id} deleted successfully", id);
            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
