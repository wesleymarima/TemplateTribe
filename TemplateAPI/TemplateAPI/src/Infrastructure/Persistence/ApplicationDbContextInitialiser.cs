using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TemplateAPI.Application.Features.Person.Commands.Create;
using TemplateAPI.Domain.Constants;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Infrastructure.Identity;

namespace TemplateAPI.Infrastructure.Persistence;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        ApplicationDbContextInitialiser initialiser =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ISender _sender;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        ISender sender)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _sender = sender;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        IdentityRole administratorRole = new(Roles.ADMINISTRATOR);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        IdentityRole auditorRole = new(Roles.AUDITOR);

        if (_roleManager.Roles.All(r => r.Name != Roles.AUDITOR))
        {
            await _roleManager.CreateAsync(auditorRole);
        }

        // Default users
        ApplicationUser administrator =
            new() { UserName = "admin@templatetribe.com", Email = "admin@templatetribe.com" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Password@1");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }

            await _sender.Send(new CreatePersonCommand
            {
                Email = "admin@templatetribe.com",
                FirstName = "Admin",
                LastName = "User",
                ApplicationUserId = administrator.Id,
                Role = Roles.ADMINISTRATOR
            });
        }

        ApplicationUser auditor = new() { UserName = "auditor@templatetribe.com", Email = "auditor@templatetribe.com" };

        if (_userManager.Users.All(u => u.UserName != auditor.UserName))
        {
            await _userManager.CreateAsync(auditor, "Password@1");
            if (!string.IsNullOrEmpty(auditorRole.Name))
            {
                await _userManager.AddToRolesAsync(auditor, new[] { auditorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯" },
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
