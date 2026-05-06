using SmartMailAnalytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Interfaces.MailCategoryInterfaces
{
    public interface IMailCategoryRepository
    {
        Task<List<MailCategory>> GetMailCategoryAsync();
        Task<MailCategory> GetByIdAsync(int id);
        Task AddMailCategoryAsync(MailCategory mailCategory);
        Task UpdateMailCategoryAsync(MailCategory mailCategory);
        Task DeleteMailCategoryAsync(int id);
    }
}
