using Dapper;
using SmartMailAnalytics.Application.Interfaces.MailCategoryInterfaces;
using SmartMailAnalytics.Domain.Entities;
using SmartMailAnalytics.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Infrastructure.Repositories.MailCategoryRepositories
{
    public class MailCategoryRepository : IMailCategoryRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public MailCategoryRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddMailCategoryAsync(MailCategory mailCategory)
        {
            string query = "INSERT INTO MailCategories (Name) VALUES (@name)";
            var parameters = new DynamicParameters();
            parameters.Add("@name", mailCategory.Name);
            var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteMailCategoryAsync(int id)
        {
            string query = "Delete From MailCategories Where MailCategoryId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<MailCategory> GetByIdAsync(int id)
        {
            string query = "Select * From MailCategories Where MailCategoryId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryFirstOrDefaultAsync<MailCategory>(query, parameters);
                return values;
            }
        }

        public async Task<List<MailCategory>> GetMailCategoryAsync()
        {
            string query = "Select * From MailCategories";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryAsync<MailCategory>(query);
                return values.ToList();
            }
        }

        public async Task UpdateMailCategoryAsync(MailCategory mailCategory)
        {
            string query = "Update MailCategories SET Name = @name WHERE MailCategoryId = @mailCategoryId";
            var parameters = new DynamicParameters();
            parameters.Add("@name", mailCategory.Name);
            parameters.Add("@mailCategoryId", mailCategory.MailCategoryId);
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
