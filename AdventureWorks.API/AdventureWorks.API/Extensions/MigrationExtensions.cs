using AdventureWorks.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            dbContext.Database.Migrate();
        }
        catch (InvalidOperationException ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Migration");
            logger.LogError(ex, "EF Core migration failed. Pending model changes may exist.");

            // Provide a clearer runtime message and rethrow to avoid silent failure.
            throw new InvalidOperationException(
                "Pending EF Core model changes detected. Add a migration (dotnet ef migrations add <Name> --project ./AdventureWorks.Infrastructure/AdventureWorks.Infrastructure.csproj --startup-project ../AdventureWorks.API/AdventureWorks.API.csproj) and apply it, or remove/adjust existing migrations to match the model.",
                ex);
        }
    }
}
