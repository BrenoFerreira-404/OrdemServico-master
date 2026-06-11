using System.Security.Claims;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Application.UnitTests.Services;

public sealed class MultiTenancyTests
{
    private readonly Mock<IClienteRepository> _clienteRepo = new();
    private readonly Mock<IOrdemServicoRepository> _osRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private static readonly Guid TenantId = Guid.NewGuid();

    [Fact]
    public void OrdemServicoCriarComTenantIdVazioDeveLancarArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            OrdemServico.Criar(Guid.Empty, Guid.NewGuid(), null, "Defeito", null, null, null, null, null));

        Assert.Contains("tenant é obrigatório", ex.Message);
    }

    [Fact]
    public void ClienteCriarComTenantIdVazioDeveLancarArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Cliente.Criar(Guid.Empty, "Nome", null, null, null, null));

        Assert.Contains("tenant é obrigatório", ex.Message);
    }

    [Fact]
    public void EquipamentoCriarComTenantIdVazioDeveLancarArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Equipamento.Criar(Guid.Empty, Guid.NewGuid(), "Tipo", null, null, null));

        Assert.Contains("tenant é obrigatório", ex.Message);
    }

    [Fact]
    public async Task OrdemServicoServiceCriarAsyncSemTenantIdDeveLancarDomainException()
    {
        var tenantProvider = new Mock<ITenantProvider>();
        tenantProvider.Setup(x => x.TenantId).Returns((Guid?)null);

        var sut = new Application.Services.OrdemServicoService(
            _osRepo.Object,
            _clienteRepo.Object,
            _uow.Object,
            tenantProvider.Object,
            NullLogger<Application.Services.OrdemServicoService>.Instance);

        var request = new Application.DTOs.OrdemServicos.CriarOrdemServicoRequest(
            Guid.NewGuid(),
            null,
            "Defeito",
            null,
            null,
            null,
            null,
            null);

        await Assert.ThrowsAsync<DomainException>(() => sut.CriarAsync(request));
    }

    [Fact]
    public async Task ClienteServiceCriarAsyncSemTenantIdDeveLancarDomainException()
    {
        var tenantProvider = new Mock<ITenantProvider>();
        tenantProvider.Setup(x => x.TenantId).Returns((Guid?)null);

        var sut = new Application.Services.ClienteService(
            _clienteRepo.Object,
            _uow.Object,
            tenantProvider.Object,
            NullLogger<Application.Services.ClienteService>.Instance);

        var request = new Application.DTOs.Clientes.CriarClienteRequest("Nome", null, null, null, null);

        await Assert.ThrowsAsync<DomainException>(() => sut.CriarAsync(request));
    }

    [Fact]
    public async Task HttpTenantProviderComSuperAdminDeveSetarEhSuperAdminTrue()
    {
        var httpContext = new DefaultHttpContext();
        var claims = new List<Claim>
        {
            new("is_super_admin", "true"),
            new("tenant_id", "")
        };
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(x => x.HttpContext).Returns(httpContext);

        var provider = new Infrastructure.Tenancy.HttpTenantProvider(accessor.Object);

        Assert.True(provider.EhSuperAdmin);
        Assert.Null(provider.TenantId);
    }

    [Fact]
    public async Task HttpTenantProviderComUsuarioComumDeveSetarTenantId()
    {
        var httpContext = new DefaultHttpContext();
        var claims = new List<Claim>
        {
            new("is_super_admin", "false"),
            new("tenant_id", TenantId.ToString()),
            new("usuario_id", Guid.NewGuid().ToString())
        };
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(x => x.HttpContext).Returns(httpContext);

        var provider = new Infrastructure.Tenancy.HttpTenantProvider(accessor.Object);

        Assert.False(provider.EhSuperAdmin);
        Assert.Equal(TenantId, provider.TenantId);
    }

    [Fact]
    public async Task HttpTenantProviderSemAutenticacaoDeveSetarValoresNull()
    {
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        var provider = new Infrastructure.Tenancy.HttpTenantProvider(accessor.Object);

        Assert.False(provider.EhSuperAdmin);
        Assert.Null(provider.TenantId);
    }
}