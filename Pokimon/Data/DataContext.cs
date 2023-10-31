using Microsoft.EntityFrameworkCore;
using Pokimon.Models;

namespace Pokimon.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {            
        }
        public DbSet<Category> Categories  { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokimonOwner> PokimonOwner { get; set; }
        public DbSet<PokimonCategory> PokimonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokimonCategory>()
                .HasKey(pc => new { pc.PokimonId, pc.CategoryId });
            modelBuilder.Entity<PokimonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokimonCategories)
                .HasForeignKey(c => c.CategoryId);
            modelBuilder.Entity<PokimonCategory>()
                .HasOne(c => c.Category)
                .WithMany(pc => pc.PokimonCategories)
                .HasForeignKey(p => p.PokimonId);

            modelBuilder.Entity<PokimonOwner>()
                .HasKey(po => new { po.PokimonId, po.OwnerId });
            modelBuilder.Entity<PokimonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(po => po.PokimonOwners)
                .HasForeignKey(o => o.OwnerId);
            modelBuilder.Entity<PokimonOwner>()
                .HasOne(o => o.Owner)
                .WithMany(po => po.PokimonOwners)
                .HasForeignKey(p => p.PokimonId);
        }

    }
    
}
