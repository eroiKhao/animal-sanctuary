namespace AnimalSanctuary.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public Animal? Animal { get; set; }
        public string VisitorKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
