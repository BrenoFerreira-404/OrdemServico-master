using Application.Interfaces;
using Moq;

namespace Application.UnitTests.Helpers;

internal static class TenantContextMockFactory
{
    public static Mock<ITenantContext> Criar(Guid? tenantId = null, bool isSuperAdmin = false)
    {
        var id = tenantId ?? Guid.NewGuid();
        var mock = new Mock<ITenantContext>();
        mock.Setup(x => x.TenantId).Returns(isSuperAdmin ? null : id);
        mock.Setup(x => x.IsSuperAdmin).Returns(isSuperAdmin);
        mock.Setup(x => x.IgnorarFiltroTenant).Returns(isSuperAdmin);
        mock.Setup(x => x.ObterTenantIdObrigatorio()).Returns(id);
        return mock;
    }
}
