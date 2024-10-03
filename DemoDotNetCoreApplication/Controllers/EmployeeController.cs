using AutoMapper;
using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoDotNetCoreApplication.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin,User")]
    [Route("employee")]
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
            var employeeDtos = _mapper.Map<EmployeeTasksDto>(response.Data);
            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(employeeDtos);
            }
            else
            {
                _logger.LogWarning($"Employee with ID {id} not found.");
                return NotFound(response.Message);
            }
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EmployeeTasksDto>>> GetEmployees()
        {
            try
            {
                var response = await _employeeProvider.getEmployees();
                var employeeDtos = _mapper.Map<List<EmployeeTasksDto>>(response.Data);// exeception here
                if (response.Status == Constants.ApiResponseType.Success)
                {
                    return Ok(employeeDtos);
                }
                else
                {
                    _logger.LogError("Error retrieving employees: " + response.Message);
                    return StatusCode(500, response.Message); // 500 Internal Server Error
                }
            }
            catch (Exception ex) {  }

            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<ActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }
            var emp = _mapper.Map<Employee>(employeeDto);
            emp.CreatedOnDt = DateOnly.FromDateTime(DateTime.Today);
            emp.CreatedBy = "admin";// this will updated from context
            var response = await _employeeProvider.CreateEmployee(emp);

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


        [HttpPut("update")]
        public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
           
            var response = await _employeeProvider.UpdateEmployee(_mapper.Map<Employee>(employeeDto));

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response); // 200 success
            }
            else
            {
                _logger.LogError("Error updating employee: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteEmployee([FromQuery] int employeeId)
        {
            var response = await _employeeProvider.DeleteEmployee(employeeId);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response); // 200 success
            }
            else
            {
                _logger.LogError("Error deleting employee: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }
    }

}
