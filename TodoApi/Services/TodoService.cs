using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<Todo>> GetUserTodos(int userId)
        {
            return await _todoRepository.GetByUserId(userId);
        }

        public async Task<Todo?> GetTodoById(int id, int userId)
        {
            var todo = await _todoRepository.GetById(id);
            
            // Ensure todo belongs to user (security check)
            if (todo == null || todo.UserId != userId)
            {
                return null;
            }

            return todo;
        }

        public async Task<Todo> CreateTodo(int userId, TodoRequest request)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                throw new ArgumentException("Todo text cannot be empty");
            }

            // Business logic: trim whitespace, limit length
            var trimmedText = request.Text.Trim();
            if (trimmedText.Length > 500)
            {
                throw new ArgumentException("Todo text cannot exceed 500 characters");
            }

            var todo = new Todo
            {
                UserId = userId,
                Text = trimmedText,
                Completed = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _todoRepository.Add(todo);
        }

        public async Task<Todo?> UpdateTodo(int id, int userId, TodoRequest request)
        {
            var todo = await GetTodoById(id, userId);
            
            if (todo == null)
            {
                return null;
            }

            // Update text if provided
            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                var trimmedText = request.Text.Trim();
                if (trimmedText.Length > 500)
                {
                    throw new ArgumentException("Todo text cannot exceed 500 characters");
                }
                todo.Text = trimmedText;
            }

            // Update completed status if provided
            if (request.Completed.HasValue)
            {
                todo.Completed = request.Completed.Value;
            }

            todo.UpdatedAt = DateTime.UtcNow;

            return await _todoRepository.Update(todo);
        }

        public async Task<bool> DeleteTodo(int id, int userId)
        {
            // Check if todo exists and belongs to user
            if (!await _todoRepository.Exists(id, userId))
            {
                return false;
            }

            await _todoRepository.Delete(id);
            return true;
        }

        public async Task<bool> ToggleTodo(int id, int userId)
        {
            var todo = await GetTodoById(id, userId);
            
            if (todo == null)
            {
                return false;
            }

            todo.Completed = !todo.Completed;
            todo.UpdatedAt = DateTime.UtcNow;

            await _todoRepository.Update(todo);
            return true;
        }
    }
}