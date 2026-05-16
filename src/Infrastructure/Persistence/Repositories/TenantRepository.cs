using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _context;

    public TenantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await _context.Tenants.AddAsync(tenant, cancellationToken);
    }

    public Task AtualizarAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        _context.Tenants.Update(tenant);
        return Task.CompletedTask;
    }

    public async Task<Tenant?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tenants.FindAsync([id], cancellationToken);
    }

    public async Task<Tenant?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var slugNormalizado = slug.Trim().ToLowerInvariant();
        return await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Slug == slugNormalizado, cancellationToken);
    }

    public async Task<bool> ExisteSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var slugNormalizado = slug.Trim().ToLowerInvariant();
        return await _context.Tenants.AnyAsync(t => t.Slug == slugNormalizado, cancellationToken);
    }

    public async Task<IEnumerable<Tenant>> ListarPaginadoAsync(int pagina, int tamanhoPagina, CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .AsNoTracking()
            .OrderBy(t => t.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> ContarAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tenants.CountAsync(cancellationToken);
    }
}
