using System.Globalization;
using FormsPoc.Components.Pages;
using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public interface IInput
{
    Form? Form { get; set; }

    string? Id { get; set; }

    ValidationState ValidationState { get; set; }

    int OnParamsSetHitCount { get; }

    int ValueChangedHitCount { get; }

    bool Required { get; set; }

    string? ValueAsString { get; }

    Task CustomValidate();

}

public class Input<TValue> : ComponentBase, IInput
{
    public ValidationState ValidationState { get; set; } = null!;

    [CascadingParameter]
    public BoundInputs? BoundInputs { get; set; }

    public string? CssClass { get; set; }

    public int ValueChangedHitCount { get; set; }

    [CascadingParameter]
    public Form? Form { get; set; }

    [Parameter, EditorRequired]
    public string? Id { get; set; }

    public int OnParamsSetHitCount { get; set; } = 0;

    [Parameter]
    public Func<CustomValidationState>? OnValidate { get; set; }

    [Parameter]
    public Func<Task<ValidationState>>? OnValidateAsync { get; set; }

    [Parameter]
    public bool Required { get; set; }

    [Parameter]
    public TValue? Value { get; set; }

    public string? ValueAsString => Value?.ToString();

    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }


    public async Task HandleChangeAsync(ChangeEventArgs changeEventArgs)
    {
        if (!BindConverter.TryConvertTo<TValue>(changeEventArgs.Value, CultureInfo.InvariantCulture, out var newValue))
        {
            throw new InvalidOperationException("Could not parse.");
        }

        Value = newValue;
        await ValueChanged.InvokeAsync(Value);

        if (BoundInputs is { Inputs.Count: > 0 })
        {
            foreach (var input in BoundInputs.Inputs)
            {
                await input.CustomValidate();
            }
        }
        else
        {
            await CustomValidate();
        }

        ValueChangedHitCount++;
    }

    public async Task CustomValidate()
    {
        if (OnValidate == null && OnValidateAsync == null)
        {
            return;
        }

        if (OnValidate != null && OnValidateAsync != null)
        {
            throw new InvalidOperationException("cannot do");
        }

        if (OnValidate != null)
        {
            ValidationState = OnValidate.Invoke();

            return;
        }

        if (OnValidateAsync != null)
        {
            ValidationState = await OnValidateAsync.Invoke();
        }
    }


    private void RequiredAndConfiguredValidate()
    {
        if (ValidationState is { ValidationType: ValidationType.Custom, IsValid: false })
        {
            return;
        }

        ValidationState = RequiredValidate();

        if (ValidationState.IsValid == false)
        {
            return;
        }

        ValidationState = ConfiguredValidate();
    }

    protected virtual ConfiguredValidationState ConfiguredValidate() => new(true);

    protected override void OnInitialized()
    {
        RequiredAndConfiguredValidate();

        Form?.AddInput(this);
        BoundInputs?.AddInput(this);
    }

    protected override void OnParametersSet()
    {
        OnParamsSetHitCount++;

        RequiredAndConfiguredValidate();
    }

    private RequiredValidationState RequiredValidate()
    {
        if (Required && string.IsNullOrEmpty(ValueAsString))
        {
            return new RequiredValidationState(false);
        }

        return new RequiredValidationState(true);
    }
}