namespace AnimalSanctuary.Models
{
    public class AdoptionRequest
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }

        public Animal? Animal { get; set; }

        public string ApplicantName { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string HousingType { get; set; } = string.Empty;
        public bool HasOtherPets { get; set; }

        public DateTime PreferredVisitDate { get; set; }

        public string? ApplicantComment { get; set; }
        public string? ManagerComment { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; }
    }
}
