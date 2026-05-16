using Api.Filters;
using Application.DTOs.Comuns;
using Application.DTOs.Tenants;
using Application.Interfaces;

namespace Api.Endpoints;

public static class TenantEndpoints
{
    public static void MapTenantEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/tenants")
            .WithTags("Tenants")
            .RequireAuthorization("SuperAdmin");

        group.MapPost("/", async (CriarTenantRequest request, ITenantService service, CancellationToken ct) =>
        {
            var response = await service.CriarAsync(request, ct);
            return Results.Created($"/api/tenants/{response.Id}", response);
        })
        .AddEndpointFilter<ValidationFilter<CriarTenantRequest>>()
        .WithName("CriarTenant")
        .Produces<TenantResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        group.MapGet("/{id:guid}", async (Guid id, ITenantService service, CancellationToken ct) =>
        {
            var response = await service.ObterPorIdAsync(id, ct);
            return response is not null ? Results.Ok(response) : Results.NotFound();
        })
        .WithName("ObterTenant")
        .Produces<TenantResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/", async ([AsParameters] PagedRequest request, ITenantService service, CancellationToken ct) =>
        {
            var response = await service.ListarPaginadoAsync(request, ct);
            return Results.Ok(response);
        })
        .WithName("ListarTenants")
        .Produces<PagedResponse<TenantResponse>>(StatusCodes.Status200OK);

        group.MapPut("/{id:guid}", async (Guid id, AtualizarTenantRequest request, ITenantService service, CancellationToken ct) =>
        {
            await service.AtualizarAsync(id, request, ct);
            return Results.NoContent();
        })
        .AddEndpointFilter<ValidationFilter<AtualizarTenantRequest>>()
        .WithName("AtualizarTenant")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        group.MapPatch("/{id:guid}/desativar", async (Guid id, ITenantService service, CancellationToken ct) =>
        {
            await service.DesativarAsync(id, ct);
            return Results.NoContent();
        })
        .WithName("DesativarTenant")
        .Produces(StatusCodes.Status204NoContent);

        group.MapPatch("/{id:guid}/reativar", async (Guid id, ITenantService service, CancellationToken ct) =>
        {
            await service.ReativarAsync(id, ct);
            return Results.NoContent();
        })
        .WithName("ReativarTenant")
        .Produces(StatusCodes.Status204NoContent);
    }
}
