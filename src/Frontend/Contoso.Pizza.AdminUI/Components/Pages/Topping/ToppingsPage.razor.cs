using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminUI.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Contoso.Pizza.AdminUI.Components.Pages.Topping;

public partial class ToppingsPage
{
    [Inject]
    public required IToppingService Service { get; set; }

    private IQueryable<ToppingEntity>? _toppings;

    private IDialogReference? _dialog;

    protected override async Task OnInitializedAsync()
    {
        await LoadToppings();
    }

    private async Task LoadToppings()
    {
        _toppings = (await Service.GetAllToppingsAsync()).AsQueryable();
    }

    private async Task OnAddNewToppingClick()
    {
        var panelTitle = $"Add a topping";
        var result = await ShowPanel(panelTitle, new ToppingEntity() { Name = "New Topping" });
        if (result.Cancelled)
        {
            return;
        }
        var entity = result.Data as ToppingEntity;
        ShowProgressToast(nameof(OnAddNewToppingClick), "Topping", entity!.Name);
        _ = await Service.AddToppingAsync(entity!);
        CloseProgressToast(nameof(OnAddNewToppingClick));
        ShowSuccessToast("Topping", entity!.Name);
        await LoadToppings();
    }

    private async Task OnEditToppingClick(ToppingEntity topping)
    {
        var panelTitle = $"Edit topping";
        var result = await ShowPanel(panelTitle, topping, false);
        if (result.Cancelled)
        {
            return;
        }
        var entity = result.Data as ToppingEntity;
        ShowProgressToast(nameof(OnEditToppingClick), "Topping", entity!.Name, Operation.Update);
        await Service.UpdateToppingAsync(entity!);
        CloseProgressToast(nameof(OnEditToppingClick));
        ShowSuccessToast("Topping", entity!.Name, Operation.Update);
        await LoadToppings();
    }

    private async Task OnDeleteToppingClick(ToppingEntity entity)
    {
        var title = "Delete topping?";
        var message = $"Are you sure you want to delete topping: <b>{entity.Name}</b>?";
        var result = await ShowConfirmationDialogAsync(title, message);
        if (result.Cancelled)
        {
            return;
        }
        try
        {
            ShowProgressToast(nameof(OnDeleteToppingClick), "Topping", entity!.Name, Operation.Delete);
            await Service.DeleteToppingAsync(entity);
            CloseProgressToast(nameof(OnDeleteToppingClick));
            ShowSuccessToast("Topping", entity.Name, Operation.Delete);
            await LoadToppings();
        }
        catch (HttpRequestException ex)
        {
            CloseProgressToast(nameof(OnDeleteToppingClick));
            if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ShowFailureToast("Topping", entity.Name, Operation.Delete, "The topping is in use and cannot be deleted.");
            }
            else
            {
                ShowFailureToast("Topping", entity.Name, Operation.Delete, ex.Message);
            }
        }
    }

    private async Task<DialogResult> ShowPanel(string title, ToppingEntity topping, bool isAdd = true)
    {
        var primaryActionText = isAdd ? "Add" : "Save changes";
        var dialogParameter = new DialogParameters<ToppingEntity>()
        {
            Content = topping,
            Alignment = HorizontalAlignment.Right,
            Title = title,
            PrimaryAction = primaryActionText,
            Width = "500px",
            PreventDismissOnOverlayClick = false,
        };
        _dialog = await DialogService.ShowPanelAsync<ToppingUpsertPanel>(topping, dialogParameter);
        return await _dialog.Result;
    }
}