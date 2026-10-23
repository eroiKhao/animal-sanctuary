using AnimalSanctuary.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Species.Any() || context.Animals.Any())
            {
                return;
            }

            var species = new Species[]
            {
                new Species { Name = "Dogs" },
                new Species { Name = "Cats" },
                new Species { Name = "Rodents" },
                new Species { Name = "Birds" }
            };
            context.Species.AddRange(species);
            context.SaveChanges();

            var animals = new Animal[]
            {
                new Animal
                {
                    Name = "Rex", SpeciesId = species[0].Id, Breed = "German Shepherd",
                    AgeMonths = 30, Gender = "Male", HealthStatus = "Healthy", IsSterilized = true, Status = "Available",
                    Description = "Rex is a loyal and energetic dog. He loves long walks, knows basic commands and gets along well with people.",
                    PhotoUrl = "/images/animals/dog3.jpg", CreatedAt = new DateTime(2026, 9, 12)
                },
                new Animal
                {
                    Name = "Bella", SpeciesId = species[0].Id, Breed = "Labrador Retriever",
                    AgeMonths = 24, Gender = "Female", HealthStatus = "Healthy", IsSterilized = true, Status = "Available",
                    Description = "Bella is a gentle and friendly girl who adores children and loves playing with a ball in the yard.",
                    PhotoUrl = "/images/animals/dog2.jpg", CreatedAt = new DateTime(2026, 9, 15)
                },
                new Animal
                {
                    Name = "Whiskers", SpeciesId = species[1].Id, Breed = "British Shorthair",
                    AgeMonths = 18, Gender = "Male", HealthStatus = "Healthy", IsSterilized = true, Status = "Reserved",
                    Description = "Whiskers is a calm and independent cat. He enjoys quiet places, soft blankets and evening cuddles.",
                    PhotoUrl = "/images/animals/cat.jpg", CreatedAt = new DateTime(2026, 9, 18)
                },
                new Animal
                {
                    Name = "Luna", SpeciesId = species[1].Id, Breed = "Domestic Shorthair",
                    AgeMonths = 8, Gender = "Female", HealthStatus = "Under Treatment", IsSterilized = false, Status = "Available",
                    Description = "Luna is a playful kitten who is currently finishing her treatment. Soon she will be ready for a new home.",
                    PhotoUrl = "/images/animals/cat2.jpg", CreatedAt = new DateTime(2026, 9, 20)
                },
                new Animal
                {
                    Name = "Rocky", SpeciesId = species[0].Id, Breed = "Mixed Breed",
                    AgeMonths = 60, Gender = "Male", HealthStatus = "Healthy", IsSterilized = true, Status = "Available",
                    Description = "Rocky is a mature and very calm dog. He is the perfect companion for quiet walks and long naps.",
                    PhotoUrl = "/images/animals/dog.jpg", CreatedAt = new DateTime(2026, 9, 25)
                },
                new Animal
                {
                    Name = "Fluffy", SpeciesId = species[2].Id, Breed = "Angora Rabbit",
                    AgeMonths = 12, Gender = "Female", HealthStatus = "Healthy", IsSterilized = false, Status = "Available",
                    Description = "Fluffy is a soft and curious rabbit. She loves fresh vegetables and exploring new places.",
                    PhotoUrl = "/images/animals/rabbit.jpg", CreatedAt = new DateTime(2026, 10, 1)
                },
                new Animal
                {
                    Name = "Kiwi", SpeciesId = species[3].Id, Breed = "Budgerigar",
                    AgeMonths = 24, Gender = "Male", HealthStatus = "Healthy", IsSterilized = false, Status = "Available",
                    Description = "Kiwi is a cheerful and talkative parrot. He whistles simple melodies and brightens every morning.",
                    PhotoUrl = "/images/animals/bird.jpg", CreatedAt = new DateTime(2026, 10, 5)
                },
                new Animal
                {
                    Name = "Peanut", SpeciesId = species[2].Id, Breed = "Syrian Hamster",
                    AgeMonths = 6, Gender = "Male", HealthStatus = "Healthy", IsSterilized = true, Status = "Adopted",
                    Description = "Peanut is a small and funny hamster. He recently found his new family - a happy story of our shelter!",
                    PhotoUrl = "/images/animals/hamster.jpg", CreatedAt = new DateTime(2026, 10, 8)
                }
            };
            context.Animals.AddRange(animals);
            context.SaveChanges();

            var requests = new AdoptionRequest[]
            {
                new AdoptionRequest
                {
                    AnimalId = animals[0].Id, ApplicantName = "Anna Kovalenko",
                    RequestType = "Adoption", PhoneNumber = "+380 67 123 45 67",
                    HousingType = "Apartment", HasOtherPets = false,
                    PreferredVisitDate = new DateTime(2026, 10, 20),
                    ApplicantComment = "We have a big park near our home.",
                    Status = "Pending", CreatedAt = new DateTime(2026, 10, 12, 10, 30, 0)
                },
                new AdoptionRequest
                {
                    AnimalId = animals[4].Id, ApplicantName = "Ivan Petrenko",
                    RequestType = "Visit", PhoneNumber = "+380 50 987 65 43",
                    HousingType = "House", HasOtherPets = true,
                    PreferredVisitDate = new DateTime(2026, 10, 18),
                    ApplicantComment = "We would like to meet Rocky on the weekend.",
                    ManagerComment = "Approved - the visit is agreed.",
                    Status = "Approved", CreatedAt = new DateTime(2026, 10, 13, 14, 0, 0)
                }
            };
            context.AdoptionRequests.AddRange(requests);
            context.SaveChanges();
        }
    }
}
