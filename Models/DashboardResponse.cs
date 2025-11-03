namespace RealTimeTaskManager.Models
{
    public class DashboardResponse
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int TotalNotes { get; set; }
        public List<ActivityResponse> RecentActivities { get; set; } = new();

        // Additional stats that might be useful
        public double CompletionRate => TotalTasks > 0 ? (CompletedTasks * 100.0) / TotalTasks : 0;
        public int NotesPerTask => TotalTasks > 0 ? TotalNotes / TotalTasks : 0;
    }
}
