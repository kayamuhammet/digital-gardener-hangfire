namespace backend.Services
{
    public interface IPlantService
    {
        Task CreatePlantAsync(string plantType);
        Task<IEnumerable<Plant>> GetAllPlantsAsync();
    }
}
