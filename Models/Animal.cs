namespace AnimalSanctuary.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SpeciesId { get; set; }
        public Species? Species { get; set; }
        public string? Breed { get; set; }
        public int AgeMonths { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string HealthStatus { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public double AgeYears => Math.Round(AgeMonths / 12.0, 1);
    }
}
