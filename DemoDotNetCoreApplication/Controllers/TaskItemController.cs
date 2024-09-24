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
        private readonly IMapper _mapper;

        public TaskItemController(ILogger<TaskItemController> logger, ITaskItemProvider taskItemProvider,IMapper mapper)
        {
            _logger = logger;
            _taskItemProvider = taskItemProvider;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> Get()
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
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
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


        [HttpPost]
        public async Task<ActionResult> CreateTaskItem([FromBody] TaskItemsDto taskItemDto)
        {
            if (taskItemDto == null)
            {
                return BadRequest("Task item data is null.");
            }

            var response = await _taskItemProvider.CreateTaskItem(_mapper.Map<TaskItem>(taskItemDto));

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return CreatedAtAction(nameof(GetTaskItem), new { id = taskItemDto.Id }, taskItemDto);
            }
            else
            {
                _logger.LogError("Error creating task item: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpPut]
        public async Task<ActionResult> UpdateTaskItem( [FromBody] TaskItemsDto taskItemDto)
        {

            var response = await _taskItemProvider.UpdateTaskItem(_mapper.Map<TaskItem>(taskItemDto));

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                _logger.LogError("Error updating task item: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskItem(int id)
        {
            var response = await _taskItemProvider.DeleteTaskItem(id);

            if (response.Status == Constants.ApiResponseType.Success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                _logger.LogError("Error deleting task item: " + response.Message);
                return StatusCode(500, response.Message); // 500 Internal Server Error
            }
        }
    }

}
