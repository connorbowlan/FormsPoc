using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public partial class InputText<TValue> : Input<TValue>
{
    [Parameter]
    public int? MaxLength { get; set; }

    [Parameter]
    public int? MinLength { get; set; }

    protected override ConfiguredValidationState ConfiguredValidate()
    {
        if (ValueAsString == null)
        {
            return new ConfiguredValidationState(true);
        }

        if (MinLength.HasValue && ValueAsString.Length < MinLength.Value)
        {

            return new ConfiguredValidationState(false, $"Must be at least {MinLength.Value} characters.");
        }

        if (MaxLength.HasValue && ValueAsString.Length > MaxLength.Value)
        {
            return new ConfiguredValidationState(false, $"Must be at most {MaxLength.Value} characters.");
        }

        return new ConfiguredValidationState(true);
    }
}