using RealTimeTaskManager.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeTaskManager.Entities
{
    [Table("tb_ActivityEntity")]
    public class ActivityEntity
    {
        public Guid Id { get; set; }
        public ActivityActionEnum Action { get; set; }
        public EntityTypeEnum EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string EntityTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Store additional data as JSON for flexibility
        public string? AdditionalData { get; set; }
    }
}
