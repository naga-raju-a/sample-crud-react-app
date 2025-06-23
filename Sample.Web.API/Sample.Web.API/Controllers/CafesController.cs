using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Web.API.Models;
using Sample.Web.API.Services;

namespace Sample.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CafesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cafes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cafe>>> GetCafe()
        {
            
            var cafes = await _context.Cafe.ToListAsync();
            var employees = await _context.Employee.ToListAsync();
            var result = cafes.Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.Location,
                c.Logo,
                EmployeeCount = employees
                    .Where(e => !string.IsNullOrEmpty(e.CafeId) && Guid.Parse(e.CafeId) == c.Id)
                    .Count()
                    });
            return Ok(result);
           
        }

        // GET: api/Cafes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cafe>> GetCafe(Guid id)
        {
            var cafe = await _context.Cafe.FindAsync(id);

            if (cafe == null)
            {
                return NotFound();
            }

            return cafe;
        }

        // PUT: api/Cafes/5     
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCafe(Guid id, Cafe cafe)
        {
            if (id != cafe.Id)
            {
                return BadRequest();
            }

            _context.Entry(cafe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CafeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCafe", new { id = cafe.Id }, cafe);
        }

        // POST: api/Cafes       
        [HttpPost]
        public async Task<ActionResult<Cafe>> PostCafe(Cafe cafe)
        {
            _context.Cafe.Add(cafe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCafe", new { id = cafe.Id }, cafe);
        }

        // DELETE: api/Cafes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCafe(Guid id)
        {
            var cafe = await _context.Cafe.FindAsync(id);
            if (cafe == null)
            {
                return NotFound();
            }

            _context.Cafe.Remove(cafe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CafeExists(Guid id)
        {
            return _context.Cafe.Any(e => e.Id == id);
        }
    }
}
