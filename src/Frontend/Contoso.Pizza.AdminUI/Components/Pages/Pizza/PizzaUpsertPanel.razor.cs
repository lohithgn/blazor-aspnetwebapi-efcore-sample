using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminUI.Services;
using Microsoft.AspNetCore.Components;

namespace Contoso.Pizza.AdminUI.Components.Pages.Pizza;

public partial class PizzaUpsertPanel
{
    [Inject]
    SauceService SauceService { get; set; } = default!;
    
    [Inject]
    ToppingService ToppingService { get; set; } = default!;
    
    [Parameter]
    public PizzaEntity Content { get; set; } = default!;

    IEnumerable<SauceEntity>? _sauces;
    List<ToppingEntity>? _toppings = [];
    bool isBusy;

    protected override async Task OnInitializedAsync()
    {
        isBusy = true;        
        _sauces = await SauceService.GetAllSaucesAsync();
        Content.Sauce = _sauces.FirstOrDefault();
        _toppings = (await ToppingService.GetAllToppingsAsync()).ToList();
        isBusy = false;
    }

    protected void OnToppingSelected(ToppingEntity item, bool selected)
    {
        if(selected)
        {
            Content.Toppings!.Add(item);
        }
        else
        {
            Content.Toppings!.Remove(item);
        }
    }
}