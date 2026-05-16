using Domain.Entities;

namespace Domain.Interfaces;

public interface ITenantRepository
{
    Task AdicionarAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task<Tenant?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tenant?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> ExisteSlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tenant>> ListarPaginadoAsync(int pagina, int tamanhoPagina, CancellationToken cancellationToken = default);
    Task<int> ContarAsync(CancellationToken cancellationToken = default);
}
