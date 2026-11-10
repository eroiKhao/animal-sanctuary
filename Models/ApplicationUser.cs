using Microsoft.AspNetCore.Identity;

namespace AnimalSanctuary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
    }
}
