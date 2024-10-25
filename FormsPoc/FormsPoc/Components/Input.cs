using System.Globalization;
using FormsPoc.Components.Pages;
using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public interface IInput
{
    Form? Form { get; set; }

    string? Id { get; set; }

    bool IsValid { get; set; }

    int opsHitCount { get; }

    bool Required { get; set; }

    string? ValueAsString { get; }

    bool IsInputValid();

    void SetValidation(bool isValid, string? validationMessage = null);

    int onchangehitcount { get; set; }

    EventCallback<IInput> OnChange { get; set; }
}

public class Input<TValue> : ComponentBase, IInput
{
    public HashSet<string> ValidationMessages = [];

    private bool _isValid;

    [CascadingParameter]
    public BoundInputs? BoundInputs { get; set; }

    public string? CssClass { get; set; }

    [CascadingParameter]
    public Form? Form { get; set; }

    [Parameter, EditorRequired]
    public string? Id { get; set; }

    public bool IsValid
    {
        get => _isValid;
        set
        {
            CssClass = value ? "valid" : "invalid";
            _isValid = value;
        }
    }

    [Parameter]
    public EventCallback<IInput> OnChange { get; set; }

    public int onchangehitcount { get; set; }

    public int opsHitCount { get; set; } = 0;

    [Parameter]
    public bool Required { get; set; }

    [Parameter]
    public TValue? Value { get; set; }

    public string? ValueAsString => Value?.ToString();

    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }


    public void HandleChange(ChangeEventArgs changeEventArgs)
    {
        if (!BindConverter.TryConvertTo<TValue>(changeEventArgs.Value, CultureInfo.InvariantCulture, out var newValue))
        {
            throw new InvalidOperationException("Could not parse.");
        }

        Value = newValue;
        ValueChanged.InvokeAsync(Value);

        if (BoundInputs != null && BoundInputs.Inputs.Any())
        {
            foreach (var input in BoundInputs.Inputs)
            {
                input.OnChange.InvokeAsync(this);
            }
        }
        else
        {
            OnChange.InvokeAsync(this);
        }
    }

    public bool IsInputValid()
    {
        var isConfiguredValid = IsConfiguredValid(out var validationMessage);

        if (isConfiguredValid && IsRequiredValid())
        {
            ValidationMessages.Clear();

            return true;
        }

        if (!string.IsNullOrEmpty(validationMessage))
        {
            ValidationMessages.Add(validationMessage);
        }

        return false;
    }

    public void SetValidation(bool isValid, string? validationMessage = null)
    {
        if (isValid)
        {
            ValidationMessages.Clear();
        }
        else
        {
            if (!string.IsNullOrEmpty(validationMessage))
            {
                ValidationMessages.Add(validationMessage);
            }
        }
    }

    protected virtual bool IsConfiguredValid(out string? validationMessage)
    {
        validationMessage = null;

        return true;
    }

    protected override void OnInitialized()
    {
        IsValid = IsInputValid();

        Form?.AddInput(this);
        BoundInputs?.AddInput(this);
    }

    protected override void OnParametersSet()
    {
        opsHitCount++;
    }

    private bool IsRequiredValid()
    {
        if (Required && string.IsNullOrEmpty(ValueAsString))
        {
            return false;
        }

        return true;
    }
}