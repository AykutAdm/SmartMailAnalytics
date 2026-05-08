using Microsoft.AspNetCore.Mvc;
using SmartMailAnalytics.Application.DTOs.MailDtos;
using SmartMailAnalytics.Application.Services.MailServices;

namespace SmartMailAnalytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly MailService _mailService;

        public MailsController(MailService mailService)
        {
            _mailService = mailService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMails(int page = 1)
        {
            var values = await _mailService.GetMailsAsync(page);
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMailById(int id)
        {
            var value = await _mailService.GetByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMail(CreateMailDto dto)
        {
            await _mailService.CreateMailAsync(dto);
            return Ok("Mail created successfully");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMail(UpdateMailDto dto)
        {
            await _mailService.UpdateMailAsync(dto);
            return Ok("Mail updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMail(int id)
        {
            await _mailService.DeleteMailAsync(id);
            return Ok("Mail deleted successfully");
        }

        [HttpGet("GetMailsByFilter")]
        public async Task<IActionResult> GetMailsByFilterAsync([FromQuery] ResultMailFilterDto filter)
        {
            var values = await _mailService.GetMailsByFilterAsync(filter);
            return Ok(values);
        }
    }
}
