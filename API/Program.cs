using API.Extensions;
using API.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddSwaggerService();


        var app = builder.Build();
        // Configure the HTTP request pipeline.

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseStatusCodePagesWithReExecute("/errors/{0}");

        app.UseSwaggerDocumentation();

        app.UseStaticFiles();

        app.UseCors("CorsPolicy");

        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<StoreContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured during migrations");
        }

        app.Run();
    }
}