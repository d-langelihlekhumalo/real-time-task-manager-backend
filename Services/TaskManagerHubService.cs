using Microsoft.AspNetCore.SignalR;
using RealTimeTaskManager.Hubs;
using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public class TaskManagerHubService : ITaskManagerHubService
    {
        private readonly IHubContext<TaskManagerHub> _hubContext;
        private readonly ILogger<TaskManagerHubService> _logger;

        public TaskManagerHubService(IHubContext<TaskManagerHub> hubContext, ILogger<TaskManagerHubService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyTaskCreatedAsync(TaskCreatedMessage message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("TaskCreated", message);
                _logger.LogInformation($"SignalR: Task created notification sent - {message.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending TaskCreated SignalR notification for task {message.Id}");
            }
        }

        public async Task NotifyTaskUpdatedAsync(TaskUpdatedMessage message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("TaskUpdated", message);
                _logger.LogInformation($"SignalR: Task updated notification sent - {message.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending TaskUpdated SignalR notification for task {message.Id}");
            }
        }

        public async Task NotifyTaskDeletedAsync(TaskDeletedMessage message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("TaskDeleted", message);
                _logger.LogInformation($"SignalR: Task deleted notification sent - {message.TaskId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending TaskDeleted SignalR notification for task {message.TaskId}");
            }
        }

        public async Task NotifyTaskCompletionChangedAsync(TaskCompletionChangedMessage message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("TaskCompletionChanged", message);
                _logger.LogInformation($"SignalR: Task completion changed notification sent - {message.Title} (Completed: {message.IsCompleted})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending TaskCompletionChanged SignalR notification for task {message.Id}");
            }
        }

        public async Task NotifyNoteAddedAsync(NoteAddedMessage message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("NoteAdded", message);
                _logger.LogInformation($"SignalR: Note added notification sent - Task {message.TaskId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending NoteAdded SignalR notification for note {message.Id}");
            }
        }

        public async Task NotifyNoteUpdatedAsync(NoteUpdatedMessage message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("NoteUpdated", message);
                _logger.LogInformation($"SignalR: Note updated notification sent - Note {message.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending NoteUpdated SignalR notification for note {message.Id}");
            }
        }

        public async Task NotifyNoteDeletedAsync(NoteDeletedMessage message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("NoteDeleted", message);
                _logger.LogInformation($"SignalR: Note deleted notification sent - Note {message.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending NoteDeleted SignalR notification for note {message.Id}");
            }
        }

        public async Task NotifyActivityUpdateAsync(ActivityResponse activity)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ActivityUpdate", activity);
                _logger.LogInformation($"SignalR: Activity update notification sent - {activity.ActionDisplayName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending ActivityUpdate SignalR notification for activity {activity.Id}");
            }
        }
    }
}