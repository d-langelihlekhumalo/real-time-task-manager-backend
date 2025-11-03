namespace RealTimeTaskManager.Models
{
    public class TaskCompletionChangedMessage
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}