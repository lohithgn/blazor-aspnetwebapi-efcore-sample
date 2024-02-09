using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminUI.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Contoso.Pizza.AdminUI.Components.Pages.Sauce;

public partial class SaucesPage
{
    [Inject]
    public required ISauceService Service { get; set; }

    private IQueryable<SauceEntity>? _sauces;

    private IDialogReference? _dialog;

    protected override async Task OnInitializedAsync()
    {
        await LoadSauces();
    }

    private async Task LoadSauces()
    {
        _sauces = (await Service.GetAllSaucesAsync()).AsQueryable();
    }

    private async Task OnAddNewSauceClick()
    {
        var panelTitle = $"Add a sauce";
        var result = await ShowPanel(panelTitle, new SauceEntity() { Name = "New Sauce" });
        if (result.Cancelled)
        {
            return;
        }
        var entity = result.Data as SauceEntity;
        ShowProgressToast(nameof(OnAddNewSauceClick), "Sauce", entity!.Name);
        await Service.AddSauceAsync(entity!);
        CloseProgressToast(nameof(OnAddNewSauceClick));
        ShowSuccessToast("Sauce", entity!.Name);
        await LoadSauces();
    }

    private async Task OnEditSauceClick(SauceEntity sauce)
    {
        var panelTitle = $"Edit sauce";
        var result = await ShowPanel(panelTitle, sauce, false);
        if (result.Cancelled)
        {
            return;
        }
        var entity = result.Data as SauceEntity;
        ShowProgressToast(nameof(OnEditSauceClick), "Sauce", entity!.Name, Operation.Update);
        await Service.UpdateSauceAsync(entity!);
        CloseProgressToast(nameof(OnEditSauceClick));
        ShowSuccessToast("Sauce", entity!.Name, Operation.Update);
        await LoadSauces();
    }

    private async Task OnDeleteSauceClick(SauceEntity entity)
    {
        var title = "Delete sauce?";
        var message = $"Are you sure you want to delete sauce: <b>{entity.Name}</b>?";
        var result = await ShowConfirmationDialogAsync(title, message);
        if (result.Cancelled)
        {
            return;
        }
        try
        {
            ShowProgressToast(nameof(OnDeleteSauceClick), "Sauce", entity!.Name, Operation.Delete);
            await Service.DeleteSauceAsync(entity);
            CloseProgressToast(nameof(OnDeleteSauceClick));
            ShowSuccessToast("Sauce", entity.Name, Operation.Delete);
            await LoadSauces();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ShowFailureToast("Sauce", entity.Name, Operation.Delete, "The sauce is in use and cannot be deleted.");
            }
            else
            {
                ShowFailureToast("Sauce", entity.Name, Operation.Delete, ex.Message);
            }
        }
    }

    private async Task<DialogResult> ShowPanel(string title, SauceEntity sauce, bool isAdd = true)
    {
        var primaryActionText = isAdd ? "Add" : "Save changes";
        var dialogParameter = new DialogParameters<SauceEntity>()
        {
            Content = sauce,
            Alignment = HorizontalAlignment.Right,
            Title = title,
            PrimaryAction = primaryActionText,
            Width = "500px",
            PreventDismissOnOverlayClick = false,
        };
        _dialog = await DialogService.ShowPanelAsync<SauceUpsertPanel>(sauce, dialogParameter);
        return await _dialog.Result;
    }
}