using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class PlantService : IPlantService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IPlantService> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public PlantService(ApplicationDbContext context, ILogger<IPlantService> logger, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task CreatePlantAsync(string plantType)
        {
            var newPlant = new Plant
            {
                PlantType = plantType,
                WaterLevel = 50,
                GrowthPoints = 0,
                HealthStatus = HealthStatus.Healthy,
                PlantedAt = DateTime.Now
            };

            await _context.Plants.AddAsync(newPlant);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New Plant Created: Id = {PlantId}, Type = {PlantType}", newPlant.Id, newPlant.PlantType);
        }

        public async Task<IEnumerable<Plant>> GetAllPlantsAsync()
        {
            return await _context.Plants.AsNoTracking().ToListAsync();
        }

        public async Task WaterAllPlants()
        {
            _logger.LogInformation("Recurring Job Started: Watering all plants.");
            var plants = await _context.Plants.ToListAsync();

            if (!plants.Any())
            {
                _logger.LogInformation("No plants to water");
                return;
            }

            foreach (var plant in plants)
            {
                plant.WaterLevel = Math.Min(plant.WaterLevel + 10, 100); // Increase by 10, but it must not exceed 100
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Recurring Job Finished: {Count} plants have been watered.", plants.Count());
        }

        public async Task GiveSunlightToAllPlants()
        {
            _logger.LogInformation("Recurring Job Started: Giving sunlight to all plants.");
            var plants = await _context.Plants.ToListAsync();

            if (!plants.Any())
            {
                _logger.LogInformation("No plants to give sunlight to.");
                return;
            }

            foreach (var plant in plants)
            {
                plant.GrowthPoints += 5;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Recurring Job Finished: {Count} plants received sunlight.", plants.Count);
        }

        public async Task PerformHealthCheckAsync()
        {
            _logger.LogInformation("Recurring Job Started. Performing health check on all plants.");
            var plants = await _context.Plants.ToListAsync();

            if (!plants.Any())
            {
                _logger.LogInformation("No plants to check");
                return;
            }

            const int ThirstyThreshold = 25;
            const int OverwateredThreshold = 85;

            foreach (var plant in plants)
            {
                var originalStatus = plant.HealthStatus;
                HealthStatus newStatus;

                if (plant.WaterLevel < ThirstyThreshold)
                {
                    newStatus = HealthStatus.Thirsty;
                }

                else if (plant.WaterLevel > OverwateredThreshold)
                {
                    newStatus = HealthStatus.Overwatered;
                }
                else
                {
                    newStatus = HealthStatus.Healthy;
                }
                // if unhealthy
                if (newStatus != plant.HealthStatus)
                {
                    plant.HealthStatus = newStatus;
                    _logger.LogInformation("Plant health status changed for PlantId: {PlantId}. Old: {OldStatus}, New: {NewStatus}", plant.Id, originalStatus, plant.HealthStatus);

                    if (plant.HealthStatus != HealthStatus.Healthy)
                    {
                        var jobId = _backgroundJobClient.Schedule<INotificationService>(
                            service => service.CheckAndNotifyForUnhealthyPlant(plant.Id),
                            TimeSpan.FromDays(1)
                        );

                        _logger.LogInformation("Scheduled a notification check job {JobId} for plant {PlantId} in 24 hours.", jobId, plant.Id);
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Recurring Job Finished: Health check complete for {Count} plants.", plants.Count);

        }

        public async Task ApplyFertilizerEffectAsync(int plantId)
        {
            _logger.LogInformation("Recurring Job started: Applying fertilizer effect");
            var plant = await _context.Plants.FindAsync(plantId);
            if (plant == null)
            {
                _logger.LogWarning("Delayed Job: Plant with Id {PlantId} not found. Fertilizer effect could not be applied.", plantId);
                return;
            }
            plant.GrowthPoints += 25;
            _logger.LogInformation("Growth points for PlantId: {PlantId} increased by 25. New total: {GrowthPoints}", plantId, plant.GrowthPoints);
            await _context.SaveChangesAsync();
        }
    }
}
