using Application.DTOs.Tenants;
using Application.Interfaces;
using Application.Services;
using Application.UnitTests.Helpers;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.Services;

public sealed class TenantServiceTests
{
    private readonly Mock<ITenantRepository> _tenantRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ITenantContext> _tenantContext = TenantContextMockFactory.Criar(isSuperAdmin: true);

    private TenantService CriarSut() =>
        new(_tenantRepository.Object, _unitOfWork.Object, _tenantContext.Object);

    [Fact]
    public async Task CriarAsyncQuandoSlugJaExisteDeveLancarDomainException()
    {
        _tenantRepository.Setup(x => x.ExisteSlugAsync("demo", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = CriarSut();
        var request = new CriarTenantRequest("Oficina", "demo");

        await Assert.ThrowsAsync<DomainException>(() => sut.CriarAsync(request));
    }

    [Fact]
    public async Task CriarAsyncQuandoNaoSuperAdminDeveLancarAcessoTenantNegadoException()
    {
        var tenantContext = TenantContextMockFactory.Criar(isSuperAdmin: false);
        var sut = new TenantService(_tenantRepository.Object, _unitOfWork.Object, tenantContext.Object);

        await Assert.ThrowsAsync<AcessoTenantNegadoException>(() =>
            sut.CriarAsync(new CriarTenantRequest("Oficina", "nova-oficina")));
    }

    [Fact]
    public async Task CriarAsyncComDadosValidosDevePersistir()
    {
        _tenantRepository.Setup(x => x.ExisteSlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _unitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var sut = CriarSut();
        var response = await sut.CriarAsync(new CriarTenantRequest("Oficina Nova", "oficina-nova"));

        Assert.Equal("oficina-nova", response.Slug);
        _tenantRepository.Verify(x => x.AdicionarAsync(It.IsAny<Tenant>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
