using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppIdentityUser>
{
    private readonly ITenantContext _tenantContext;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
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

    private bool IgnorarFiltroTenant => _tenantContext.IgnorarFiltroTenant;
    private Guid? TenantIdAtual => _tenantContext.TenantId;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<Cliente>().HasQueryFilter(e =>
            IgnorarFiltroTenant || e.TenantId == TenantIdAtual);

        builder.Entity<Equipamento>().HasQueryFilter(e =>
            IgnorarFiltroTenant || e.TenantId == TenantIdAtual);

        builder.Entity<OrdemServico>().HasQueryFilter(e =>
            IgnorarFiltroTenant || e.TenantId == TenantIdAtual);

        builder.Entity<Usuario>().HasQueryFilter(u =>
            IgnorarFiltroTenant || (u.TenantId != null && u.TenantId == TenantIdAtual));
    }
}
