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
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            var employees = await _context.Employee.ToListAsync();
            var cafes = await _context.Cafe.ToListAsync();

            var sgtTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            var todaySGT = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sgtTimeZone).Date;

            var result = employees.Select(e => new
            {
                e.Id,
                e.Name,
                e.EmailAddress,
                e.PhoneNumber,
                e.Gender,
                e.CafeId,
                e.EmploymentDate,
                CafeName = !string.IsNullOrEmpty(e.CafeId) ? cafes.FirstOrDefault(c => c.Id == Guid.Parse((e.CafeId)))?.Name : "",
                DaysWorked = e.EmploymentDate.HasValue
                        ? (todaySGT - e.EmploymentDate.Value.Date).Days
                        : 0
            });
            return Ok(result);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

         // PUT: api/Employees/5
      
 [HttpPut("{id}")]
 public async Task<IActionResult> PutEmployee(string id, Employee employee)
 {
     if (id != employee.Id)
     {
         return BadRequest();
     }

     _context.Entry(employee).State = EntityState.Modified;

     try
     {
         await _context.SaveChangesAsync();
         return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, new
         {
             status = "success",
             message = "Employee updated successfully.",
             data = employee
         });
     }
     catch (DbUpdateConcurrencyException ex)
     {
         return StatusCode(StatusCodes.Status500InternalServerError, new
         {
             status = "error",
             message = "An error occurred while updating employee data.",
             details = ex.Message,
             data = employee
         });
     }

    
 }

 // POST: api/Employees     
 [HttpPost]
 public async Task<IActionResult> PostEmployee(Employee employee)
 {          
     
     if (EmployeeExists(null, employee))
     {
         return Ok(new 
         {
             data = employee,
             status = "conflict",
             message = "Employee with the same email already exists." 
         });
     }

     try
     {
         employee.Id = GenerateEmployeeId();
         _context.Employee.Add(employee);
         await _context.SaveChangesAsync();

         return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, new
         {
             status = "success",
             message = "Employee added successfully.",
             data = employee
         });
     }
     catch (DbUpdateException ex)
     {
         return StatusCode(StatusCodes.Status500InternalServerError, new
         {
             status = "error",
             message = "An error occurred while saving employee data.",
             details = ex.Message,
             data = employee
         });
     }
 }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(string? Id, Employee emp)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                return _context.Employee.Any(e => e.Id == Id);
            }
            else
            {
                return _context.Employee.Any(e => e.EmailAddress == emp.EmailAddress);
            }
        }

        public static string GenerateEmployeeId()
        {
            var rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string id;
            do
            {
                id = "UI" + new string(Enumerable.Range(0, 7).Select(_ => chars[rnd.Next(chars.Length)]).ToArray());
            } while (!id.Skip(2).Any(char.IsDigit)); // Ensure at least one digit
            return id;
        }
    }
}
