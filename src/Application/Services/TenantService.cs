using Application.DTOs.Comuns;
using Application.DTOs.Tenants;
using Application.DTOs.Tenants.Mappings;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public sealed class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public TenantService(
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<TenantResponse> CriarAsync(CriarTenantRequest request, CancellationToken cancellationToken = default)
    {
        GarantirSuperAdmin();

        if (await _tenantRepository.ExisteSlugAsync(request.Slug, cancellationToken))
            throw new DomainException("Ja existe um tenant com este slug.");

        var tenant = Tenant.Criar(request.Nome, request.Slug);
        await _tenantRepository.AdicionarAsync(tenant, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return tenant.ToResponse();
    }

    public async Task<TenantResponse?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        GarantirSuperAdmin();

        var tenant = await _tenantRepository.ObterPorIdAsync(id, cancellationToken);
        return tenant?.ToResponse();
    }

    public async Task<PagedResponse<TenantResponse>> ListarPaginadoAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        GarantirSuperAdmin();

        var total = await _tenantRepository.ContarAsync(cancellationToken);
        var itens = await _tenantRepository.ListarPaginadoAsync(request.Page, request.PageSize, cancellationToken);

        return new PagedResponse<TenantResponse>(
            itens.Select(t => t.ToResponse()),
            total,
            request.Page,
            request.PageSize);
    }

    public async Task AtualizarAsync(Guid id, AtualizarTenantRequest request, CancellationToken cancellationToken = default)
    {
        GarantirSuperAdmin();

        var tenant = await ObterTenantOuThrow(id, cancellationToken);
        tenant.Atualizar(request.Nome);
        await _tenantRepository.AtualizarAsync(tenant, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task DesativarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        GarantirSuperAdmin();

        var tenant = await ObterTenantOuThrow(id, cancellationToken);
        tenant.Desativar();
        await _tenantRepository.AtualizarAsync(tenant, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task ReativarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        GarantirSuperAdmin();

        var tenant = await ObterTenantOuThrow(id, cancellationToken);
        tenant.Reativar();
        await _tenantRepository.AtualizarAsync(tenant, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private void GarantirSuperAdmin()
    {
        if (!_tenantContext.IsSuperAdmin)
            throw new AcessoTenantNegadoException("Apenas SuperAdmin pode gerenciar tenants.");
    }

    private async Task<Tenant> ObterTenantOuThrow(Guid id, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.ObterPorIdAsync(id, cancellationToken);
        if (tenant is null)
            throw new DomainException("Tenant nao encontrado.");

        return tenant;
    }
}
