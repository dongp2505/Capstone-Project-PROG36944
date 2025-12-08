using System.Linq;
using System.Threading.Tasks;
using Capstone_Project_PROG36944.Data;
using Capstone_Project_PROG36944.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project_PROG36944.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<ProjectsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Projects/Index accessed by {User}", User?.Identity?.Name);
            var projects = await _context.Projects.ToListAsync();
            _logger.LogInformation("Projects/Index returning {Count} projects", projects.Count);

            return View(projects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Projects/Details called with null id");
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            if (project == null)
            {
                _logger.LogWarning("Project with id {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Projects/Details returning project {Id}", id);
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            _logger.LogInformation("Projects/Create GET accessed by {User}", User?.Identity?.Name);
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Description,StartDate,EndDate")] Project project)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Projects/Create POST invalid model state");
                return View(project);
            }

            _context.Add(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New project {Id} created by {User}", project.ProjectId, User?.Identity?.Name);
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Projects/Edit GET called with null id");
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                _logger.LogWarning("Project {Id} not found for edit", id);
                return NotFound();
            }

            _logger.LogInformation("Projects/Edit GET accessed for project {Id}", id);
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Description,StartDate,EndDate")] Project project)
        {
            if (id != project.ProjectId)
            {
                _logger.LogWarning("Projects/Edit POST mismatch: route {RouteId}, model {ModelId}", id, project.ProjectId);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Projects/Edit POST invalid model state for id {Id}", id);
                return View(project);
            }

            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Project {Id} updated by {User}", project.ProjectId, User?.Identity?.Name);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProjectExists(project.ProjectId))
                {
                    _logger.LogWarning("Project {Id} no longer exists during edit", project.ProjectId);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error editing project {Id}", project.ProjectId);
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Projects/Delete GET called with null id");
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            if (project == null)
            {
                _logger.LogWarning("Project {Id} not found for deletion", id);
                return NotFound();
            }

            _logger.LogInformation("Projects/Delete GET confirmation for project {Id}", id);
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Project {Id} deleted by {User}", id, User?.Identity?.Name);
            }
            else
            {
                _logger.LogWarning("Project {Id} not found for DeleteConfirmed", id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
