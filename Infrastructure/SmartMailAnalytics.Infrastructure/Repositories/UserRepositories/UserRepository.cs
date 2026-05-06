using Dapper;
using SmartMailAnalytics.Application.Interfaces.UserInterfaces;
using SmartMailAnalytics.Domain.Entities;
using SmartMailAnalytics.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Infrastructure.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddUserAsync(User user)
        {
            string query = "INSERT INTO Users (Email, Name) VALUES (@email, @name)";
            var parameters = new DynamicParameters();
            parameters.Add("@email", user.Email);
            parameters.Add("@name", user.Name);
            var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteUserAsync(int id)
        {
            string query = "Delete From Users Where UserId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            string query = "Select * From Users Where UserId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);
                return values;
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            string query = "Select * From Users";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var values = await connection.QueryAsync<User>(query);
                return values.ToList();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            string query = "Update Users SET Email = @email, Name = @name WHERE UserId = @userId";
            var parameters = new DynamicParameters();
            parameters.Add("@email", user.Email);
            parameters.Add("@name", user.Name);
            parameters.Add("@userId", user.UserId);
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
