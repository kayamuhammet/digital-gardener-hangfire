using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Plant
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? PlantType { get; set; }
    public int WaterLevel { get; set; }
    public int GrowthPoints { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HealthStatus HealthStatus { get; set; }
    public DateTime PlantedAt { get; set; }

}