using AnimalSanctuary.Models;
using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Table of animal species (directory).
        public DbSet<Species> Species { get; set; }

        // Table of animal catalog cards.
        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One species contains many animals (one-to-many).
            // The foreign key is Animal.SpeciesId.
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Species)
                .WithMany()
                .HasForeignKey(a => a.SpeciesId);
        }
    }
