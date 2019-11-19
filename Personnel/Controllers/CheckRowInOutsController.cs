using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personnel.Models;

namespace Personnel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckRowInOutsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CheckRowInOutsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/CheckRowInOuts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckRowInOut>>> GetCheckRowInOuts()
        {
            return await _context.CheckRowInOuts.ToListAsync();
        }

        // GET: api/CheckRowInOuts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheckRowInOut>> GetCheckRowInOut(int id)
        {
            var checkRowInOut = await _context.CheckRowInOuts.FindAsync(id);

            if (checkRowInOut == null)
            {
                return NotFound();
            }

            return checkRowInOut;
        }

        // PUT: api/CheckRowInOuts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckRowInOut(int id, CheckRowInOut checkRowInOut)
        {
            if (id != checkRowInOut.ID)
            {
                return BadRequest();
            }

            _context.Entry(checkRowInOut).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckRowInOutExists(id))
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

        // POST: api/CheckRowInOuts
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CheckRowInOut>> PostCheckRowInOut(CheckRowInOut checkRowInOut)
        {
            _context.CheckRowInOuts.Add(checkRowInOut);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCheckRowInOut", new { id = checkRowInOut.ID }, checkRowInOut);
        }

        // DELETE: api/CheckRowInOuts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CheckRowInOut>> DeleteCheckRowInOut(int id)
        {
            var checkRowInOut = await _context.CheckRowInOuts.FindAsync(id);
            if (checkRowInOut == null)
            {
                return NotFound();
            }

            _context.CheckRowInOuts.Remove(checkRowInOut);
            await _context.SaveChangesAsync();

            return checkRowInOut;
        }

        private bool CheckRowInOutExists(int id)
        {
            return _context.CheckRowInOuts.Any(e => e.ID == id);
        }
    }
}
