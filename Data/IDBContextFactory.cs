namespace RealTimeTaskManager.Data
{
    public interface IDBContextFactory
    {
        ApplicationDbContext CreateDbContext();
    }
}
