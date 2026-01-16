using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TemplateAPI.Application.Features.Branch.Commands.Create;
using TemplateAPI.Application.Features.Company.Commands.Create;
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
        IdentityRole administratorRole = new(Roles.ADMIN);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        IdentityRole auditorRole = new(Roles.AUDITOR);

        if (_roleManager.Roles.All(r => r.Name != Roles.AUDITOR))
        {
            await _roleManager.CreateAsync(auditorRole);
        }

        // Default Currency
        Currency defaultCurrency = null!;
        if (!_context.Set<Currency>().Any())
        {
            defaultCurrency = new Currency { Code = "USD", IsActive = true };
            _context.Set<Currency>().Add(defaultCurrency);
            await _context.SaveChangesAsync();
        }
        else
        {
            defaultCurrency = await _context.Set<Currency>().FirstAsync();
        }

        // Default Company
        int companyId = 0;
        if (!_context.Companies.Any())
        {
            companyId = await _sender.Send(new CreateCompanyCommand
            {
                Name = "Template Tribe Inc.",
                LegalName = "Template Tribe Incorporated",
                TaxId = "123456789",
                RegistrationNumber = "REG-2026-001",
                Email = "contact@templatetribe.com",
                Phone = "+1-555-0100",
                Website = "https://templatetribe.com",
                LogoUrl = "",
                AddressLine1 = "123 Business Street",
                AddressLine2 = "Suite 100",
                City = "Tech City",
                State = "California",
                Country = "United States",
                CurrencyId = defaultCurrency.Id,
                FiscalYearStartMonth = 1
            });
        }
        else
        {
            companyId = _context.Companies.First().Id;
        }

        // Default Branch
        int branchId = 0;
        if (!_context.Branches.Any())
        {
            branchId = await _sender.Send(new CreateBranchCommand
            {
                Name = "Main Office",
                Code = "HQ-001",
                Email = "mainoffice@templatetribe.com",
                Phone = "+1-555-0101",
                Description = "Corporate Headquarters",
                AddressLine1 = "123 Business Street",
                AddressLine2 = "Suite 100",
                City = "Tech City",
                BranchType = "Office",
                IsHeadquarters = true,
                CompanyId = companyId
            });
        }
        else
        {
            branchId = _context.Branches.First().Id;
        }

        // Default users
        ApplicationUser administrator =
            new() { UserName = "admin@templatetribe.co.zw", Email = "admin@templatetribe.co.zw" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Password@1");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }

            await _sender.Send(new CreatePersonCommand
            {
                Email = "admin@templatetribe.co.zw",
                FirstName = "Tendai",
                LastName = "Moyo",
                ApplicationUserId = administrator.Id,
                Role = Roles.ADMIN,
                BranchId = branchId
            });
        }

        ApplicationUser auditor =
            new() { UserName = "auditor@templatetribe.co.zw", Email = "auditor@templatetribe.co.zw" };

        if (_userManager.Users.All(u => u.UserName != auditor.UserName))
        {
            await _userManager.CreateAsync(auditor, "Password@1");
            if (!string.IsNullOrEmpty(auditorRole.Name))
            {
                await _userManager.AddToRolesAsync(auditor, new[] { auditorRole.Name });
            }

            await _sender.Send(new CreatePersonCommand
            {
                Email = "auditor@templatetribe.co.zw",
                FirstName = "Rumbidzai",
                LastName = "Ncube",
                ApplicationUserId = auditor.Id,
                Role = Roles.AUDITOR,
                BranchId = branchId
            });
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
