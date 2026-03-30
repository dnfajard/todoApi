using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : BaseRepository<Todo>,ITodoRepository
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetByUserId(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }


        public async Task<bool> Exists(int id, int userId)
        {
            return await _context.Todos
                .AnyAsync(t => t.Id == id && t.UserId == userId);
        }
    }
    
}