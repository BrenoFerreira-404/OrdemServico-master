using Application.DTOs.Comuns;
using Application.DTOs.Tenants;

namespace Application.Interfaces;

public interface ITenantService
{
    Task<TenantResponse> CriarAsync(CriarTenantRequest request, CancellationToken cancellationToken = default);
    Task<TenantResponse?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResponse<TenantResponse>> ListarPaginadoAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Guid id, AtualizarTenantRequest request, CancellationToken cancellationToken = default);
    Task DesativarAsync(Guid id, CancellationToken cancellationToken = default);
    Task ReativarAsync(Guid id, CancellationToken cancellationToken = default);
}
