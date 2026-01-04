using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Todo?> GetById(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<IEnumerable<Todo>> GetByUserId(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Todo> Add(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> Update(Todo todo)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task Delete(int id)
        {
            var todo = await GetById(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id, int userId)
        {
            return await _context.Todos
                .AnyAsync(t => t.Id == id && t.UserId == userId);
        }
    }
}