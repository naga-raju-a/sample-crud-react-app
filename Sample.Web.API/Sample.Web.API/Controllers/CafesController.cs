using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Web.API.Data;
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

        /// <summary>
        /// Gets all cafes along with their employee count.
        /// Seeds sample data if not already present.
        /// </summary>
        /// <returns>List of cafes with details.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cafe>>> GetCafe()
        {
            DataRepository.SeedData(_context); // Ensures data is loaded if not exists

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
            }).OrderByDescending(c =>c.EmployeeCount);
            return Ok(result);

        }

        /// <summary>
        /// Gets a specific cafe by its unique Guid.
        /// </summary>
        /// <param name="id">Cafe ID</param>
        /// <returns>Cafe details if found</returns>
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

        /// <summary>
        /// Updates a cafe record by Id.
        /// </summary>
        /// <param name="id">Cafe ID</param>
        /// <param name="cafe">Cafe object to update</param>
        /// <returns>Status of the update operation</returns>
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
                return CreatedAtAction(nameof(GetCafe), new { id = cafe.Id }, new
                {
                    status = "success",
                    message = "Cafe updated successfully.",
                    data = cafe
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "An error occurred while updating cafe data.",
                    details = ex.Message,
                    data = cafe
                });
            }
        }

        /// <summary>
        /// Adds a new cafe to the database.
        /// </summary>
        /// <param name="cafe">New cafe object</param>
        /// <returns>Result of creation operation</returns>
        [HttpPost]
        public async Task<ActionResult<Cafe>> PostCafe(Cafe cafe)
        {
            try
            {
                _context.Cafe.Add(cafe);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCafe), new { id = cafe.Id }, new
                {
                    status = "success",
                    message = "Cafe updated successfully.",
                    data = cafe
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "An error occurred while saving cafe data.",
                    details = ex.Message,
                    data = cafe
                });
            }
        }

        /// <summary>
        /// Deletes an cafe by ID.
        /// </summary>
        /// <param name="id">cafe ID</param>
        /// <returns>Status of the delete operation</returns>
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
