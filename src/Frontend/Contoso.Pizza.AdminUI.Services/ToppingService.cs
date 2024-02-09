using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminUI.Services.Contracts;
using System.Net.Http.Json;

namespace Contoso.Pizza.AdminUI.Services;

public class ToppingService : IToppingService
{
    private readonly HttpClient _httpClient;
    private const string _baseUri = "/api/toppings";

    public ToppingService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AdminApi");
    }

    public async Task<ToppingEntity> AddToppingAsync(ToppingEntity entity)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUri, entity);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ToppingEntity>())!;
    }

    public async Task DeleteToppingAsync(ToppingEntity entity)
    {
        var deleteUri = $"{_baseUri}/{entity.Id}";
        var response = await _httpClient.DeleteAsync(deleteUri);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<ToppingEntity>> GetAllToppingsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ToppingEntity>>(_baseUri) ?? Array.Empty<ToppingEntity>();
    }

    public async Task UpdateToppingAsync(ToppingEntity entity)
    {
        var updateUri = $"{_baseUri}/{entity.Id}";
        var response = await _httpClient.PutAsJsonAsync(updateUri, entity);
        response.EnsureSuccessStatusCode();
    }
}