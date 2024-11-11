using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hoppKutya.Contexts;
using hoppKutya.Models;

namespace hoppKutya.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameTablesController : ControllerBase
    {
        private readonly hoppkutyaDbContext _context;

        public GameTablesController(hoppkutyaDbContext context)
        {
            _context = context;
        }

        // GET: api/GameTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameTables>>> GetGameTables()
        {
			Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0";
			Response.Headers["Pragma"] = "no-cache";
			Response.Headers["Expires"] = "0";

			return await _context.GameTables.ToListAsync();
        }

        // GET: api/GameTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameTables>> GetGameTables(int id)
        {
			Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0";
			Response.Headers["Pragma"] = "no-cache";
			Response.Headers["Expires"] = "0";

			var gameTables = await _context.GameTables.FindAsync(id);

            if (gameTables == null)
            {
                return NotFound();
            }

            return gameTables;
        }

        // PUT: api/GameTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameTables(int id, GameTables gameTables)
        {
			Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0";
			Response.Headers["Pragma"] = "no-cache";
			Response.Headers["Expires"] = "0";

			if (id != gameTables.TableId)
            {
                return BadRequest();
            }

            _context.Entry(gameTables).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameTablesExists(id))
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

        // POST: api/GameTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameTables>> PostGameTables(GameTables gameTables)
        {
			Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0";
			Response.Headers["Pragma"] = "no-cache";
			Response.Headers["Expires"] = "0";

			_context.GameTables.Add(gameTables);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameTables", new { id = gameTables.TableId }, gameTables);
        }

        // DELETE: api/GameTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameTables(int id)
        {
			Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0";
			Response.Headers["Pragma"] = "no-cache";
			Response.Headers["Expires"] = "0";

			var gameTables = await _context.GameTables.FindAsync(id);
            if (gameTables == null)
            {
                return NotFound();
            }

            _context.GameTables.Remove(gameTables);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool GameTablesExists(int id)
        {
            return _context.GameTables.Any(e => e.TableId == id);
        }
    }
}
