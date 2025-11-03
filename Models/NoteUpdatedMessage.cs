namespace RealTimeTaskManager.Models
{
    public class NoteUpdatedMessage
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}