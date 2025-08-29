namespace backend.Services
{
    public interface INotificationService
    {
        Task CheckAndNotifyForUnhealthyPlant(int plantId);
    }
}
