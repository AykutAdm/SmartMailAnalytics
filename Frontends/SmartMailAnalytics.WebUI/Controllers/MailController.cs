using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartMailAnalytics.WebUI.DTOs.MailDtos;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.WebUI.Controllers
{
    public class MailController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MailController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> MailList(int page = 1, string q = "", string spam = "all")
        {
            ViewBag.Page = page;
            ViewBag.Q = q;
            ViewBag.Spam = spam;
            var url = $"https://localhost:7161/api/Mails/GetMailsByFilter?Page={page}";
            if (q.Length > 0)
                url += "&Subject=" + Uri.EscapeDataString("%" + q + "%");
            if (spam == "spam")
                url += "&IsSpam=true";
            else if (spam == "clean")
                url += "&IsSpam=false";
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMailDto>>(jsonData);
                return View(values);
            }
            return View();
        }


        [HttpGet]
        public IActionResult CreateMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMail(CreateMailDto createMailDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createMailDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7161/api/Mails", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MailList");
            }
            return View(createMailDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7161/api/Mails/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateMailDto>(jsonData);
                return View(value);
            }
            return RedirectToAction("MailList");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMail(UpdateMailDto updateMailDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateMailDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7161/api/Mails", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MailList");
            }
            return View(updateMailDto);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7161/api/Mails/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MailList");
            }
            return RedirectToAction("MailList");
        }
    }
}
