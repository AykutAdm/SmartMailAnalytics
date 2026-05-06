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
        Task<List<Mail>> GetMailsAsync();
        Task<Mail> GetByIdAsync(int id);
        Task AddMailAsync(Mail mail);
        Task UpdateMailAsync(Mail mail);
        Task DeleteMailAsync(int id);
    }
}
