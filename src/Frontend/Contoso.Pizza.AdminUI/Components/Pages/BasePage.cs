using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Contoso.Pizza.AdminUI.Components.Pages;

public class BasePage : ComponentBase
{
    [Inject]
    public required IDialogService DialogService { get; set; }

    [Inject]
    public required IToastService ToastService { get; set; }

    protected enum Operation
    {
        Add,
        Update,
        Delete
    };

    protected async Task<DialogResult> ShowConfirmationDialogAsync(string title, string message)
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
        {
            Content = new()
            {
                Title = title,
                MarkupMessage = new MarkupString(message),
                Icon = new Icons.Regular.Size24.QuestionCircle(),
                IconColor = Color.Error,
            },
            PrimaryAction = "Ok",
            SecondaryAction = "Cancel"
        });
        return await dialog.Result;
    }

    protected void ShowSuccessToast(string entityType, string entityName, Operation operation = Operation.Add)
    {
        var sentenceCasedEntityTypeName = ToSentenceCase(entityType);
        var title = PrepareSuccessToastTitle(operation, sentenceCasedEntityTypeName);
        string message = PrepareSuccessToastMessage(entityName, operation, sentenceCasedEntityTypeName);
        ToastService.ShowCommunicationToast(new ToastParameters<CommunicationToastContent>
        {
            Intent = ToastIntent.Success,
            Title = title,
            Timeout = 5000,
            Content = new CommunicationToastContent
            {
                Details = message
            }
        });
    }

    protected void ShowFailureToast(string entityType, string entityName, Operation operation = Operation.Add, string failureMessage = "")
    {
        var sentenceCasedEntityTypeName = ToSentenceCase(entityType);
        var title = PrepareFailureToastTitle(operation, sentenceCasedEntityTypeName);
        var message = failureMessage == string.Empty ? PrepareFailureToastMessage(entityName, operation, sentenceCasedEntityTypeName)
                                                     : failureMessage;
        ToastService.ShowCommunicationToast(new ToastParameters<CommunicationToastContent>
        {
            Intent = ToastIntent.Error,
            Title = title,
            Timeout = 5000,
            Content = new CommunicationToastContent
            {
                Details = message
            }
        });
    }

    protected void ShowProgressToast(string id, string entityType, string entityName, Operation operation = Operation.Add)
    {
        var sentenceCasedEntityTypeName = ToSentenceCase(entityType);
        ToastService.ShowProgressToast(new ToastParameters<ProgressToastContent>
        {
            Id = id,
            Intent = ToastIntent.Progress,
            Title = PrepareProgressToastTitle(operation, sentenceCasedEntityTypeName),
            Content = new ProgressToastContent
            {
                Details = PrepareProgressToastMessage(entityName, operation),
            }
        });
    }

    protected void CloseProgressToast(string id)
    {
        ToastService.CloseToast(id);
    }

    private static string PrepareProgressToastTitle(Operation operation, string sentenceCasedEntityTypeName)
    {
        return $"{operation switch
        {
            Operation.Add => "Creating",
            Operation.Update => "Updating",
            Operation.Delete => "Deleting",
            _ => "Creating/Updating/Deleting"
        }} {sentenceCasedEntityTypeName}";
    }

    private string PrepareProgressToastMessage(string entityName, Operation operation)
    {
        return $"{operation switch
        {
            Operation.Add => "Creating",
            Operation.Update => "Updating",
            Operation.Delete => "Deleting",
            _ => "Creating/Updating/Deleting"
        }} {entityName}. Please wait...";
    }

    

    private string PrepareSuccessToastTitle(Operation operation, string entityType)
    {
        return $"{entityType} {operation switch
        {
            Operation.Add => "created",
            Operation.Update => "updated",
            Operation.Delete => "deleted",
            _ => "created/updated/deleted"
        }} successfully";
    }

    private string PrepareSuccessToastMessage(string entityName, Operation operation, string entityType)
    {
        return $"{entityType}: {entityName} was {operation switch
        {
            Operation.Add => "created",
            Operation.Update => "updated",
            Operation.Delete => "deleted",
            _ => throw new InvalidOperationException("Invalid operation")
        }} successfully";
    }

    private static string PrepareFailureToastTitle(Operation operation, string entityName)
    {
        return $"{operation switch
        {
            Operation.Add => "Adding",
            Operation.Update => "Updating",
            Operation.Delete => "Deleting",
            _ => "Add/Update/Delete action on "
        }} {entityName} failed";
    }

    private string PrepareFailureToastMessage(string entityName, Operation operation, string entityType)
    {
        return $"Error {entityType}: {entityName} was {operation switch
        {
            Operation.Add => "created",
            Operation.Update => "updated",
            Operation.Delete => "deleted",
            _ => "created/updated/deleted"
        }} successfully";
    }
    
    private string ToSentenceCase(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        string lowerCase = str.ToLower();
        return char.ToUpper(lowerCase[0]) + lowerCase.Substring(1);
    }
}
