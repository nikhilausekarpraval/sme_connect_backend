using AutoMapper;
using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Mvc;


namespace DemoDotNetCoreApplication.Controllers
{

    [ApiController]
    [Route("task")]
    public class TaskItemController : ControllerBase
    {
        private readonly ILogger<TaskItemController> _logger;
        private readonly ITaskItemProvider _taskItemProvider;
        private readonly IEmployeeProvider _employeeProvider;
        private readonly IMapper _mapper;

        public TaskItemController(ILogger<TaskItemController> logger, ITaskItemProvider taskItemProvider,IMapper mapper,IEmployeeProvider employeeProvider)
        {
            _logger = logger;
            _taskItemProvider = taskItemProvider;
            _employeeProvider = employeeProvider;
            _mapper = mapper;
        }


        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DemoDotNetCoreApplication.Modals.Task>>> Get()
        {
            var response = await _taskItemProvider.getTaskItems();

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                _logger.LogError("Error retrieving task items: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DemoDotNetCoreApplication.Modals.Task>> GetTaskItem(int id)
        {
            var response = await _taskItemProvider.GetTaskItem(id);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                _logger.LogWarning($"Task item with ID {id} not found.");
                return NotFound(response.Message);
            }
        }


        [HttpPost("register")]
        public async Task<ActionResult> CreateTaskItem(TaskItemsDto taskItemDto)
        {
            if (taskItemDto == null)
            {
                return BadRequest("Task item data is null.");
            }

            if(taskItemDto.EmployeeId == 0)
            {
                taskItemDto.EmployeeId = null;
            }
            var task = _mapper.Map<DemoDotNetCoreApplication.Modals.Task>(taskItemDto);
            task.CreatedOnDt = DateOnly.FromDateTime(DateTime.Today);
            task.CreatedBy = "admin";// this will updated from context

            var response = await _taskItemProvider.CreateTaskItem(task);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return CreatedAtAction(nameof(GetTaskItem), new { id = taskItemDto.Id}, taskItemDto);
            }
            else
            {
                _logger.LogError("Error creating task item: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpPut("update")]
        public async Task<ActionResult> UpdateTaskItem( [FromBody] TaskItemsDto taskItemDto)
        {
            if (taskItemDto != null)
            {
                if(taskItemDto.EmployeeId == null)
                {
                    return  StatusCode(404, "Employee not found");

                }else
                {
                    ApiResponse<Employee> result = await _employeeProvider.GetEmployee(taskItemDto.EmployeeId);
                    if (result.Status != Constants.ApiResponseType.Success)
                    {
                        return StatusCode(500, result.Message);
                    }
                }
            }

            var response = await _taskItemProvider.UpdateTaskItem(_mapper.Map<Modals.Task>(taskItemDto));

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response); // 204 No Content
            }
            else
            {
                _logger.LogError("Error updating task item: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteTaskItem([FromQuery] int taskId)
        {
            var response = await _taskItemProvider.DeleteTaskItem(taskId);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response); // 204 No Content
            }
            else
            {
                _logger.LogError("Error deleting task item: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }
    }

}
