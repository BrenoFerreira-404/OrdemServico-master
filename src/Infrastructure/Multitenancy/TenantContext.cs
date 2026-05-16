using System.Security.Claims;
using Application.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Multitenancy;

public sealed class TenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId => ResolverTenantIdEfetivo();

    public bool IsSuperAdmin =>
        string.Equals(ObterCargoDoUsuario(), "SuperAdmin", StringComparison.OrdinalIgnoreCase);

    public bool IgnorarFiltroTenant
    {
        get
        {
            if (_httpContextAccessor.HttpContext is null)
                return true;

            if (IsSuperAdmin && !PossuiTenantExplicito())
                return true;

            return false;
        }
    }

    public Guid ObterTenantIdObrigatorio()
    {
        var tenantId = TenantId;
        if (!tenantId.HasValue || tenantId.Value == Guid.Empty)
        {
            if (IsSuperAdmin)
                throw new AcessoTenantNegadoException(
                    "Informe o header X-Tenant-Id para operar em um tenant especifico.");

            throw new AcessoTenantNegadoException("Tenant nao identificado na requisicao.");
        }

        return tenantId.Value;
    }

    private bool PossuiTenantExplicito()
    {
        if (ObterTenantIdDoHeader().HasValue)
            return true;

        var claim = ObterTenantIdDoClaim();
        return claim.HasValue && claim.Value != Guid.Empty;
    }

    private Guid? ResolverTenantIdEfetivo()
    {
        var headerTenant = ObterTenantIdDoHeader();
        if (headerTenant.HasValue)
            return headerTenant;

        return ObterTenantIdDoClaim();
    }

    private Guid? ObterTenantIdDoHeader()
    {
        if (!IsSuperAdmin)
            return null;

        var http = _httpContextAccessor.HttpContext;
        if (http is null)
            return null;

        if (!http.Request.Headers.TryGetValue(TenantHeaderNames.TenantId, out var valores))
            return null;

        var valor = valores.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(valor))
            return null;

        return Guid.TryParse(valor, out var tenantId) ? tenantId : null;
    }

    private Guid? ObterTenantIdDoClaim()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst("tenant_id")?.Value;
        if (string.IsNullOrWhiteSpace(claim))
            return null;

        return Guid.TryParse(claim, out var tenantId) ? tenantId : null;
    }

    private string? ObterCargoDoUsuario()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user is null)
            return null;

        return user.FindFirst("role")?.Value ?? user.FindFirst(ClaimTypes.Role)?.Value;
    }
}
