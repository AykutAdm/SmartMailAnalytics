using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartMailAnalytics.WebUI.DTOs.UserDtos;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> UserList(int page = 1)
        {
            ViewBag.Page = page;
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7161/api/Users?page={page}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultUserDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new CreateUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createUserDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7161/api/Users", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }
            return View(createUserDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7161/api/Users/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateUserDto>(jsonData);
                return View(value);
            }
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateUserDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7161/api/Users", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }
            return View(updateUserDto);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7161/api/Users/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }
            return RedirectToAction("UserList");
        }
    }
}
