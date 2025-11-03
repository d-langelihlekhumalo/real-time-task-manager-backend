using Microsoft.AspNetCore.SignalR;

namespace RealTimeTaskManager.Hubs
{
    public class TaskManagerHub : Hub
    {
        private readonly ILogger<TaskManagerHub> _logger;

        public TaskManagerHub(ILogger<TaskManagerHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            if (exception != null)
            {
                _logger.LogError(exception, $"Client disconnected with error: {Context.ConnectionId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Client-side methods that will be called from the server
        // These are the method names that clients should listen for:
        
        // Task-related events
        // - TaskCreated(TaskCreatedMessage message)
        // - TaskUpdated(TaskUpdatedMessage message)
        // - TaskDeleted(TaskDeletedMessage message)
        // - TaskCompletionChanged(TaskCompletionChangedMessage message)
        
        // Note-related events
        // - NoteAdded(NoteAddedMessage message)
        // - NoteUpdated(NoteUpdatedMessage message)
        // - NoteDeleted(NoteDeletedMessage message)
        
        // Activity feed events
        // - ActivityUpdate(ActivityResponse activity)
    }
}