using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeTaskManager.Entities
{
    [Table("tb_NoteEntity")]
    public class NoteEntity
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public TaskEntity Task { get; set; } = null!;
    }
}
