using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetSpot.DATA.Entities;
using System.Linq;

namespace PetSpot.DATA
{
    public class PetSpotDbContext : IdentityDbContext<User, UserRole, string>
    {
        public PetSpotDbContext(DbContextOptions<PetSpotDbContext> options)
            : base(options)
        {

        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Species> Species { get; set; }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        private void UpdateSoftDeleteStatuses()
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().
                Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is IDeletable entity)
                {
                    item.State = EntityState.Unchanged;
                    entity.IsDeleted = true;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(x => x.Breed)
                    .HasMaxLength(64);

                entity.Property(x => x.Description)
                    .HasMaxLength(512);

                entity.HasOne(x => x.User)
                    .WithMany(y => y.Animals)
                    .HasForeignKey(x => x.UserId);

                entity.HasOne(x => x.Location)
                    .WithOne(y => y.Animal)
                    .HasForeignKey<Location>(y => y.AnimalId);

                entity.HasOne(x => x.Species)
                    .WithMany(y => y.Animals)
                    .HasForeignKey(x => x.SpeciesId);

            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CoordX)
                    .IsRequired();

                entity.Property(x => x.CoordY)
                    .IsRequired();
            });

            modelBuilder.Entity<Species>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasMany(x => x.Animals)
                    .WithOne(y => y.Species)
                    .HasForeignKey(y => y.SpeciesId);

                entity.HasData(
                    new Species { Id = 1, Name = "Dog" },
                    new Species { Id = 2, Name = "Cat" },
                    new Species { Id = 3, Name = "Pig" },
                    new Species { Id = 4, Name = "Guinea Pig" },
                    new Species { Id = 5, Name = "Horse" },
                    new Species { Id = 6, Name = "Hedgehog" },
                    new Species { Id = 7, Name = "Chicken" },
                    new Species { Id = 8, Name = "Cow" },
                    new Species { Id = 9, Name = "Bunny" },
                    new Species { Id = 10, Name = "Ferret" }
                    );
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasData(
                    new UserRole { Id = "1", Name = "Registered user", NormalizedName = "registered user" },
                    new UserRole { Id = "2", Name = "Administrator", NormalizedName = "administrator" }
                );
            });
        }
    }
}
