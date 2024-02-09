using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminUI.Services.Contracts;
using System.Net.Http.Json;

namespace Contoso.Pizza.AdminUI.Services;

public class PizzaService : IPizzaService
{
    private readonly HttpClient _httpClient;
    private const string _baseUri = "/api/pizzas";

    public PizzaService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AdminApi");
    }

    public async Task<PizzaEntity> AddPizzaAsync(PizzaEntity entity)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUri, entity);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PizzaEntity>())!;
    }

    public async Task DeletePizzaAsync(PizzaEntity entity)
    {
        var deleteUri = $"{_baseUri}/{entity.Id}";
        var response = await _httpClient.DeleteAsync(deleteUri);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<PizzaEntity>> GetAllPizzasAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<PizzaEntity>>(_baseUri) ?? Array.Empty<PizzaEntity>();
    }

    public async Task UpdatePizzaAsync(PizzaEntity entity)
    {
        var updateUri = $"{_baseUri}/{entity.Id}";
        var response = await _httpClient.PutAsJsonAsync(updateUri, entity);
        response.EnsureSuccessStatusCode();
    }
}