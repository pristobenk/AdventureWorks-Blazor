namespace AdventureWorks.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        // Serve the generated OpenAPI document and the Swagger UI
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            // Ensure the Swagger JSON endpoint is registered so the UI can load it.
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });

        // Redirect root ("/") to the Swagger UI so the first page opened is the UI.
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/" || string.IsNullOrEmpty(context.Request.Path))
            {
                context.Response.Redirect("/swagger/index.html");
                return;
            }

            await next();
        });

        return app;
    }
}
