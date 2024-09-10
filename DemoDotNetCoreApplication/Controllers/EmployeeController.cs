using AutoMapper;
using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Mvc;

namespace DemoDotNetCoreApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeProvider _employeeProvider;
        private readonly IMapper _mapper;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeProvider employeeProvider,IMapper mapper)
        {
            _logger = logger;
            _employeeProvider = employeeProvider;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var response =  await _employeeProvider.GetEmployee(id);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                _logger.LogWarning($"Employee with ID {id} not found.");
                return NotFound(response.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var response = await _employeeProvider.getEmployees();

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                _logger.LogError("Error retrieving employees: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            var response = await _employeeProvider.CreateEmployee(_mapper.Map<Employee>(employeeDto));

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return CreatedAtAction(nameof(GetEmployee), new { id = employeeDto.Id }, employeeDto);
            }
            else
            {
                _logger.LogError("Error creating employee: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpPut]
        public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
           
            var response = await _employeeProvider.UpdateEmployee(_mapper.Map<Employee>(employeeDto));

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                _logger.LogError("Error updating employee: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var response = await _employeeProvider.DeleteEmployee(id);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                _logger.LogError("Error deleting employee: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }
    }

}
