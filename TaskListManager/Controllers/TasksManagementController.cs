using Microsoft.AspNetCore.Mvc;
using TaskListManager.Business.Interface;
using TaskListManager.Data.ViewModel;

namespace TaskListManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksManagementController : ControllerBase
    {
        private readonly ITaskListManagerService _taskService;
        private readonly ILogger<TasksManagementController> _logger;

        public TasksManagementController(ITaskListManagerService taskService, ILogger<TasksManagementController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var response = await _taskService.GetAllTasks();
            if (!response.Successful)
            {
                _logger.LogWarning("Failed to get tasks: {Message}", response.Message);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var response = await _taskService.GetTaskById(id);
            if (!response.Successful)
            {
                _logger.LogWarning("Failed to get task {TaskId}: {Message}", id, response.Message);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            var response = await _taskService.CreateTask(createTaskDto);
            if (!response.Successful)
            {
                _logger.LogWarning("Failed to create task: {Message}", response.Message);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var response = await _taskService.UpdateTask(id, updateTaskDto);
            if (!response.Successful)
            {
                _logger.LogWarning("Failed to update task {TaskId}: {Message}", id, response.Message);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var response = await _taskService.DeleteTask(id);
            if (!response.Successful)
            {
                _logger.LogWarning("Failed to delete task {TaskId}: {Message}", id, response.Message);
                return BadRequest(response);
            }
            return NoContent();
        }
    }
}