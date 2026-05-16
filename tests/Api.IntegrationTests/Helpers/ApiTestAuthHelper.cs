using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.DTOs.Auth;
using Domain.Constants;
using Web.Services.Auth;

namespace Api.IntegrationTests.Helpers;

public static class ApiTestAuthHelper
{
    public static async Task<HttpClient> CriarClienteAutenticadoAsync(HttpClient client, CancellationToken cancellationToken = default)
    {
        var (authenticatedClient, _) = await CriarClienteAutenticadoComTokenStorageAsync(client, cancellationToken);
        return authenticatedClient;
    }

    public static async Task<(HttpClient Client, TokenStorage TokenStorage)> CriarClienteAutenticadoComTokenStorageAsync(
        HttpClient client,
        CancellationToken cancellationToken = default)
    {
        var loginRequest = new LoginRequest(TenantSeedConstants.AdminDemoEmail, TenantSeedConstants.AdminDemoSenha);
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest, cancellationToken);

        loginResponse.EnsureSuccessStatusCode();

        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Resposta de login invalida.");

        var tokenStorage = new TokenStorage();
        await tokenStorage.SalvarTokensAsync(login.AccessToken, login.RefreshToken);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.AccessToken);
        return (client, tokenStorage);
    }

    public static async Task<HttpClient> CriarClienteSuperAdminAsync(
        HttpClient client,
        CancellationToken cancellationToken = default)
    {
        var loginRequest = new LoginRequest(TenantSeedConstants.SuperAdminEmail, TenantSeedConstants.SuperAdminSenha);
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest, cancellationToken);
        loginResponse.EnsureSuccessStatusCode();

        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Resposta de login invalida.");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.AccessToken);
        return client;
    }

    public static void DefinirTenantHeader(HttpClient client, Guid tenantId)
    {
        client.DefaultRequestHeaders.Remove("X-Tenant-Id");
        client.DefaultRequestHeaders.Add("X-Tenant-Id", tenantId.ToString());
    }
}
