using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminUI.Services.Contracts;
using System.Net.Http.Json;

namespace Contoso.Pizza.AdminUI.Services;

public class SauceService : ISauceService
{
    private readonly HttpClient _httpClient;
    private const string _baseUri = "/api/sauces";

    public SauceService(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("AdminApi");
    }

    public async Task<SauceEntity> AddSauceAsync(SauceEntity entity)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUri, entity);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<SauceEntity>())!;
    }

    public async Task DeleteSauceAsync(SauceEntity entity)
    {
        var deleteUri = $"{_baseUri}/{entity.Id}";
        var response = await _httpClient.DeleteAsync(deleteUri);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<SauceEntity>> GetAllSaucesAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<SauceEntity>>(_baseUri) ?? Array.Empty<SauceEntity>();
    }

    public async Task UpdateSauceAsync(SauceEntity entity)
    {
        var updateUri = $"{_baseUri}/{entity.Id}";
        var response = await _httpClient.PutAsJsonAsync(updateUri, entity);
        response.EnsureSuccessStatusCode();
    }
}
