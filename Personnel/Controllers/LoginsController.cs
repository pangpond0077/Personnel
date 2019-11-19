using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personnel.Helpers;
using Personnel.Models;

namespace Personnel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LoginsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Logins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetLogins()
        {
            return await _context.Logins.ToListAsync();
        }

        // GET: api/Logins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Login>> GetLogin(int id)
        {
            var login = await _context.Logins.FindAsync(id);

            if (login == null)
            {
                return NotFound();
            }

            return login;
        }

        // PUT: api/Logins/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogin(int id, Login login)
        {
            if (id != login.ID)
            {
                return BadRequest();
            }

            var password = EncryptionHelper.Encrypt(login.Password);
            login.Password = password;
            _context.Entry(login).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(id))
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

        // POST: api/Logins
        [HttpPost]
        public async Task<ActionResult<Login>> PostLogin(Login login)
        {
            var password = EncryptionHelper.Encrypt(login.Password);
            login.Password = password;
            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogin", new { id = login.ID }, login);
        }

        // DELETE: api/Logins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Login>> DeleteLogin(int id)
        {
            var login = await _context.Logins.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }

            _context.Logins.Remove(login);
            await _context.SaveChangesAsync();

            return login;
        }

        private bool LoginExists(int id)
        {
            return _context.Logins.Any(e => e.ID == id);
        }
    }
}
