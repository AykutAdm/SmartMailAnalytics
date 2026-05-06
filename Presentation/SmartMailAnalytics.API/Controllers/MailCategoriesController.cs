using Microsoft.AspNetCore.Mvc;
using SmartMailAnalytics.Application.DTOs.MailCategoryDtos;
using SmartMailAnalytics.Application.Services.MailCategoryServices;


namespace SmartMailAnalytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailCategoriesController : ControllerBase
    {
        private readonly MailCategoryService _mailCategoryService;

        public MailCategoriesController(MailCategoryService mailCategoryService)
        {
            _mailCategoryService = mailCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMailCategories()
        {
            var values = await _mailCategoryService.GetMailCategoriesAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMailCategoryById(int id)
        {
            var value = await _mailCategoryService.GetByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMailCategory(CreateMailCategoryDto dto)
        {
            await _mailCategoryService.CreateMailCategoryAsync(dto);
            return Ok("MailCategory created successfully");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMailCategory(UpdateMailCategoryDto dto)
        {
            await _mailCategoryService.UpdateMailCategoryAsync(dto);
            return Ok("MailCategory updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMailCategory(int id)
        {
            await _mailCategoryService.DeleteMailCategoryAsync(id);
            return Ok("MailCategory deleted successfully");
        }
    }
}
