using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartMailAnalytics.WebUI.DTOs.MailCategoryDtos;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.WebUI.Controllers
{
    public class MailCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MailCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> MailCategoryList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7161/api/MailCategories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMailCategoryDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateMailCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMailCategory(CreateMailCategoryDto createMailCategoryDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createMailCategoryDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7161/api/MailCategories", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MailCategoryList");
            }
            return View(createMailCategoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMailCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7161/api/MailCategories/{id}");
            return RedirectToAction("MailCategoryList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMailCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7161/api/MailCategories/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateMailCategoryDto>(jsonData);
                return View(value);
            }
            return RedirectToAction("MailCategoryList");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMailCategory(UpdateMailCategoryDto updateMailCategoryDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateMailCategoryDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7161/api/MailCategories", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MailCategoryList");
            }
            return View(updateMailCategoryDto);
        }
    }
}
