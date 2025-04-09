using Microsoft.Extensions.Options;
using NotesApp.Api.Auth;
using NotesApp.Api.Dtos;
using NotesApp.Api.Models;
using NotesApp.Api.Repositories;

namespace NotesApp.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepo, IOptions<JwtSettings> jwtOptions)
        {
            _userRepo = userRepo;
            _jwtSettings = jwtOptions.Value;
        }
        public async Task<AuthResult> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.Username);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new AuthResult { Success = false, Error = "Invalid credentials" };
            }

            var token = JwtUtils.GenerateToken(user, _jwtSettings);

            return new AuthResult { Success = true, Token = token , Username = dto.Username};
        }
        public async Task<AuthResult> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepo.GetByUsernameAsync(dto.Username);
            if (existing != null)
            {
                return new AuthResult { Success = false, Error = "Username already exists" };
            }

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            user.Id = await _userRepo.CreateAsync(user);

            var token = JwtUtils.GenerateToken(user, _jwtSettings);

            return new AuthResult { Success = true, Token = token , Username = dto.Username};
        }
    }
}