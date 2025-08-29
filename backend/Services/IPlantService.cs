namespace backend.Services
{
    public interface IPlantService
    {
        Task CreatePlantAsync(string plantType);
        Task<IEnumerable<Plant>> GetAllPlantsAsync();
        Task WaterAllPlants();
        Task GiveSunlightToAllPlants();
        Task PerformHealthCheckAsync();
        Task ApplyFertilizerEffectAsync(int plantId);
    }
}
