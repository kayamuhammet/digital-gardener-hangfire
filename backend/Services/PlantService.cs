using backend.Services;
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
    }
}
