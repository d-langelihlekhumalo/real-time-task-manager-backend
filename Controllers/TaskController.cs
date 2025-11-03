using Microsoft.AspNetCore.Mvc;
using RealTimeTaskManager.Models;
using RealTimeTaskManager.Services;

namespace RealTimeTaskManager.Controllers
{
    /// <summary>
    /// Controller for managing tasks in the Real-Time Task Manager application
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        /// <summary>
        /// Retrieves all tasks from the system
        /// </summary>
        /// <returns>A list of all tasks</returns>
        /// <response code="200">Returns the list of tasks</response>
        /// <response code="400">If there was an error retrieving the tasks</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TaskResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<TaskResponse>>> GetTasks()
        {
            _logger.LogInformation("Getting all tasks");

            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tasks");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a specific task by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the task</param>
        /// <returns>The task with the specified ID</returns>
        /// <response code="200">Returns the requested task</response>
        /// <response code="404">If the task is not found</response>
        /// <response code="400">If there was an error retrieving the task</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TaskResponse>> GetTaskById(Guid id)
        {
            _logger.LogInformation($"Get task by id: {id}");

            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tasks");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new task in the system
        /// </summary>
        /// <param name="taskRequest">The task creation request containing task details</param>
        /// <returns>The newly created task</returns>
        /// <response code="201">Returns the newly created task</response>
        /// <response code="400">If the request is invalid or null</response>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest taskRequest)
        {
            _logger.LogInformation("Creating a new task");

            if (taskRequest == null)
                return BadRequest("Task request cannot be null");

            var result = await _taskService.CreateTaskAsync(taskRequest);
            return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing task with new information
        /// </summary>
        /// <param name="id">The unique identifier of the task to update</param>
        /// <param name="taskRequest">The task update request containing new task details</param>
        /// <returns>The updated task</returns>
        /// <response code="201">Returns the updated task</response>
        /// <response code="400">If the request is invalid or the task ID is empty</response>
        /// <response code="404">If the task is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskResponse>> UpdateTaskByID(Guid id, [FromBody] UpdateTaskRequest taskRequest)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid task ID");

            if (taskRequest == null)
                return BadRequest("Task request cannot be null");

            var result = await _taskService.UpdateTaskAsync(id, taskRequest);
            if (result == null)
                return NotFound();

            return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Deletes a task from the system
        /// </summary>
        /// <param name="id">The unique identifier of the task to delete</param>
        /// <returns>Success status of the deletion operation</returns>
        /// <response code="200">If the task was successfully deleted</response>
        /// <response code="400">If the task ID is invalid</response>
        /// <response code="404">If the task is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> DeleteTask(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid task ID");

            var isDeleted = await _taskService.DeleteTaskAsync(id);

            if (!isDeleted)
                return NotFound();

            return Ok();
        }

        /// <summary>
        /// Toggles the completion status of a task (completed/incomplete)
        /// </summary>
        /// <param name="id">The unique identifier of the task to toggle</param>
        /// <returns>Success status of the toggle operation</returns>
        /// <response code="200">If the task completion status was successfully toggled</response>
        /// <response code="400">If the task ID is invalid</response>
        /// <response code="404">If the task is not found</response>
        [HttpPatch("{id}/toggle-completion")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> ToggleTaskCompletion(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid task ID");

            var isToggled = await _taskService.ToggleTaskCompletionAsync(id);

            if (!isToggled)
                return NotFound();

            return Ok(true);
        }
    }
}
