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
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all employees along with their associated cafe name and number of days worked.
        /// </summary>
        /// <returns>List of employees with details.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEmployee()
        {
            DataRepository.SeedData(_context); // Ensure sample data is available

            var employees = await _context.Employee.ToListAsync();
            var cafes = await _context.Cafe.ToListAsync();

            // Get current Singapore time
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
                CafeName = !string.IsNullOrEmpty(e.CafeId)
                    ? cafes.FirstOrDefault(c => c.Id == Guid.Parse(e.CafeId))?.Name
                    : "",
                DaysWorked = e.EmploymentDate.HasValue
                    ? Math.Max(1, (todaySGT - e.EmploymentDate.Value.Date).Days)
                    : 0
            }).OrderByDescending(e => e.DaysWorked);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific employee by ID.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details if found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
                return NotFound();

            return employee;
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="employee">Employee object to update</param>
        /// <returns>Status of the update operation</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(string id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

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

        /// <summary>
        /// Adds a new employee.
        /// </summary>
        /// <param name="employee">New employee object</param>
        /// <returns>Result of creation operation</returns>
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
                employee.Id = DataRepository.GenerateEmployeeId();
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

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Status of the delete operation</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
                return NotFound();

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks whether an employee exists based on ID or EmailAddress.
        /// </summary>
        /// <param name="Id">Optional ID</param>
        /// <param name="emp">Employee object</param>
        /// <returns>True if exists, false otherwise</returns>
        private bool EmployeeExists(string? Id, Employee emp)
        {
            if (!string.IsNullOrEmpty(Id))
                return _context.Employee.Any(e => e.Id == Id);
            else
                return _context.Employee.Any(e => e.EmailAddress == emp.EmailAddress);
        }
    }
}
