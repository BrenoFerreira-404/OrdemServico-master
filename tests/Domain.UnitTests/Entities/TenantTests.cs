using Domain.Entities;
using Domain.Enums;

namespace Domain.UnitTests.Entities;

public sealed class TenantTests
{
    private const string NomeValido = "Assistência Tech";
    private const string EmailValido = "contato@assistenciatech.com";

    [Fact]
    public void CriarComDadosValidosDeveCriarTenant()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);

        Assert.NotEqual(Guid.Empty, tenant.Id);
        Assert.Equal(NomeValido, tenant.Nome);
        Assert.Equal("assistencia-tech", tenant.Slug);
        Assert.Equal(EmailValido, tenant.Email);
        Assert.True(tenant.Ativo);
        Assert.Equal(PlanoTenant.Free, tenant.PlanoAtual);
    }

    [Fact]
    public void CriarComNomeVazioDeveLancarArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Tenant.Criar("", EmailValido));
    }

    [Fact]
    public void CriarComEmailVazioDeveLancarArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Tenant.Criar(NomeValido, ""));
    }

    [Fact]
    public void SuspenderDeveSetarAtivoFalseEMotivo()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);
        tenant.Suspender("Pagamento não efetuado");

        Assert.False(tenant.Ativo);
        Assert.Equal("Pagamento não efetuado", tenant.MotivoDesativacao);
    }

    [Fact]
    public void SuspenderComMotivoVazioDeveLancarArgumentException()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);

        Assert.Throws<ArgumentException>(() => tenant.Suspender(""));
    }

    [Fact]
    public void ReativarDeveSetarAtivoTrueELimparMotivo()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);
        tenant.Suspender("Pagamento não efetuado");
        tenant.Reativar();

        Assert.True(tenant.Ativo);
        Assert.Null(tenant.MotivoDesativacao);
    }

    [Fact]
    public void AlterarPlanoDeveAtualizarPlano()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);
        tenant.AlterarPlano(PlanoTenant.Pro);

        Assert.Equal(PlanoTenant.Pro, tenant.PlanoAtual);
    }

    [Fact]
    public void EstaExpiradoComDataExpiracaoNoPassadoDeveRetornarTrue()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);
        var passado = DateTime.UtcNow.AddDays(-1);
        tenant.RenovarAte(passado);

        Assert.True(tenant.EstaExpirado());
    }

    [Fact]
    public void EstaExpiradoSemDataExpiracaoDeveRetornarFalse()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);

        Assert.False(tenant.EstaExpirado());
    }

    [Fact]
    public void PodeSerUsadoComTenantAtivoNaoExpiradoDeveRetornarTrue()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);

        Assert.True(tenant.PodeSerUsado());
    }

    [Fact]
    public void PodeSerUsadoComTenantInativoDeveRetornarFalse()
    {
        var tenant = Tenant.Criar(NomeValido, EmailValido);
        tenant.Suspender("Motivo");

        Assert.False(tenant.PodeSerUsado());
    }
}