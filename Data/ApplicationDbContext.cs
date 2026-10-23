using AnimalSanctuary.Models;
using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Species> Species { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Species)
                .WithMany()
                .HasForeignKey(a => a.SpeciesId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Animal)
                .WithMany()
                .HasForeignKey(f => f.AnimalId);
            
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.AnimalId, f.VisitorKey })
                .IsUnique();
            
            modelBuilder.Entity<AdoptionRequest>()
                .HasOne(r => r.Animal)
                .WithMany()
                .HasForeignKey(r => r.AnimalId);
        }
    }
