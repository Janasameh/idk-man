using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hamada.Data;
using hamada.models;

namespace hamada
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTask>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> GetUserTask(int id)
        {
            var userTask = await _context.Tasks.FindAsync(id);

            if (userTask == null)
            {
                return NotFound();
            }

            return userTask;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTask(int id, UserTask userTask)
        {
            if (id != userTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(userTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTask>> PostUserTask(UserTask userTask)
        {
            _context.Tasks.Add(userTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserTask", new { id = userTask.Id }, userTask);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTask(int id)
        {
            var userTask = await _context.Tasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(userTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
