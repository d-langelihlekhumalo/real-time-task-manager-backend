using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeTaskManager.Entities
{
    [Table("tb_Task")]
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public List<NoteEntity> Notes { get; set; } = new();
    }
}
