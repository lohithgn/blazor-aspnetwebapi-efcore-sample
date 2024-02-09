using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminUI.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Contoso.Pizza.AdminUI.Components.Pages.Pizza;

public partial class PizzasPage
{
    [Inject]
    public required IPizzaService Service { get; set; }

    private IQueryable<PizzaEntity>? _pizzas;

    private IDialogReference? _dialog;

    protected override async Task OnInitializedAsync()
    {
        await LoadPizzas();
    }

    private async Task LoadPizzas()
    {
        _pizzas = (await Service.GetAllPizzasAsync()).AsQueryable();
    }

    private async Task OnAddNewPizzaClick()
    {
        var panelTitle = $"Add a pizza";
        var result = await ShowPanel(panelTitle, new PizzaEntity() { Name = "New Pizza" });
        if (result.Cancelled)
        {
            return;
        }
        var entity = result.Data as PizzaEntity;
        ShowProgressToast(nameof(OnAddNewPizzaClick), "Pizza", entity!.Name);
        _ = await Service.AddPizzaAsync(entity!);
        CloseProgressToast(nameof(OnAddNewPizzaClick));
        ShowSuccessToast("Pizza", entity!.Name);
        await LoadPizzas();
    }

    private async Task OnEditPizzaClick(PizzaEntity pizza)
    {
        var panelTitle = $"Edit pizza";
        var result = await ShowPanel(panelTitle, pizza, false);
        if (result.Cancelled)
        {
            return;
        }
        var entity = result.Data as PizzaEntity;
        ShowProgressToast(nameof(OnEditPizzaClick), "Pizza", entity!.Name, Operation.Update);
        await Service.UpdatePizzaAsync(entity!);
        CloseProgressToast(nameof(OnEditPizzaClick));
        ShowSuccessToast("Pizza", entity!.Name, Operation.Update);
        await LoadPizzas();
    }

    private async Task OnDeletePizzaClick(PizzaEntity entity)
    {
        var confirm = await ShowConfirmationDialogAsync("Delete Pizza", $"Are you sure you want to delete {entity.Name}?");
        if (confirm.Cancelled)
        {
            return;
        }
        ShowProgressToast(nameof(OnDeletePizzaClick), "Pizza", entity.Name, Operation.Delete);
        await Service.DeletePizzaAsync(entity);
        CloseProgressToast(nameof(OnDeletePizzaClick));
        ShowSuccessToast("Pizza", entity.Name, Operation.Delete);
        await LoadPizzas();
    }

    private async Task<DialogResult> ShowPanel(string title, PizzaEntity pizza, bool isAdd = true)
    {
        var primaryActionText = isAdd ? "Add" : "Save changes";
        var dialogParameter = new DialogParameters<PizzaEntity>()
        {
            Content = pizza,
            Alignment = HorizontalAlignment.Right,
            Title = title,
            PrimaryAction = primaryActionText,
            Width = "500px",
            PreventDismissOnOverlayClick = true,
        };
        _dialog = await DialogService.ShowPanelAsync<PizzaUpsertPanel>(pizza, dialogParameter);
        return await _dialog.Result;
    }
}
