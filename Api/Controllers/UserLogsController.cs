#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserLogs
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserLog>>> GetUserLog()
        {
            return await _context.UserLog.ToListAsync();
        }

        // GET: api/UserLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLog>> GetUserLog(long id)
        {
            var userLog = await _context.UserLog.FindAsync(id);

            if (userLog == null)
            {
                return NotFound();
            }

            return userLog;
        }

        // PUT: api/UserLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLog(long id, UserLog userLog)
        {
            if (id != userLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(userLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLogExists(id))
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

        // POST: api/UserLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserLog>> PostUserLog(UserLog userLog)
        {
            _context.UserLog.Add(userLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserLog", new { id = userLog.Id }, userLog);
        }

        // DELETE: api/UserLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLog(long id)
        {
            var userLog = await _context.UserLog.FindAsync(id);
            if (userLog == null)
            {
                return NotFound();
            }

            _context.UserLog.Remove(userLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserLogExists(long id)
        {
            return _context.UserLog.Any(e => e.Id == id);
        }
    }
}
