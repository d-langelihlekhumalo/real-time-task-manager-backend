namespace RealTimeTaskManager.Models
{
    public class CreateNoteRequest
    {
        public Guid TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
