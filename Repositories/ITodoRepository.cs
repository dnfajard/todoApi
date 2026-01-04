using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<Todo?> GetById(int id);
        Task<IEnumerable<Todo>> GetByUserId(int userId);
        Task<Todo> Add(Todo todo);
        Task<Todo> Update(Todo todo);
        Task Delete(int id);
        Task<bool> Exists(int id, int userId);
    }
}