using AutoMapper;
using MathslideLearning.Business.Interfaces;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using MathslideLearning.Models.AccountsDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> AdminUpdateUserAsync(int userId, AdminUpdateUserRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Role = request.Role;
            user.Grade = request.Grade;

            await _userRepository.UpdateUserAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Grade = user.Grade
            };
        }

        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            return await _userRepository.SoftDeleteUserAsync(userId);
        }
        public async Task<UserResponseDto> UpdateUserAsync(int userId, UpdateUserRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");
            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null && existingUser.Id != userId) throw new Exception("This email is already in use by another account.");
                user.Email = request.Email;
            }
            if (!string.IsNullOrEmpty(request.Name)) user.Name = request.Name;
            if (!string.IsNullOrEmpty(request.Password)) user.Password = request.Password;
            await _userRepository.UpdateUserAsync(user);
            return new UserResponseDto { Id = user.Id, Name = user.Name, Email = user.Email, Role = user.Role, Grade = user.Grade };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || user.Password != request.Password) throw new Exception("Invalid email or password.");
            
            var token = GenerateJwtToken(user);
            var userResponse = _mapper.Map<UserResponseDto>(user);
            
            return new LoginResponseDto 
            { 
                User = userResponse,
                Token = token 
            };
        }

        public async Task<User> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                Role = "Student",
                Grade = request.Grade
            };
            return await _userRepository.CreateUserAsync(newUser);
        }

        public async Task<UserResponseDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Grade = user.Grade
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Grade = user.Grade,
                IsDeleted = user.IsDeleted
            });
        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Name, user.Name ?? ""),
        new Claim(ClaimTypes.Role, user.Role ?? "Teacher"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],       
                audience: _configuration["Jwt:Audience"],   
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


