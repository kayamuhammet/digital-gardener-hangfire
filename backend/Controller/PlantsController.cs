using backend.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PlantController : ControllerBase
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IPlantService _plantService;

    public PlantController(IBackgroundJobClient backgroundJobClient, IPlantService plantService)
    {
        _backgroundJobClient = backgroundJobClient;
        _plantService = plantService;
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

}