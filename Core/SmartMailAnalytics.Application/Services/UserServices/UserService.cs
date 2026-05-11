using SmartMailAnalytics.Application.DTOs.UserDtos;
using SmartMailAnalytics.Application.Interfaces.MailInterfaces;
using SmartMailAnalytics.Application.Interfaces.UserInterfaces;
using SmartMailAnalytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Services.UserServices
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<List<ResultUserDto>> GetUsersAsync(int page = 1)
        {
            var values = await _userRepository.GetUsersAsync(page);
            return values.Select(x => new ResultUserDto
            {
                UserId = x.UserId,
                Email = x.Email,
                Name = x.Name
            }).ToList();
        }

        public async Task<GetUserByIdDto> GetByIdAsync(int id)
        {
            var value = await _userRepository.GetByIdAsync(id);
            return new GetUserByIdDto
            {
                UserId = value.UserId,
                Email = value.Email,
                Name = value.Name
            };
        }

        public async Task CreateUserAsync(CreateUserDto dto)
        {
            var value = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };

            await _userRepository.AddUserAsync(value);
        }

        public async Task UpdateUserAsync(UpdateUserDto dto)
        {
            var value = await _userRepository.GetByIdAsync(dto.UserId);

            value.Email = dto.Email;
            value.Name = dto.Name;

            await _userRepository.UpdateUserAsync(value);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<int> GetUserCountAsync()
        {
            return await _userRepository.GetUserCountAsync();
        }
    }
}
