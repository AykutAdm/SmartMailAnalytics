using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMailAnalytics.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var mailCountResp = await client.GetAsync("https://localhost:7161/api/Mails/GetMailCount");
            var mailCountJson = await mailCountResp.Content.ReadAsStringAsync();
            ViewBag.MailCount = JsonSerializer.Deserialize<int>(mailCountJson);

            var mailWeekResp = await client.GetAsync("https://localhost:7161/api/Mails/GetMailCountByDate");
            var mailWeekJson = await mailWeekResp.Content.ReadAsStringAsync();
            ViewBag.MailCountWeek = JsonSerializer.Deserialize<int>(mailWeekJson);

            var userCountResp = await client.GetAsync("https://localhost:7161/api/Users/GetUserCount");
            var userCountJson = await userCountResp.Content.ReadAsStringAsync();
            ViewBag.UserCount = JsonSerializer.Deserialize<int>(userCountJson);

            return View();
        }
    }
}
