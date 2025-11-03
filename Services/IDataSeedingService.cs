namespace RealTimeTaskManager.Services
{
    public interface IDataSeedingService
    {
        Task SeedDemoDataAsync();
        Task ResetDemoDataAsync();
    }
}