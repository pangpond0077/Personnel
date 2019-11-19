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
    public class CheckInOutsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CheckInOutsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/CheckInOuts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckInOut>>> GetCheckInOuts()
        {
            return await _context.CheckInOuts.ToListAsync();
        }

        // GET: api/CheckInOuts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheckInOut>> GetCheckInOut(int id)
        {
            var checkInOut = await _context.CheckInOuts.FindAsync(id);

            if (checkInOut == null)
            {
                return NotFound();
            }

            return checkInOut;
        }

        // PUT: api/CheckInOuts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckInOut(int id, CheckInOut checkInOut)
        {
            if (id != checkInOut.ID)
            {
                return BadRequest();
            }

            _context.Entry(checkInOut).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckInOutExists(id))
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

        // POST: api/CheckInOuts
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CheckInOut>> PostCheckInOut(List<CheckInOut> checkInOut)
        {

            _context.CheckInOuts.AddRange(checkInOut);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCheckInOut", new { id = checkInOut[0].ID }, checkInOut);
        }





     
        // DELETE: api/CheckRowInOuts/5
        [HttpDelete("{site}")]
        public ActionResult<IEnumerable<CheckInOut>> DeleteCheckInOut(string site)
        {
            var checkInOut = _context.CheckInOuts.Where(p=>p.Site==site).ToList();
            if (checkInOut == null)
            {
                return NotFound();
            }

            _context.CheckInOuts.RemoveRange(checkInOut);
            _context.SaveChanges();

            return checkInOut;
        }

        private bool CheckInOutExists(int id)
        {
            return _context.CheckInOuts.Any(e => e.ID == id);
        }
    }
}
