using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone_Project_PROG36944.Data;
using Capstone_Project_PROG36944.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TaskItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TaskItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TaskItems.Include(t => t.AssignedEmployee).Include(t => t.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.AssignedEmployee)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TaskItemId == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItems/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AssignedEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Email");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name");
            return View();
        }

        // POST: TaskItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskItemId,Title,Description,Priority,Status,DueDate,ProjectId,AssignedEmployeeId")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Email", taskItem.AssignedEmployeeId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", taskItem.ProjectId);
            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var taskItemModel = await _context.TaskItems.FindAsync(id);
            if (taskItemModel == null)
            {
                return NotFound();
            }
            return View(taskItemModel);
        }

        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("TaskItemId,Title,Description,Priority,Status,DueDate,ProjectId,AssignedEmployeeId")] TaskItem taskItem)
        {
            if (id != taskItem.TaskItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.TaskItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Email", taskItem.AssignedEmployeeId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", taskItem.ProjectId);
            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .Include(t => t.AssignedEmployee)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TaskItemId == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.TaskItemId == id);
        }
    }
}
