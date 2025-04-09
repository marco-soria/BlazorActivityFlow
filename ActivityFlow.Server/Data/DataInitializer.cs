using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace ActivityFlow.Server.Data;

public class DataInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DataInitializer> _logger;

    // Contraseña por defecto para todos los usuarios
    private const string DefaultPassword = "User123!";

    public DataInitializer(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<DataInitializer> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Iniciando la inicialización de datos...");

            // Asegurar que la base de datos esté creada y actualizada
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Base de datos migrada correctamente");

            // Seed roles
            await SeedRolesAsync();
            _logger.LogInformation("Roles inicializados correctamente");

            // Seed users
            var createdUsers = await SeedUsersAsync();
            _logger.LogInformation("Usuarios inicializados correctamente");

            // Seed categories
            await SeedCategoriesAsync();
            _logger.LogInformation("Categorías inicializadas correctamente");

            // Seed statuses
            await SeedStatusesAsync();
            _logger.LogInformation("Estados inicializados correctamente");

            // Seed activities
            await SeedActivitiesAsync(createdUsers);
            _logger.LogInformation("Actividades inicializadas correctamente");

            await _context.SaveChangesAsync();
            _logger.LogInformation("Inicialización de datos completada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la inicialización de datos");
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[] { "User", "Admin", "Manager" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task<List<ApplicationUser>> SeedUsersAsync()
    {
        var createdUsers = new List<ApplicationUser>();
        var users = new[]
        {
            new { UserName = "admin1", Email = "admin1@example.com", Role = "Admin" },
            new { UserName = "manager1", Email = "manager1@example.com", Role = "Manager" },
            new { UserName = "user1", Email = "user1@example.com", Role = "User" },
            new { UserName = "user2", Email = "user2@example.com", Role = "User" },
            new { UserName = "user3", Email = "user3@example.com", Role = "User" },
            new { UserName = "user4", Email = "user4@example.com", Role = "User" },
            new { UserName = "user5", Email = "user5@example.com", Role = "User" },
            new { UserName = "user6", Email = "user6@example.com", Role = "User" },
            new { UserName = "user7", Email = "user7@example.com", Role = "User" },
            new { UserName = "user8", Email = "user8@example.com", Role = "User" }
        };

        foreach (var userInfo in users)
        {
            var existingUser = await _userManager.FindByEmailAsync(userInfo.Email);
            if (existingUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userInfo.UserName,
                    Email = userInfo.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, DefaultPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userInfo.Role);
                    createdUsers.Add(user);
                }
                else
                {
                    _logger.LogError($"Error al crear usuario {userInfo.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                createdUsers.Add(existingUser);
            }
        }

        return createdUsers;
    }

    private async Task SeedCategoriesAsync()
    {
        if (!await _context.Categories.AnyAsync())
        {
            var categories = new[]
            {
                new Category { Name = "Work", Description = "Work related tasks" },
                new Category { Name = "Personal", Description = "Personal tasks" },
                new Category { Name = "Health", Description = "Health and wellness tasks" },
                new Category { Name = "Finance", Description = "Financial tasks" },
                new Category { Name = "Education", Description = "Educational tasks" },
                new Category { Name = "Shopping", Description = "Shopping tasks" },
                new Category { Name = "Travel", Description = "Travel planning tasks" },
                new Category { Name = "Home", Description = "Home improvement tasks" },
                new Category { Name = "Hobbies", Description = "Hobby related tasks" },
                new Category { Name = "Miscellaneous", Description = "Miscellaneous tasks" }
            };

            await _context.Categories.AddRangeAsync(categories);
        }
    }

    private async Task SeedStatusesAsync()
    {
        if (!await _context.Statuses.AnyAsync())
        {
            var statuses = new List<Status>
            {
                new Status { Id = 1, Name = ActivityStatus.Pending, Description = "Actividad pendiente", DisplayOrder = 1 },
                new Status { Id = 2, Name = ActivityStatus.InProgress, Description = "Actividad en progreso", DisplayOrder = 2 },
                new Status { Id = 3, Name = ActivityStatus.Completed, Description = "Actividad completada", DisplayOrder = 3 },
                new Status { Id = 4, Name = ActivityStatus.Cancelled, Description = "Actividad cancelada", DisplayOrder = 4 },
                new Status { Id = 5, Name = ActivityStatus.OnHold, Description = "Actividad en espera", DisplayOrder = 5 }
            };

            await _context.Statuses.AddRangeAsync(statuses);
        }
    }

    private async Task SeedActivitiesAsync(List<ApplicationUser> users)
    {
        var categories = await _context.Categories.ToListAsync();
        var statuses = await _context.Statuses.ToListAsync();
        var existingActivities = await _context.Activities.ToListAsync();

        if (categories.Count > 0 && users.Count > 0 && statuses.Count > 0)
        {
            if (existingActivities.Count == 0)
            {
                var activities = new List<Activity>();
                for (int i = 1; i <= 20; i++)
                {
                    activities.Add(new Activity
                    {
                        Title = $"Activity {i}",
                        Description = $"Description for activity {i}",
                        StatusId = statuses[i % statuses.Count].Id,
                        UserId = users[i % users.Count].Id,
                        CategoryId = categories[i % categories.Count].Id,
                        Priority = i % 3, // 0: Normal, 1: High, 2: Urgent
                        CreatedAt = DateTime.UtcNow.AddDays(-i),
                        DueDate = DateTime.UtcNow.AddDays(i)
                    });
                }
                await _context.Activities.AddRangeAsync(activities);
            }
            else
            {
                foreach (var activity in existingActivities)
                {
                    if (activity.Priority == 0 && activity.CreatedAt == default)
                    {
                        activity.Priority = activity.Id % 3;
                        activity.CreatedAt = DateTime.UtcNow.AddDays(-activity.Id);
                        activity.DueDate = DateTime.UtcNow.AddDays(activity.Id);
                    }
                }
            }
        }
    }
} 