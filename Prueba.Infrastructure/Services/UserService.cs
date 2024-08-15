using AutoMapper;
using Prueba.Core.Dtos;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;

namespace Prueba.Infrastructure.Services;

public class UserService : IUserService
{
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(UserDto newUser)
        {
            var user = _mapper.Map<User>(newUser);
            user.PasswordHash = _passwordHasher.HashPassword(newUser.Password);

            var userId = await _userRepository.AddAsync(user);
            newUser.Id = userId;

            return newUser;
        }

        public async Task UpdateUserAsync(UserDto updatedUser)
        {
            var user = await _userRepository.GetByIdAsync(updatedUser.Id);
            if (user != null)
            {
                _mapper.Map(updatedUser, user);

                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    user.PasswordHash = _passwordHasher.HashPassword(updatedUser.Password);
                }

                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
}