using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Contrato para operações de persistência de Tenant.
/// </summary>
public interface ITenantRepository
{
    Task AdicionarAsync(Tenant tenant, CancellationToken ct = default);
    Task AtualizarAsync(Tenant tenant, CancellationToken ct = default);
    Task<Tenant?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Tenant?> ObterPorSlugAsync(string slug, CancellationToken ct = default);
    Task<bool> SlugExisteAsync(string slug, CancellationToken ct = default);
    Task<IEnumerable<Tenant>> ListarTodosAsync(int pagina, int tamanhoPagina, CancellationToken ct = default);
    Task<int> ContarAsync(CancellationToken ct = default);
}
