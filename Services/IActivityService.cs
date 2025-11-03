using RealTimeTaskManager.Enums;
using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public interface IActivityService
    {
        Task RecordActivityAsync(ActivityActionEnum action, EntityTypeEnum entityType, Guid entityId, string entityTitle, string? description = null, string? additionalData = null);
        Task<List<ActivityResponse>> GetRecentActivitiesAsync(int count = 20);
        Task<DashboardResponse> GetDashboardDataAsync();
    }
}
