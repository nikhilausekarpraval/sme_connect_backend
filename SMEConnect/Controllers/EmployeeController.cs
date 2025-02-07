using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Dtos;
using SMEConnect.Helpers;
using SMEConnect.Modals;
using static SMEConnect.Constatns.Constants;

namespace SMEConnect.Controllers
{

    [ApiController]
    //[Authorize(Roles = "User")]
    [Route("employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeProvider _employeeProvider;
        private readonly IMapper _mapper;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeProvider employeeProvider, IMapper mapper)
        {
            _logger = logger;
            _employeeProvider = employeeProvider;
            _mapper = mapper;
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var response = await _employeeProvider.GetEmployee(id);
            var employeeDtos = _mapper.Map<EmployeeTasksDto>(response.Data);
            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(employeeDtos);
            }
            else
            {
                _logger.LogWarning(Helper.GetErrorEntityWithIdNotFound(ModuleName.Employee, id));
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
                    _logger.LogError(ApiErrors.ErrorRetrivingEmployees + response.Message);
                    return StatusCode(500, response.Message); // 500 Internal Server Error
                }
            }
            catch (Exception ex) { }

            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<ActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest(ApiErrors.NullEmployee);
            }
            var emp = _mapper.Map<Employee>(employeeDto);
            emp.CreatedOnDt = DateOnly.FromDateTime(DateTime.Today);
            emp.CreatedBy = RoleName.Admin;// this will updated from context
            var response = await _employeeProvider.CreateEmployee(emp);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return CreatedAtAction(nameof(GetEmployee), new { id = employeeDto.Id }, employeeDto);
            }
            else
            {
                _logger.LogError(ApiErrors.ErrorCreatingEmployee + response.Message);
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
                _logger.LogError(ApiErrors.ErrorUpdatingEmployee + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }

        [HttpDelete("delete")]
        [Authorize(Policy = "CanDelete")]
        public async Task<ActionResult> DeleteEmployee([FromQuery] int employeeId)
        {
            var response = await _employeeProvider.DeleteEmployee(employeeId);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response); // 200 success
            }
            else
            {
                _logger.LogError(ApiErrors.ErrorDeletingEmployee + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }
    }

}
