using System.ComponentModel.DataAnnotations;

namespace AnimalSanctuary.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public int AnimalId { get; set; }
        public DateTime AddedAt { get; set; }
        public ApplicationUser? User { get; set; }
        public Animal? Animal { get; set; }
    }
}
