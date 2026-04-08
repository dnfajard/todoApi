using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmail(string email);
        Task<bool> Exists(string email);
    }
}