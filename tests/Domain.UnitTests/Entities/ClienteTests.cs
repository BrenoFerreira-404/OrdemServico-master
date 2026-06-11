using Domain.Entities;

namespace Domain.UnitTests.Entities;

public class ClienteTests
{
    [Fact]
    public void CriarComNomeValidoDeveCriarCliente()
    {
        var tenantId = Guid.NewGuid();
        var cliente = Cliente.Criar(tenantId, "Cliente Teste", "123", "11999990000", "cliente@teste.com", "Rua A");

        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal("Cliente Teste", cliente.Nome);
        Assert.Equal(tenantId, cliente.TenantId);
    }

    [Fact]
    public void CriarComNomeVazioDeveLancarArgumentException()
    {
        var tenantId = Guid.NewGuid();
        Assert.Throws<ArgumentException>(() => Cliente.Criar(tenantId, " ", null, null, null, null));
    }

    [Fact]
    public void AtualizarDeveAtualizarDados()
    {
        var tenantId = Guid.NewGuid();
        var cliente = Cliente.Criar(tenantId, "Cliente Teste", "123", "11999990000", "cliente@teste.com", "Rua A");

        cliente.Atualizar("Cliente Novo", "999", "11888880000", "novo@teste.com", "Rua B");

        Assert.Equal("Cliente Novo", cliente.Nome);
        Assert.Equal("999", cliente.Documento);
    }
}
