using Dapper;
using SmartMailAnalytics.Application.Interfaces.MailInterfaces;
using SmartMailAnalytics.Domain.Entities;
using SmartMailAnalytics.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Infrastructure.Repositories.MailRepositories
{
    public class MailRepository : IMailRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public MailRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddMailAsync(Mail mail)
        {
            string query = "INSERT INTO Mails (UserId, MailCategoryId, SenderEmail, ReceiverEmail, Subject, Content, IsSpam, CreatedDate) VALUES (@UserId, @MailCategoryId, @SenderEmail, @ReceiverEmail, @Subject, @Content, @IsSpam, @CreatedDate)";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", mail.UserId);
            parameters.Add("@MailCategoryId", mail.MailCategoryId);
            parameters.Add("@SenderEmail", mail.SenderEmail);
            parameters.Add("@ReceiverEmail", mail.ReceiverEmail);
            parameters.Add("@Subject", mail.Subject);
            parameters.Add("@Content", mail.Content);
            parameters.Add("@IsSpam", mail.IsSpam);
            parameters.Add("@CreatedDate", mail.CreatedDate);
            var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteMailAsync(int id)
        {
            string query = "Delete From Mails where MailId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<Mail> GetByIdAsync(int id)
        {
            string query = "Select * From Mails Where MailId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryFirstOrDefaultAsync<Mail>(query, parameters);
                return values;
            }
        }

        public async Task<List<Mail>> GetMailsAsync()
        {
            string query = "Select * From Mails";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryAsync<Mail>(query);
                return values.ToList();
            }
        }

        public async Task UpdateMailAsync(Mail mail)
        {
            string query = "Update Mails SET UserId = @UserId, MailCategoryId = @MailCategoryId, SenderEmail = @SenderEmail, ReceiverEmail = @ReceiverEmail, Subject = @Subject, Content = @Content, IsSpam = @IsSpam, CreatedDate = @CreatedDate WHERE MailId = @MailId";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", mail.UserId);
            parameters.Add("@MailCategoryId", mail.MailCategoryId);
            parameters.Add("@SenderEmail", mail.SenderEmail);
            parameters.Add("@ReceiverEmail", mail.ReceiverEmail);
            parameters.Add("@Subject", mail.Subject);
            parameters.Add("@Content", mail.Content);
            parameters.Add("@IsSpam", mail.IsSpam);
            parameters.Add("@CreatedDate", mail.CreatedDate);
            parameters.Add("@MailId", mail.MailId);
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
