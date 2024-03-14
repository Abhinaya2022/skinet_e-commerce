using System.Reflection;
using API.Extensions;
using API.Middlewares;
using Carter;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Host.UseSerilog(
            (context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            }
        );

        builder.Services.AddControllers();

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);
        builder.Services.AddSwaggerService();
       
        // builder.Services.AddSwaggerGen(options =>
        // {
        //     options.CustomSchemaIds(type => type.FullName);
        //     var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //     options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        // });

        var app = builder.Build();
        // Configure the HTTP request pipeline.

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSerilogRequestLogging();

        app.UseStatusCodePagesWithReExecute("/errors/{0}");

        app.UseSwaggerDocumentation();

        app.UseStaticFiles();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapCarter();
        

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<StoreContext>();
        var identityContext = services.GetRequiredService<AppIdentityDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(context);
            await identityContext.Database.MigrateAsync();
            await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured during migrations");
        }

        app.Run();
    }
}
