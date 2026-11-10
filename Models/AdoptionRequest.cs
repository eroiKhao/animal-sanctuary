using System.ComponentModel.DataAnnotations;

namespace AnimalSanctuary.Models
{
    public class AdoptionRequest
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        
        public Animal? Animal { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

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
