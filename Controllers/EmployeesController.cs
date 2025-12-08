using Capstone_Project_PROG36944.Data.Repositories;
using Capstone_Project_PROG36944.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _repo;

        public EmployeesController(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _repo.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,Email,Position,HireDate")] Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);

            await _repo.AddAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _repo.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Email,Position,HireDate")] Employee employee)
        {
            if (id != employee.EmployeeId) return NotFound();

            if (!ModelState.IsValid) return View(employee);

            try
            {
                await _repo.UpdateAsync(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repo.ExistsAsync(employee.EmployeeId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _repo.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
