namespace Application.Interfaces;

/// <summary>
/// Contexto do tenant da requisicao atual (JWT / usuario autenticado).
/// </summary>
public interface ITenantContext
{
    Guid? TenantId { get; }

    bool IsSuperAdmin { get; }

    /// <summary>
    /// Quando true, consultas EF nao aplicam filtro por tenant (SuperAdmin ou operacao de sistema).
    /// </summary>
    bool IgnorarFiltroTenant { get; }

    Guid ObterTenantIdObrigatorio();
}
