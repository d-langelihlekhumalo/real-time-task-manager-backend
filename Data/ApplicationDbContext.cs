using Microsoft.EntityFrameworkCore;
using RealTimeTaskManager.Entities;

namespace RealTimeTaskManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Notes)
                      .WithOne(e => e.Task)
                      .HasForeignKey(e => e.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NoteEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Task)
                      .WithMany(e => e.Notes)
                      .HasForeignKey(e => e.TaskId);
            });

            modelBuilder.Entity<ActivityEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
