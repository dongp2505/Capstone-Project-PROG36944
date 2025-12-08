using System.Linq;
using System.Threading.Tasks;
using Capstone_Project_PROG36944.Data;
using Capstone_Project_PROG36944.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<EmployeesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Employees/Index called by user {User}", User?.Identity?.Name ?? "Anonymous");
            var employees = await _context.Employees.ToListAsync();
            _logger.LogInformation("Employees/Index returning {Count} employees", employees.Count);
            return View(employees);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Employees/Details called with null id");
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with id {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Employees/Details showing employee {Id}", id);
            return View(employee);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            _logger.LogInformation("Employees/Create GET accessed by {User}", User?.Identity?.Name);
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,Email,Position,HireDate")] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Employees/Create POST received invalid model state");
                return View(employee);
            }

            _context.Add(employee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New employee {Id} created by {User}", employee.EmployeeId, User?.Identity?.Name);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Employees/Edit GET called with null id");
                return NotFound();
            }

            var employeeModel = await _context.Employees.FindAsync(id);
            if (employeeModel == null)
            {
                _logger.LogWarning("Employees/Edit GET: employee {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Employees/Edit GET for employee {Id}", id);
            return View(employeeModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Email,Position,HireDate")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                _logger.LogWarning("Employees/Edit POST id mismatch: route id {RouteId}, model id {ModelId}", id, employee.EmployeeId);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Employees/Edit POST invalid model state for id {Id}", id);
                return View(employee);
            }

            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Employee {Id} updated by {User}", employee.EmployeeId, User?.Identity?.Name);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EmployeeExists(employee.EmployeeId))
                {
                    _logger.LogWarning("Employees/Edit concurrency error: employee {Id} no longer exists", employee.EmployeeId);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Employees/Edit concurrency exception for id {Id}", employee.EmployeeId);
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Employees/Delete GET called with null id");
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employee == null)
            {
                _logger.LogWarning("Employees/Delete GET: employee {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Employees/Delete GET confirmation for {Id}", id);
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Employee {Id} deleted by {User}", id, User?.Identity?.Name);
            }
            else
            {
                _logger.LogWarning("Employees/DeleteConfirmed: employee {Id} not found", id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
