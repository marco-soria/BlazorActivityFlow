using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ActivityFlow.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for other entities
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuración de ApplicationUser
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // Configuración de Activity
        builder.Entity<Activity>(entity =>
        {
            entity.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Category)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Status)
                .WithMany(s => s.Activities)
                .HasForeignKey(a => a.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.AssignedTo)
                .WithMany()
                .HasForeignKey(a => a.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(a => a.Progress)
                .HasPrecision(5, 2);
        });

        // Configuración de Status
        builder.Entity<Status>(entity =>
        {
            entity.HasData(
                new Status { Id = 1, Name = ActivityStatus.Pending, Description = "Actividad pendiente", DisplayOrder = 1, CreatedAt = new DateTime(2024, 1, 1) },
                new Status { Id = 2, Name = ActivityStatus.InProgress, Description = "Actividad en progreso", DisplayOrder = 2, CreatedAt = new DateTime(2024, 1, 1) },
                new Status { Id = 3, Name = ActivityStatus.Completed, Description = "Actividad completada", DisplayOrder = 3, CreatedAt = new DateTime(2024, 1, 1) },
                new Status { Id = 4, Name = ActivityStatus.Cancelled, Description = "Actividad cancelada", DisplayOrder = 4, CreatedAt = new DateTime(2024, 1, 1) }
            );

            entity.Property(s => s.DisplayOrder)
                .HasDefaultValue(0);
        });

        // Configuración de Category
        builder.Entity<Category>(entity =>
        {
            entity.HasData(
                new Category { Id = 1, Name = "Trabajo", Description = "Actividades relacionadas con el trabajo", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 2, Name = "Personal", Description = "Actividades personales", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 3, Name = "Estudio", Description = "Actividades de estudio", CreatedAt = new DateTime(2024, 1, 1) },
                new Category { Id = 4, Name = "Otros", Description = "Otras actividades", CreatedAt = new DateTime(2024, 1, 1) }
            );
        });
    }
} 