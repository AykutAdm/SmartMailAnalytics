using SmartMailAnalytics.Application.DTOs.MailCategoryDtos;
using SmartMailAnalytics.Application.Interfaces.MailCategoryInterfaces;
using SmartMailAnalytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Services.MailCategoryServices
{
    public class MailCategoryService
    {
        private readonly IMailCategoryRepository _mailCategoryRepository;

        public MailCategoryService(IMailCategoryRepository mailCategoryRepository)
        {
            _mailCategoryRepository = mailCategoryRepository;
        }

        public async Task<List<ResultMailCategoryDto>> GetMailCategoriesAsync()
        {
            var values = await _mailCategoryRepository.GetMailCategoryAsync();
            return values.Select(x => new ResultMailCategoryDto
            {
                MailCategoryId = x.MailCategoryId,
                Name = x.Name
            }).ToList();
        }

        public async Task<GetMailCategoryByIdDto> GetByIdAsync(int id)
        {
            var value = await _mailCategoryRepository.GetByIdAsync(id);
            return new GetMailCategoryByIdDto
            {
                MailCategoryId = value.MailCategoryId,
                Name = value.Name
            };
        }

        public async Task CreateMailCategoryAsync(CreateMailCategoryDto dto)
        {
            var value = new MailCategory
            {
                Name = dto.Name
            };

            await _mailCategoryRepository.AddMailCategoryAsync(value);
        }

        public async Task UpdateMailCategoryAsync(UpdateMailCategoryDto dto)
        {
            var value = await _mailCategoryRepository.GetByIdAsync(dto.MailCategoryId);

            value.Name = dto.Name;

            await _mailCategoryRepository.UpdateMailCategoryAsync(value);
        }

        public async Task DeleteMailCategoryAsync(int id)
        {
            await _mailCategoryRepository.DeleteMailCategoryAsync(id);
        }
    }
}
