using backend.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PlantsController : ControllerBase
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IPlantService _plantService;
    private readonly ILogger<PlantsController> _logger;

    public PlantsController(IBackgroundJobClient backgroundJobClient, IPlantService plantService, ILogger<PlantsController> logger)
    {
        _backgroundJobClient = backgroundJobClient;
        _plantService = plantService;
        _logger = logger;
    }
    [HttpPost("plant-seed")]
    public IActionResult PlantNewSeed(string plantType)
    {
        var jobId = _backgroundJobClient.Enqueue<IPlantService>(service => service.CreatePlantAsync(plantType));

        return Accepted($"Job '{jobId}' is enqueuded. A new '{plantType}' will be planted.");
    }
    [HttpGet]
    public async Task<IActionResult> GetAllPlants()
    {
        var plants = await _plantService.GetAllPlantsAsync();
        return Ok(plants);
    }

    [HttpPost("{plantId}/fertilize")]
    public IActionResult FertilizePlant(int plantId)
    {
        _logger.LogInformation("Request received to fertilize plant with Id: {PlantId}", plantId);

        var jobId = _backgroundJobClient.Schedule<IPlantService>(
            service => service.ApplyFertilizerEffectAsync(plantId),
            TimeSpan.FromHours(2)
        );

        _logger.LogInformation("Job {JobId} scheduled to apply fertilizer effect in 2 hours for PlantId: {PlantId}", jobId, plantId);
        return Accepted($"Job '{jobId}' has been scheduled. Fertilizer effect will be applied in 2 hours to plant {plantId}.");
    }

}