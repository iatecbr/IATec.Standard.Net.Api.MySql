using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Api.Configurations.Extensions;

public static class MigrationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        // if (app.Environment.EnvironmentName is "Local") return app;

        using var scope = app.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<WriteDataContext>();
        dataContext.Database.Migrate();

        return app;
    }
}