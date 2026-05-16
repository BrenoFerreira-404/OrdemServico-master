using Application.Interfaces;
using Domain.Exceptions;

namespace Infrastructure.Multitenancy;

/// <summary>
/// Contexto usado em design-time (migrations/ferramentas EF) sem HTTP.
/// </summary>
internal sealed class DesignTimeTenantContext : ITenantContext
{
    public Guid? TenantId => null;

    public bool IsSuperAdmin => true;

    public bool IgnorarFiltroTenant => true;

    public Guid ObterTenantIdObrigatorio() =>
        throw new AcessoTenantNegadoException("Tenant indisponivel em design-time.");
}
