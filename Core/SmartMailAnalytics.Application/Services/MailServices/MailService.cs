using SmartMailAnalytics.Application.DTOs.MailDtos;
using SmartMailAnalytics.Application.Interfaces.MailInterfaces;
using SmartMailAnalytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Services.MailServices
{
    public class MailService
    {
        private readonly IMailRepository _mailRepository;

        public MailService(IMailRepository mailRepository)
        {
            _mailRepository = mailRepository;
        }

        public async Task<List<ResultMailDto>> GetMailsAsync()
        {
            var values = await _mailRepository.GetMailsAsync();
            return values.Select(x => new ResultMailDto
            {
                MailId = x.MailId,
                SenderEmail = x.SenderEmail,
                ReceiverEmail = x.ReceiverEmail,
                Subject = x.Subject,
                Content = x.Content,
                IsSpam = x.IsSpam,
                CreatedDate = x.CreatedDate,
                MailCategoryId = x.MailCategoryId,
                UserId = x.UserId,
            }).ToList();
        }

        public async Task<GetMailByIdDto> GetByIdAsync(int id)
        {
            var value = await _mailRepository.GetByIdAsync(id);
            return new GetMailByIdDto
            {
                MailId = value.MailId,
                SenderEmail = value.SenderEmail,
                ReceiverEmail = value.ReceiverEmail,
                Subject = value.Subject,
                Content = value.Content,
                IsSpam = value.IsSpam,
                CreatedDate = value.CreatedDate,
                MailCategoryId = value.MailCategoryId,
                UserId = value.UserId
            };
        }

        public async Task CreateMailAsync(CreateMailDto dto)
        {
            var value = new Mail
            {
                UserId = dto.UserId,
                MailCategoryId = dto.MailCategoryId,
                SenderEmail = dto.SenderEmail,
                ReceiverEmail = dto.ReceiverEmail,
                Subject = dto.Subject,
                Content = dto.Content,
                IsSpam = false,
                CreatedDate = DateTime.UtcNow,
            };

            await _mailRepository.AddMailAsync(value);
        }

        public async Task UpdateMailAsync(UpdateMailDto dto)
        {
            var value = await _mailRepository.GetByIdAsync(dto.MailId);

            value.Subject = dto.Subject;
            value.Content = dto.Content;

            await _mailRepository.UpdateMailAsync(value);
        }

        public async Task DeleteMailAsync(int id)
        {
            await _mailRepository.DeleteMailAsync(id);
        }
    }
}
