using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public interface ITaskService
    {
        Task<List<TaskResponse>> GetAllTasksAsync();
        Task<TaskResponse?> GetTaskByIdAsync(Guid id);
        Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request);
        Task<TaskResponse?> UpdateTaskAsync(Guid id, UpdateTaskRequest request);
        Task<bool> DeleteTaskAsync(Guid id);
        Task<bool> ToggleTaskCompletionAsync(Guid id);
    }
}
