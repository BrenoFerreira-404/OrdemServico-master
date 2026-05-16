using Application.Interfaces;
using Domain.Exceptions;
using Infrastructure.Multitenancy;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Middlewares;

public sealed class TenantHeaderValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TenantHeaderValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, AppDbContext dbContext)
    {
        if (tenantContext.IsSuperAdmin
            && context.Request.Headers.TryGetValue(TenantHeaderNames.TenantId, out var valores))
        {
            var valor = valores.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(valor))
            {
                if (!Guid.TryParse(valor, out var tenantId))
                    throw new DomainException("Header X-Tenant-Id invalido.");

                var tenantExiste = await dbContext.Tenants
                    .AsNoTracking()
                    .AnyAsync(t => t.Id == tenantId && t.Ativo);

                if (!tenantExiste)
                    throw new DomainException("Tenant informado no header nao existe ou esta inativo.");
            }
        }

        await _next(context);
    }
}
