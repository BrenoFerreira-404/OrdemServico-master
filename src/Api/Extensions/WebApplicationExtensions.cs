using Api.Endpoints;
using Api.Middlewares;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiSetup(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (app.Environment.IsEnvironment("Testing"))
                dbContext.Database.EnsureCreated();
            else
                dbContext.Database.Migrate();

            // Seed de dados iniciais (so insere se o banco estiver vazio)
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            seeder.SeedAsync().GetAwaiter().GetResult();
        }

        app.UseExceptionHandler(); // Usará o IExceptionHandler global via DI

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Custom Middlewares
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseCors("ApiCorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<TenantHeaderValidationMiddleware>();

        app.UseHttpsRedirection();

        // Endpoints genéricos (minimal APIs root extension methods)
        app.MapAuthEndpoints();
        app.MapTenantEndpoints();
        app.MapClienteEndpoints();
        app.MapEquipamentoEndpoints();
        app.MapOrdemServicoEndpoints();

        // Endpoint de seed para desenvolvimento
        if (app.Environment.IsDevelopment())
        {
            app.MapPost("/api/seed", async (DatabaseSeeder seeder, CancellationToken ct) =>
            {
                await seeder.ForceSeedAsync(ct);
                return Results.Ok(new { message = "Seed executado com sucesso." });
            }).WithTags("Dev").ExcludeFromDescription();
        }

        return app;
    }
}
