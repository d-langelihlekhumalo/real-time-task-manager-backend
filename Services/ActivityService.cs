using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealTimeTaskManager.Data;
using RealTimeTaskManager.Entities;
using RealTimeTaskManager.Enums;
using RealTimeTaskManager.Models;

namespace RealTimeTaskManager.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ActivityService> _logger;

        public ActivityService(ApplicationDbContext dbContext, IMapper mapper, ILogger<ActivityService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task RecordActivityAsync(ActivityActionEnum action, EntityTypeEnum entityType, Guid entityId, string entityTitle, string? description = null, string? additionalData = null)
        {
            var activity = new ActivityEntity
            {
                Id = Guid.NewGuid(),
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                EntityTitle = entityTitle,
                Description = description ?? GenerateDefaultDescription(action, entityTitle),
                AdditionalData = additionalData,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Activities.Add(activity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ActivityResponse>> GetRecentActivitiesAsync(int count = 20)
        {
            var activities = await _dbContext.Activities
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<List<ActivityResponse>>(activities);
        }

        public async Task<DashboardResponse> GetDashboardDataAsync()
        {
            var tasks = await _dbContext.Tasks.ToListAsync();
            var totalTasks = tasks.Count;
            var completedTasks = tasks.Count(t => t.IsCompleted);
            var totalNotes = await _dbContext.Notes.CountAsync();
            var recentActivities = await GetRecentActivitiesAsync(10);

            return new DashboardResponse
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = totalTasks - completedTasks,
                TotalNotes = totalNotes,
                RecentActivities = recentActivities
            };
        }

        private static string GenerateDefaultDescription(ActivityActionEnum action, string entityTitle)
        {
            return action switch
            {
                ActivityActionEnum.TaskCreated => $"Task '{entityTitle}' was created",
                ActivityActionEnum.TaskUpdated => $"Task '{entityTitle}' was updated",
                ActivityActionEnum.TaskDeleted => $"Task '{entityTitle}' was deleted",
                ActivityActionEnum.TaskCompleted => $"Task '{entityTitle}' was completed",
                ActivityActionEnum.TaskUncompleted => $"Task '{entityTitle}' was marked as pending",
                ActivityActionEnum.NoteCreated => $"Note was added to task '{entityTitle}'",
                ActivityActionEnum.NoteUpdated => $"Note was updated in task '{entityTitle}'",
                ActivityActionEnum.NoteDeleted => $"Note was deleted from task '{entityTitle}'",
                _ => $"{action} performed on {entityTitle}"
            };
        }
    }
}
