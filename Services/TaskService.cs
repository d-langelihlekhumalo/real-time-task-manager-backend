using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealTimeTaskManager.Data;
using RealTimeTaskManager.Entities;
using RealTimeTaskManager.Enums;
using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        private readonly IActivityService _activityService;
        private readonly ITaskManagerHubService _hubService;

        public TaskService(ApplicationDbContext dbContext, IMapper mapper, ILogger<TaskService> logger, IActivityService activityService, ITaskManagerHubService hubService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _activityService = activityService;
            _hubService = hubService;
        }

        public async Task<List<TaskResponse>> GetAllTasksAsync()
        {
            var tasks = await _dbContext.Tasks
                .Include(t => t.Notes)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<TaskResponse>>(tasks);
        }

        public async Task<TaskResponse?> GetTaskByIdAsync(Guid id)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.Notes)
                .FirstOrDefaultAsync(t => t.Id == id);

            return task == null ? null : _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request)
        {
            var taskEntity = _mapper.Map<TaskEntity>(request);

            // Set properties that AutoMapper ignores
            taskEntity.Id = Guid.NewGuid();
            taskEntity.IsCompleted = false;
            taskEntity.CreatedAt = DateTime.UtcNow;
            taskEntity.UpdatedAt = DateTime.UtcNow;

            _dbContext.Tasks.Add(taskEntity);
            await _dbContext.SaveChangesAsync();

            // Record activity
            await _activityService.RecordActivityAsync(
                ActivityActionEnum.TaskCreated,
                EntityTypeEnum.Task,
                taskEntity.Id,
                taskEntity.Title
            );

            var taskResponse = _mapper.Map<TaskResponse>(taskEntity);

            // Send SignalR notification
            var taskCreatedMessage = new TaskCreatedMessage
            {
                Id = taskEntity.Id,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                CreatedAt = taskEntity.CreatedAt
            };
            await _hubService.NotifyTaskCreatedAsync(taskCreatedMessage);

            _logger.LogInformation($"Task created: {taskEntity.Title}");

            return taskResponse;
        }

        public async Task<TaskResponse?> UpdateTaskAsync(Guid id, UpdateTaskRequest request)
        {
            var taskEntity = await _dbContext.Tasks
                .Include(t => t.Notes)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity == null)
                return null;

            var wasCompleted = taskEntity.IsCompleted;
            // Use AutoMapper to update properties from request
            _mapper.Map(request, taskEntity);
            taskEntity.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            // Record activity
            var action = wasCompleted != request.IsCompleted
                ? (request.IsCompleted ? ActivityActionEnum.TaskCompleted : ActivityActionEnum.TaskUncompleted)
                : ActivityActionEnum.TaskUpdated;

            await _activityService.RecordActivityAsync(
                action,
                EntityTypeEnum.Task,
                taskEntity.Id,
                taskEntity.Title
            );

            var taskResponse = _mapper.Map<TaskResponse>(taskEntity);

            // Send SignalR notification - differentiate between completion change and regular update
            if (wasCompleted != request.IsCompleted)
            {
                var taskCompletionMessage = new TaskCompletionChangedMessage
                {
                    Id = taskEntity.Id,
                    Title = taskEntity.Title,
                    IsCompleted = taskEntity.IsCompleted,
                    UpdatedAt = taskEntity.UpdatedAt
                };
                await _hubService.NotifyTaskCompletionChangedAsync(taskCompletionMessage);
            }
            else
            {
                var taskUpdatedMessage = new TaskUpdatedMessage
                {
                    Id = taskEntity.Id,
                    Title = taskEntity.Title,
                    Description = taskEntity.Description,
                    IsCompleted = taskEntity.IsCompleted,
                    UpdatedAt = taskEntity.UpdatedAt
                };
                await _hubService.NotifyTaskUpdatedAsync(taskUpdatedMessage);
            }

            _logger.LogInformation($"Task updated: {taskEntity.Title}");

            return taskResponse;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var taskEntity = await _dbContext.Tasks.FindAsync(id);
            if (taskEntity == null)
                return false;

            // Record activity BEFORE deletion (so we have the title)
            await _activityService.RecordActivityAsync(
                ActivityActionEnum.TaskDeleted,
                EntityTypeEnum.Task,
                taskEntity.Id,
                taskEntity.Title
            );

            _dbContext.Tasks.Remove(taskEntity);
            await _dbContext.SaveChangesAsync();

            // Send SignalR notification
            var taskDeletedMessage = new TaskDeletedMessage
            {
                TaskId = taskEntity.Id
            };
            await _hubService.NotifyTaskDeletedAsync(taskDeletedMessage);

            _logger.LogInformation($"Task deleted: {taskEntity.Title}");

            return true;
        }

        public async Task<bool> ToggleTaskCompletionAsync(Guid id)
        {
            var taskEntity = await _dbContext.Tasks.FindAsync(id);
            if (taskEntity == null)
                return false;

            var newCompletionStatus = !taskEntity.IsCompleted;
            taskEntity.IsCompleted = !taskEntity.IsCompleted;
            taskEntity.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            // Record activity
            var action = newCompletionStatus ? ActivityActionEnum.TaskCompleted : ActivityActionEnum.TaskUncompleted;
            await _activityService.RecordActivityAsync(
                action,
                EntityTypeEnum.Task,
                taskEntity.Id,
                taskEntity.Title
            );

            // Send SignalR notification
            var taskCompletionMessage = new TaskCompletionChangedMessage
            {
                Id = taskEntity.Id,
                Title = taskEntity.Title,
                IsCompleted = taskEntity.IsCompleted,
                UpdatedAt = taskEntity.UpdatedAt
            };
            await _hubService.NotifyTaskCompletionChangedAsync(taskCompletionMessage);

            _logger.LogInformation($"Task completion toggled: {taskEntity.Title} -> {newCompletionStatus}");

            return true;
        }
    }
}
