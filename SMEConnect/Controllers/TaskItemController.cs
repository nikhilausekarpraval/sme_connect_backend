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
    [Route("task")]
    [Authorize(Roles = "User")]
    public class TaskItemController : ControllerBase
    {
        private readonly ILogger<TaskItemController> _logger;
        private readonly ITaskItemProvider _taskItemProvider;
        private readonly IEmployeeProvider _employeeProvider;
        private readonly IMapper _mapper;

        public TaskItemController(ILogger<TaskItemController> logger, ITaskItemProvider taskItemProvider, IMapper mapper, IEmployeeProvider employeeProvider)
        {
            _logger = logger;
            _taskItemProvider = taskItemProvider;
            _employeeProvider = employeeProvider;
            _mapper = mapper;
        }


        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<SMEConnect.Modals.Task>>> Get()
        {
            var response = await _taskItemProvider.getTaskItems();

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                _logger.LogError(ApiErrors.ErrorRetrivingTasks + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SMEConnect.Modals.Task>> GetTaskItem(int id)
        {
            var response = await _taskItemProvider.GetTaskItem(id);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                _logger.LogWarning(Helper.GetErrorEntityWithIdNotFound(ModuleName.TaskItem, id));
                return NotFound(response.Message);
            }
        }


        [HttpPost("register")]
        public async Task<ActionResult> CreateTaskItem(TaskItemsDto taskItemDto)
        {
            if (taskItemDto == null)
            {
                return BadRequest(ApiErrors.NullTask);
            }

            if (taskItemDto.EmployeeId == 0)
            {
                taskItemDto.EmployeeId = null;
            }
            var task = _mapper.Map<SMEConnect.Modals.Task>(taskItemDto);
            task.CreatedOnDt = DateOnly.FromDateTime(DateTime.Today);
            task.CreatedBy = RoleName.Admin;// this will updated from context

            var response = await _taskItemProvider.CreateTaskItem(task);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return CreatedAtAction(nameof(GetTaskItem), new { id = taskItemDto.Id }, taskItemDto);
            }
            else
            {
                _logger.LogError(ApiErrors.ErrorCreatingTask + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpPut("update")]
        public async Task<ActionResult> UpdateTaskItem([FromBody] TaskItemsDto taskItemDto)
        {
            if (taskItemDto != null)
            {
                if (taskItemDto.EmployeeId == null)
                {
                    return StatusCode(404, ApiErrors.TaskNotFound);

                }
                else
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
                _logger.LogError(ApiErrors.ErrorUpdatingTask + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpDelete("delete")]
        [Authorize(Policy = "CanDelete")]
        public async Task<ActionResult> DeleteTaskItem([FromQuery] int taskId)
        {
            var response = await _taskItemProvider.DeleteTaskItem(taskId);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return Ok(response); // 204 No Content
            }
            else
            {
                _logger.LogError(ApiErrors.ErrorDeletingTask + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }
    }

}
