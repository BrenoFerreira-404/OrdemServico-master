using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Fixtures;
using Api.IntegrationTests.Helpers;
using Application.DTOs.Tenants;
using Domain.Constants;

namespace Api.IntegrationTests.Endpoints;

[Collection(ApiTestSuite.Name)]
public sealed class TenantEndpointsTests
{
    private readonly WebApplicationFixture _fixture;

    public TenantEndpointsTests(WebApplicationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SuperAdminDeveCriarListarEAtualizarTenant()
    {
        await _fixture.ResetDatabaseAsync();
        using var client = await ApiTestAuthHelper.CriarClienteSuperAdminAsync(_fixture.CreateApiClient());

        var criarRequest = new CriarTenantRequest("Oficina Alpha", "oficina-alpha");
        var postResponse = await client.PostAsJsonAsync("/api/tenants/", criarRequest);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var tenant = await postResponse.Content.ReadFromJsonAsync<TenantResponse>();
        Assert.NotNull(tenant);
        Assert.Equal("oficina-alpha", tenant!.Slug);

        var listResponse = await client.GetAsync("/api/tenants?page=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var putResponse = await client.PutAsJsonAsync(
            $"/api/tenants/{tenant.Id}",
            new AtualizarTenantRequest("Oficina Alpha Atualizada"));
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
    }

    [Fact]
    public async Task SuperAdminComHeaderTenantDeveIsolarDadosDoTenant()
    {
        await _fixture.ResetDatabaseAsync();
        using var client = await ApiTestAuthHelper.CriarClienteSuperAdminAsync(_fixture.CreateApiClient());
        ApiTestAuthHelper.DefinirTenantHeader(client, TenantSeedConstants.DemonstracaoId);

        var buscaResponse = await client.GetAsync("/api/clientes/busca?nome=Maria");
        Assert.Equal(HttpStatusCode.OK, buscaResponse.StatusCode);

        var clientes = await buscaResponse.Content.ReadFromJsonAsync<IEnumerable<Application.DTOs.Clientes.ClienteResponse>>();
        Assert.NotNull(clientes);
        Assert.NotEmpty(clientes!);
    }

    [Fact]
    public async Task AdminTenantNaoDeveAcessarEndpointsDeTenants()
    {
        await _fixture.ResetDatabaseAsync();
        using var client = await ApiTestAuthHelper.CriarClienteAutenticadoAsync(_fixture.CreateApiClient());

        var response = await client.GetAsync("/api/tenants?page=1&pageSize=10");
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
