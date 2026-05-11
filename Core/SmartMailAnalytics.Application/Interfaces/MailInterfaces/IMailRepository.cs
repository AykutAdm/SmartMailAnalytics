using SmartMailAnalytics.Application.DTOs.MailDtos;
using SmartMailAnalytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Interfaces.MailInterfaces
{
    public interface IMailRepository
    {
        Task<List<ResultMailDto>> GetMailsAsync(int page = 1);
        Task<Mail> GetByIdAsync(int id);
        Task AddMailAsync(Mail mail);
        Task UpdateMailAsync(Mail mail);
        Task DeleteMailAsync(int id);
        Task<List<ResultMailDto>> GetMailsByFilterAsync(ResultMailFilterDto filter);
        Task UpdateSpamStatusAsync(int mailId, bool isSpam);
    }
}
