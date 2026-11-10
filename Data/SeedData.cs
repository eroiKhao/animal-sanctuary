using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            string[] roleNames = { "Administrator", "Manager", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (await userManager.FindByEmailAsync("admin@shelter.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@shelter.com",
                    Email = "admin@shelter.com",
                    FirstName = "Olena",
                    LastName = "Adminova",
                    PhoneNumber = "+380671112233",
                    EmailConfirmed = true,
                    RegistrationDate = new DateTime(2026, 9, 1)
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Administrator");
            }

            if (await userManager.FindByEmailAsync("manager@shelter.com") == null)
            {
                var manager = new ApplicationUser
                {
                    UserName = "manager@shelter.com",
                    Email = "manager@shelter.com",
                    FirstName = "Ivan",
                    LastName = "Managerov",
                    PhoneNumber = "+380671112244",
                    EmailConfirmed = true,
                    RegistrationDate = new DateTime(2026, 9, 2)
                };
                await userManager.CreateAsync(manager, "Manager123!");
                await userManager.AddToRoleAsync(manager, "Manager");
            }

            if (!await context.Species.AnyAsync())
            {
                var species = new Species[]
                {
                    new Species { Name = "Dogs" },
                    new Species { Name = "Cats" },
                    new Species { Name = "Rodents" },
                    new Species { Name = "Birds" }
                };
                context.Species.AddRange(species);
                await context.SaveChangesAsync();

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
                await context.SaveChangesAsync();
            }

            var demoUser = await userManager.FindByEmailAsync("user@shelter.com");
            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    UserName = "user@shelter.com",
                    Email = "user@shelter.com",
                    FirstName = "Maria",
                    LastName = "Petrenko",
                    PhoneNumber = "+380991234567",
                    EmailConfirmed = true,
                    RegistrationDate = new DateTime(2026, 10, 10)
                };
                await userManager.CreateAsync(demoUser, "User123!");
                await userManager.AddToRoleAsync(demoUser, "User");
            }

            if (!await context.Favorites.AnyAsync())
            {
                var rex = await context.Animals.FirstAsync(a => a.Name == "Rex");
                var luna = await context.Animals.FirstAsync(a => a.Name == "Luna");
                context.Favorites.AddRange(
                    new Favorite { UserId = demoUser.Id, AnimalId = rex.Id, AddedAt = new DateTime(2026, 10, 11) },
                    new Favorite { UserId = demoUser.Id, AnimalId = luna.Id, AddedAt = new DateTime(2026, 10, 12) }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.AdoptionRequests.AnyAsync())
            {
                var bella = await context.Animals.FirstAsync(a => a.Name == "Bella");
                var manager = await userManager.FindByEmailAsync("manager@shelter.com");
                context.AdoptionRequests.Add(new AdoptionRequest
                {
                    AnimalId = bella.Id,
                    UserId = demoUser.Id,
                    RequestType = "Adoption",
                    PhoneNumber = "+380991234567",
                    HousingType = "House",
                    HasOtherPets = false,
                    PreferredVisitDate = new DateTime(2026, 11, 7),
                    ApplicantComment = "We have a big yard and a lot of love to give.",
                    Status = "Pending",
                    CreatedAt = new DateTime(2026, 10, 14)
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
