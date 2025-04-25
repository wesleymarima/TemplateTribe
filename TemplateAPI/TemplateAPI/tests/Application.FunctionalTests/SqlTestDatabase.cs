using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Respawn;
using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Infrastructure.Persistence;

namespace TemplateAPI.Application.FunctionalTests;

public class SqlTestDatabase : ITestDatabase
{
    private readonly string _connectionString = null!;
    private readonly IUser _currentUser;
    private SqlConnection _connection = null!;
    private Respawner _respawner = null!;

    public SqlTestDatabase(IUser currentUser)
    {
        _currentUser = currentUser;
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = configuration.GetConnectionString("TemplateAPIDb");

        Guard.Against.Null(connectionString);

        _connectionString = connectionString;
    }

    public async Task InitialiseAsync()
    {
        _connection = new SqlConnection(_connectionString);

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning))
            .Options;

        ApplicationDbContext context = new(options, _currentUser);

        context.Database.EnsureDeleted();
        context.Database.Migrate();

        _respawner = await Respawner.CreateAsync(_connectionString,
            new RespawnerOptions { TablesToIgnore = ["__EFMigrationsHistory"] });
    }

    public DbConnection GetConnection()
    {
        return _connection;
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_connectionString);
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
