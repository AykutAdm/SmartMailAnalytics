using Microsoft.AspNetCore.Mvc;

namespace SmartMailAnalytics.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
