using Microsoft.AspNetCore.Mvc;
using EmployeeManagment.Models;
using EmployeeManagment.Repositories;

namespace EmployeeManagment.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            return Ok(await _employeeRepository.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            await _employeeRepository.AddEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployeeById(int id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployeeAsync(int id, Employee employee)
        {
            if(id != employee.Id)
            {
                return BadRequest();
            }
            await _employeeRepository.UpdateEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }
    }
}
