using Serilog;
using TemplateAPI.Infrastructure;
using TemplateAPI.Infrastructure.Persistence;
using TemplateAPI.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();
// Configure the HTTP request pipeline.
await app.InitialiseDatabaseAsync();

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

// CORS
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

// Configure OpenAPI/Swagger
app.UseOpenApi(settings =>
{
    settings.DocumentName = "v1";
    settings.Path = "/api/specification.json";
});

app.UseSwaggerUi(settings =>
{
    settings.DocumentPath = "/api/specification.json";
    settings.Path = "/api";
});

app.UseExceptionHandler(options => { });

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();

public partial class Program
{
}
