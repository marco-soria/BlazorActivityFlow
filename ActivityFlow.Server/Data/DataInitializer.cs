using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Server.Data;

public class DataInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        // Seed roles
        if (!await _roleManager.Roles.AnyAsync())
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

        // Seed users
        var users = new[]
        {
            new IdentityUser { UserName = "admin1", Email = "admin1@example.com" },
            new IdentityUser { UserName = "manager1", Email = "manager1@example.com" },
            new IdentityUser { UserName = "user1", Email = "user1@example.com" },
            new IdentityUser { UserName = "user2", Email = "user2@example.com" },
            new IdentityUser { UserName = "user3", Email = "user3@example.com" },
            new IdentityUser { UserName = "user4", Email = "user4@example.com" },
            new IdentityUser { UserName = "user5", Email = "user5@example.com" },
            new IdentityUser { UserName = "user6", Email = "user6@example.com" },
            new IdentityUser { UserName = "user7", Email = "user7@example.com" },
            new IdentityUser { UserName = "user8", Email = "user8@example.com" }
        };

        var userRoles = new[] { "Admin", "Manager", "User", "User", "User", "User", "User", "User", "User", "User" };
        var passwords = new[] { "User123!", "User123!", "User123!", "User123!", "User123!", "User123!", "User123!", "User123!", "User123!", "User123!" };

        var createdUsers = new List<IdentityUser>();

        if (!await _userManager.Users.AnyAsync())
        {
            for (int i = 0; i < users.Length; i++)
            {
                var user = users[i];
                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await _userManager.CreateAsync(user, passwords[i]);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, userRoles[i]);
                        createdUsers.Add(user);
                    }
                }
                else
                {
                    createdUsers.Add(await _userManager.FindByEmailAsync(user.Email));
                }
            }
        }

        // Seed categories
        if (!_context.Categories.Any())
        {
            _context.Categories.AddRange(
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
            );
        }

        // Seed statuses
        if (!_context.Statuses.Any())
        {
            _context.Statuses.AddRange(
                new Status { Name = ActivityStatus.Pending, Description = "Task is pending", Order = 1 },
                new Status { Name = ActivityStatus.InProgress, Description = "Task is in progress", Order = 2 },
                new Status { Name = ActivityStatus.Completed, Description = "Task is completed", Order = 3 },
                new Status { Name = ActivityStatus.Cancelled, Description = "Task is cancelled", Order = 4 },
                new Status { Name = ActivityStatus.OnHold, Description = "Task is on hold", Order = 5 }
            );
        }

        // Seed activities
        if (!_context.Activities.Any())
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories.Count > 0 && createdUsers.Count > 0)
            {
                var activities = new List<Activity>();
                for (int i = 1; i <= 20; i++)
                {
                    activities.Add(new Activity
                    {
                        Title = $"Activity {i}",
                        Description = $"Description for activity {i}",
                        StatusId = (i % 5) + 1,
                        UserId = createdUsers[i % createdUsers.Count].Id,
                        CategoryId = categories[i % categories.Count].Id
                    });
                }
                _context.Activities.AddRange(activities);
            }
        }

        await _context.SaveChangesAsync();
    }
} 