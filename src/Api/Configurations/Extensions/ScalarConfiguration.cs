using Scalar.AspNetCore;

namespace Api.Configurations.Extensions;

public static class ScalarConfiguration
{
    public static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new()
                {
                    Title = "{API_NAME}",
                    Version = "v1",
                    Description = "API documentation"
                };
                return Task.CompletedTask;
            });
        });

        return services;
    }

    public static void ConfigureOpenApi(this IEndpointRouteBuilder app, IWebHostEnvironment environment)
    {
        app.MapOpenApi()
            .AllowAnonymous();

        app.MapScalarApiReference("/documentation", options =>
        {
            options
                .WithTitle($"{{API_NAME}} - {environment.EnvironmentName}")
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .ForceDarkMode()
                .ExpandAllTags()
                .SortTagsAlphabetically();
        }).AllowAnonymous();
    }
}