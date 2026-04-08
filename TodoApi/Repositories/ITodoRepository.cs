using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository : IBaseRepository<Todo>
    {
        Task<IEnumerable<Todo>> GetByUserId(int userId);
        Task<bool> Exists(int id, int userId);
    }
}