using SmartMailAnalytics.Application.DTOs.MailDtos;
using SmartMailAnalytics.Application.DTOs.RabbitMqDtos;
using SmartMailAnalytics.Application.Interfaces.MailInterfaces;
using SmartMailAnalytics.Application.Services.RabbitMqServices;
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
        private readonly RabbitMqPublisher _publisher;

        public MailService(IMailRepository mailRepository, RabbitMqPublisher publisher)
        {
            _mailRepository = mailRepository;
            _publisher = publisher;
        }

        public async Task<List<ResultMailDto>> GetMailsAsync(int page = 1)
        {
            return await _mailRepository.GetMailsAsync(page);
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

            // RabbitMQ'ya gönder
            _publisher.Publish(new SpamRequestDto
            {
                MailId = value.MailId,
                Subject = value.Subject,
                Content = value.Content
            });
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

        public async Task<List<ResultMailDto>> GetMailsByFilterAsync(ResultMailFilterDto filter)
        {
            return await _mailRepository.GetMailsByFilterAsync(filter);
        }
    }
}
