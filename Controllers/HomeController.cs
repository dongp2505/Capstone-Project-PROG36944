using System.Diagnostics;
using Capstone_Project_PROG36944.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Project_PROG36944.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home/Index accessed at {Time}", DateTime.UtcNow);
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Home/Privacy accessed at {Time}", DateTime.UtcNow);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                _logger.LogError(
                    exceptionFeature.Error,
                    "Unhandled exception on path {Path} with RequestId {RequestId}",
                    exceptionFeature.Path,
                    Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            }

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
