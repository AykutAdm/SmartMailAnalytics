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
            string query = @"INSERT INTO Mails (UserId, MailCategoryId, SenderEmail, ReceiverEmail, Subject, Content, IsSpam, CreatedDate) 
                     VALUES (@UserId, @MailCategoryId, @SenderEmail, @ReceiverEmail, @Subject, @Content, @IsSpam, @CreatedDate);
                     SELECT CAST(SCOPE_IDENTITY() AS INT)";
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
            mail.MailId = await connection.ExecuteScalarAsync<int>(query, parameters);
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

        public async Task<List<ResultMailDto>> GetMailsAsync(int page = 1)
        {
            var offset = (page - 1) * 12;
            string query = @"SELECT m.MailId, m.UserId, m.MailCategoryId, m.SenderEmail, m.ReceiverEmail, m.Subject, m.Content, m.IsSpam, m.CreatedDate,
                     c.Name AS MailCategoryName
                     FROM Mails m
                     LEFT JOIN MailCategories c ON c.MailCategoryId = m.MailCategoryId
                     ORDER BY m.CreatedDate DESC OFFSET @Offset ROWS FETCH NEXT 12 ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Offset", offset);
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultMailDto>(query, parameters);
                return values.ToList();
            }
        }

        public async Task<List<ResultMailDto>> GetMailsByFilterAsync(ResultMailFilterDto filter)
        {
            var offset = (filter.Page - 1) * 12;
            var parameters = new DynamicParameters();
            parameters.Add("@Offset", offset);
            parameters.Add("@SenderEmail", filter.SenderEmail);
            parameters.Add("@Subject", filter.Subject);
            parameters.Add("@IsSpam", filter.IsSpam);

            string query = @"SELECT m.MailId, m.UserId, m.MailCategoryId, m.SenderEmail, m.ReceiverEmail, m.Subject, m.Content, m.IsSpam, m.CreatedDate,
                     c.Name AS MailCategoryName
                     FROM Mails m
                     LEFT JOIN MailCategories c ON c.MailCategoryId = m.MailCategoryId
                     WHERE (@SenderEmail IS NULL OR m.SenderEmail LIKE @SenderEmail)
                     AND (@Subject IS NULL OR m.Subject LIKE @Subject)
                     AND (@IsSpam IS NULL OR m.IsSpam = @IsSpam)
                     ORDER BY m.CreatedDate DESC
                     OFFSET @Offset ROWS FETCH NEXT 12 ROWS ONLY";

            using var connection = _connectionFactory.CreateConnection();
            var values = await connection.QueryAsync<ResultMailDto>(query, parameters);
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

        public async Task UpdateSpamStatusAsync(int mailId, bool isSpam)
        {
            string query = "UPDATE Mails SET IsSpam = @IsSpam WHERE MailId = @MailId";
            var parameters = new DynamicParameters();
            parameters.Add("@IsSpam", isSpam);
            parameters.Add("@MailId", mailId);
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
