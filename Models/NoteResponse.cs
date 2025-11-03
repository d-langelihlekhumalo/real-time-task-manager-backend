namespace RealTimeTaskManager.Models
{
    public class NoteResponse
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
