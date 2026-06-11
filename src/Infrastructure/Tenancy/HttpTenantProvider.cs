using System.Security.Claims;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Tenancy;

/// <summary>
/// Implementação de ITenantProvider que extrai TenantId e EhSuperAdmin do contexto HTTP (claims JWT).
/// </summary>
public sealed class HttpTenantProvider : ITenantProvider
{
    public Guid? TenantId { get; }
    public bool EhSuperAdmin { get; }

    public HttpTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            TenantId = null;
            EhSuperAdmin = false;
            return;
        }

        // Extrair claim is_super_admin do JWT
        var superAdminClaim = user.FindFirst("is_super_admin");
        EhSuperAdmin = superAdminClaim is not null && bool.TryParse(superAdminClaim.Value, out var isSuperAdmin) && isSuperAdmin;

        // Extrair claim tenant_id do JWT
        var tenantClaim = user.FindFirst("tenant_id");
        if (tenantClaim is not null && Guid.TryParse(tenantClaim.Value, out var tid) && tid != Guid.Empty)
        {
            TenantId = tid;
        }
        else
        {
            TenantId = null;
        }
    }
}
