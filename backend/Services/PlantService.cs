using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class PlantService : IPlantService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IPlantService> _logger;
        public PlantService(ApplicationDbContext context, ILogger<IPlantService> logger)
        {
            _context = context;
            _logger = logger;
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

                if (plant.WaterLevel < ThirstyThreshold)
                {
                    plant.HealthStatus = HealthStatus.Thirsty;
                }

                else if (plant.WaterLevel > OverwateredThreshold)
                {
                    plant.HealthStatus = HealthStatus.Overwatered;
                }
                else
                {
                    plant.HealthStatus = HealthStatus.Healthy;
                }

                if (originalStatus != plant.HealthStatus)
                {
                    _logger.LogInformation("Plant health status changed for PlantId: {PlantId}. Old: {OldStatus}, New: {NewStatus}",
                        plant.Id, originalStatus, plant.HealthStatus);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Recurring Job Finished: Health check complete for {Count} plants.", plants.Count);

        }
    }
}
