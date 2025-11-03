namespace RealTimeTaskManager.Models
{
    public class NoteDeletedMessage
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
    }
}