namespace backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(ApplicationDbContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CheckAndNotifyForUnhealthyPlant(int plantId)
        {
            _logger.LogInformation("Delayed Job (Notification Check) Started for PlantId: {PlantId}", plantId);
            var plant = await _context.Plants.FindAsync(plantId);

            if (plant == null)
            {
                _logger.LogWarning("Notification Check: Plant with Id {PlantId} not found.", plantId);
                return;
            }

            if (plant.HealthStatus != HealthStatus.Healthy)
            {
                _logger.LogCritical("ALERT: Plant {PlantId} ({PlantType}) is still in an unhealthy state: {HealthStatus}. User needs to take action!", plant.Id, plant.PlantType, plant.HealthStatus);
            }
            else
            {
                _logger.LogInformation("Notification Check: Plant {PlantId} is healthy again. No notification needed.", plantId);
            }


        }
    }
}