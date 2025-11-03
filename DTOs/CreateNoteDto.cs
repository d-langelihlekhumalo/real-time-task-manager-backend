namespace RealTimeTaskManager.DTOs
{
    public class CreateNoteDto
    {
        public Guid TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
