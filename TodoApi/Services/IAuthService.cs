using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface IAuthService
    {
        Task<(User user, string token)?> Register(RegisterRequest request);
        Task<(User user, string token)?> Login(LoginRequest request);
    }
}