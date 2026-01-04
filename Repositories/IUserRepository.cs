using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetById(int id);
        Task<User?> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task<User> Add(User user);
        Task<User> Update(User user);
        Task Delete(int id);
        Task<bool> Exists(string email);
    }
}