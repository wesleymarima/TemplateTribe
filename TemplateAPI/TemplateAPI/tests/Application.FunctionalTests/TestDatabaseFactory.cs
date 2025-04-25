using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync(IUser currentUser)
    {
        // Testcontainers requires Docker. To use a local SQL Server database instead,
        // switch to `SqlTestDatabase` and update appsettings.json.
        SqlTestcontainersTestDatabase database = new(currentUser);

        await database.InitialiseAsync();

        return database;
    }
}
