using Domain.Entities;
using Domain.Exceptions;

namespace Domain.UnitTests.Entities;

public sealed class TenantTests
{
    [Fact]
    public void CriarComDadosValidosDeveCriarTenant()
    {
        var tenant = Tenant.Criar("Oficina Teste", "oficina-teste");

        Assert.NotEqual(Guid.Empty, tenant.Id);
        Assert.Equal("Oficina Teste", tenant.Nome);
        Assert.Equal("oficina-teste", tenant.Slug);
        Assert.True(tenant.Ativo);
    }

    [Fact]
    public void CriarComSlugVazioDeveLancarArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Tenant.Criar("Oficina", " "));
    }

    [Fact]
    public void DesativarQuandoJaDesativadoDeveLancarDomainException()
    {
        var tenant = Tenant.Criar("Oficina", "oficina");
        tenant.Desativar();

        Assert.Throws<DomainException>(() => tenant.Desativar());
    }
}
