using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var userId = GetUserId();
                var todos = await _todoService.GetUserTodos(userId);
                return Ok(new { todos });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching todos" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            try
            {
                var userId = GetUserId();
                var todo = await _todoService.GetTodoById(id, userId);

                if (todo == null)
                {
                    return NotFound(new { message = "Todo not found" });
                }

                return Ok(new { todo });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the todo" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoRequest request)
        {
            try
            {
                var userId = GetUserId();
                var todo = await _todoService.CreateTodo(userId, request);
                return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, new { todo });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating the todo" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoRequest request)
        {
            try
            {
                var userId = GetUserId();
                var todo = await _todoService.UpdateTodo(id, userId, request);

                if (todo == null)
                {
                    return NotFound(new { message = "Todo not found" });
                }

                return Ok(new { todo });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the todo" });
            }
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleTodo(int id)
        {
            try
            {
                var userId = GetUserId();
                var success = await _todoService.ToggleTodo(id, userId);

                if (!success)
                {
                    return NotFound(new { message = "Todo not found" });
                }

                return Ok(new { message = "Todo toggled successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while toggling the todo" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                var userId = GetUserId();
                var success = await _todoService.DeleteTodo(id, userId);

                if (!success)
                {
                    return NotFound(new { message = "Todo not found" });
                }

                return Ok(new { message = "Todo deleted successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the todo" });
            }
        }
    }
}