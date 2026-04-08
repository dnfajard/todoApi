using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetUserTodos(int userId);
        Task<Todo?> GetTodoById(int id, int userId);
        Task<Todo> CreateTodo(int userId, TodoRequest request);
        Task<Todo?> UpdateTodo(int id, int userId, TodoRequest request);
        Task<bool> DeleteTodo(int id, int userId);
        Task<bool> ToggleTodo(int id, int userId);
    }
}