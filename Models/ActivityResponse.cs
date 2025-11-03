using RealTimeTaskManager.Enums;

namespace RealTimeTaskManager.Models
{
    public class ActivityResponse
    {
        public Guid Id { get; set; }
        public ActivityActionEnum Action { get; set; }
        public EntityTypeEnum EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string EntityTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ActionDisplayName => GetActionDisplayName();
        public string EntityTypeDisplayName => EntityType.ToString();

        private string GetActionDisplayName()
        {
            return Action switch
            {
                ActivityActionEnum.TaskCreated => "Task Created",
                ActivityActionEnum.TaskUpdated => "Task Updated",
                ActivityActionEnum.TaskDeleted => "Task Deleted",
                ActivityActionEnum.TaskCompleted => "Task Completed",
                ActivityActionEnum.TaskUncompleted => "Task Uncompleted",
                ActivityActionEnum.NoteCreated => "Note Created",
                ActivityActionEnum.NoteUpdated => "Note Updated",
                ActivityActionEnum.NoteDeleted => "Note Deleted",
                _ => Action.ToString()
            };
        }
    }
}
