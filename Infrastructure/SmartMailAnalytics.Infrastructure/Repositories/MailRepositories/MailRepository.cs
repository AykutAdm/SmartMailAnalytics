using Dapper;
using SmartMailAnalytics.Application.DTOs.MailDtos;
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

        public async Task<List<Mail>> GetMailsAsync(int page = 1)
        {
            var offset = (page - 1) * 12;
            string query = "SELECT * FROM Mails ORDER BY CreatedDate DESC OFFSET @Offset ROWS FETCH NEXT 12 ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Offset", offset);
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryAsync<Mail>(query, parameters);
                return values.ToList();
            }
        }

        public async Task<List<Mail>> GetMailsByFilterAsync(ResultMailFilterDto filter)
        {
            var offset = (filter.Page - 1) * 12;
            var parameters = new DynamicParameters();
            parameters.Add("@Offset", offset);
            parameters.Add("@SenderEmail", filter.SenderEmail);
            parameters.Add("@Subject", filter.Subject);
            parameters.Add("@IsSpam", filter.IsSpam);

            string query = @"SELECT * FROM Mails 
                     WHERE (@SenderEmail IS NULL OR SenderEmail LIKE @SenderEmail)
                     AND (@Subject IS NULL OR Subject LIKE @Subject)
                     AND (@IsSpam IS NULL OR IsSpam = @IsSpam)
                     ORDER BY CreatedDate DESC
                     OFFSET @Offset ROWS FETCH NEXT 12 ROWS ONLY";

            using var connection = _connectionFactory.CreateConnection();
            var values = await connection.QueryAsync<Mail>(query, parameters);
            return values.ToList();
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
