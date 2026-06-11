using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppIdentityUser>
{
    private readonly ITenantProvider _tenantProvider;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider) : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Equipamento> Equipamentos => Set<Equipamento>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
    public DbSet<OrdemServicoServico> OrdensServicoServicos => Set<OrdemServicoServico>();
    public DbSet<OrdemServicoProduto> OrdensServicoProdutos => Set<OrdemServicoProduto>();
    public DbSet<OrdemServicoTaxa> OrdensServicoTaxas => Set<OrdemServicoTaxa>();
    public DbSet<OrdemServicoPagamento> OrdensServicoPagamentos => Set<OrdemServicoPagamento>();
    public DbSet<OrdemServicoFoto> OrdensServicoFotos => Set<OrdemServicoFoto>();
    public DbSet<OrdemServicoAnotacao> OrdensServicoAnotacoes => Set<OrdemServicoAnotacao>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Identity tables DEVEM ser configuradas PRIMEIRO
        base.OnModelCreating(builder);

        // Depois aplica as configurations customizadas (entidades de dominio)
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Query filters para multi-tenancy: somente se nao for SuperAdmin
        // SuperAdmin precisa ver dados de todos os tenants
        if (!_tenantProvider.EhSuperAdmin && _tenantProvider.TenantId.HasValue)
        {
            var tenantId = _tenantProvider.TenantId.Value;
            builder.Entity<Cliente>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<Equipamento>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<OrdemServico>().HasQueryFilter(x => x.TenantId == tenantId);
            builder.Entity<Usuario>().HasQueryFilter(x => x.TenantId == tenantId);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Defense in depth: garantir TenantId nas insercoes
        foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added))
        {
            var tenantIdProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "TenantId");
            if (tenantIdProp is not null
                && tenantIdProp.CurrentValue is Guid id
                && id == Guid.Empty
                && _tenantProvider.TenantId.HasValue)
            {
                tenantIdProp.CurrentValue = _tenantProvider.TenantId.Value;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
