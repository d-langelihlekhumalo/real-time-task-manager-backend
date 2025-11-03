using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public interface ITaskManagerHubService
    {
        // Task-related SignalR notifications
        Task NotifyTaskCreatedAsync(TaskCreatedMessage message);
        Task NotifyTaskUpdatedAsync(TaskUpdatedMessage message);
        Task NotifyTaskDeletedAsync(TaskDeletedMessage message);
        Task NotifyTaskCompletionChangedAsync(TaskCompletionChangedMessage message);

        // Note-related SignalR notifications
        Task NotifyNoteAddedAsync(NoteAddedMessage message);
        Task NotifyNoteUpdatedAsync(NoteUpdatedMessage message);
        Task NotifyNoteDeletedAsync(NoteDeletedMessage message);

        // Activity feed notifications
        Task NotifyActivityUpdateAsync(ActivityResponse activity);
    }
}