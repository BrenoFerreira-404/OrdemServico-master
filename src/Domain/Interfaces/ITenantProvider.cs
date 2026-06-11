namespace Domain.Interfaces;

/// <summary>
/// Abstração para resolver o TenantId e status de SuperAdmin do contexto atual.
/// Implementação: HttpTenantProvider (extrai do JWT).
/// </summary>
public interface ITenantProvider
{
    /// <summary>
    /// ID do tenant atual. NULL para SuperAdmin.
    /// </summary>
    Guid? TenantId { get; }

    /// <summary>
    /// True se o usuário atual é SuperAdmin (ignora query filters, ve dados globais).
    /// </summary>
    bool EhSuperAdmin { get; }
}
