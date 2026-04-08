using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AuthService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<(User user, string token)?> Register(RegisterRequest request)
        {
            // Check if user exists
            if (await _userRepository.Exists(request.Email))
            {
                return null;
            }

            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            Console.WriteLine(request.Name);
            // Create user
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = hashedPassword
            };

            // Save to database
            await _userRepository.Add(user);

            // Generate token
            var token = _tokenService.GenerateToken(user);

            return (user, token);
        }

        public async Task<(User user, string token)?> Login(LoginRequest request)
        {            
            // Find user
            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                return null;
            }
            
            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            
            if (!isPasswordValid)
            {
                return null;
            }

            // Generate token
            var token = _tokenService.GenerateToken(user);
            
            return (user, token);         
        }
    }
}