using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementação do repositório de Tenant.
/// Sem query filter de tenancy, pois SuperAdmin acessa todos os tenants.
/// </summary>
public sealed class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _context;

    public TenantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Tenant tenant, CancellationToken ct = default)
    {
        await _context.Tenants.AddAsync(tenant, ct);
    }

    public async Task AtualizarAsync(Tenant tenant, CancellationToken ct = default)
    {
        _context.Tenants.Update(tenant);
        await Task.CompletedTask;
    }

    public async Task<Tenant?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<Tenant?> ObterPorSlugAsync(string slug, CancellationToken ct = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Slug == slug, ct);
    }

    public async Task<bool> SlugExisteAsync(string slug, CancellationToken ct = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .AnyAsync(t => t.Slug == slug, ct);
    }

    public async Task<IEnumerable<Tenant>> ListarTodosAsync(int pagina, int tamanhoPagina, CancellationToken ct = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(ct);
    }

    public async Task<int> ContarAsync(CancellationToken ct = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .CountAsync(ct);
    }
}
