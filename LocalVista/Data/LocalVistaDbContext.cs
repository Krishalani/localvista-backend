using LocalVista.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocalVista.Data;

public class LocalVistaDbContext : IdentityDbContext<ApplicationUser>
{
    public LocalVistaDbContext(DbContextOptions<LocalVistaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Attraction> Attractions => Set<Attraction>();
    public DbSet<AttractionImage> AttractionImages => Set<AttractionImage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        builder.Entity<Attraction>(entity =>
        {
            entity.ToTable("Attractions");
            entity.HasKey(e => e.AttractionId);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.OpeningHours).HasMaxLength(200);
            entity.Property(e => e.DistanceKm).HasColumnType("decimal(6,2)");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9,6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9,6)");
            entity.Property(e => e.CreatedAtUtc).HasColumnType("datetime2(0)");
            entity.Property(e => e.UpdatedAtUtc).HasColumnType("datetime2(0)");

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Attractions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<AttractionImage>(entity =>
        {
            entity.ToTable("AttractionImages");
            entity.HasKey(e => e.AttractionImageId);
            entity.Property(e => e.ImageUrl).HasMaxLength(1000).IsRequired();
            entity.HasIndex(e => new { e.AttractionId, e.SortOrder }).IsUnique();

            entity.HasOne(e => e.Attraction)
                .WithMany(a => a.Images)
                .HasForeignKey(e => e.AttractionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
