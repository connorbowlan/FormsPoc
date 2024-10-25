using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public partial class InputText<TValue> : Input<TValue>
{
    [Parameter]
    public int? MaxLength { get; set; }

    [Parameter]
    public int? MinLength { get; set; }

    protected override bool IsConfiguredValid(out string? validationMessage)
    {
        if (ValueAsString == null)
        {
            validationMessage = null;

            return true;
        }

        if (MinLength.HasValue && ValueAsString.Length < MinLength.Value)
        {
            validationMessage = $"Must be at least {MinLength.Value} characters.";

            return false;
        }

        if (MaxLength.HasValue && ValueAsString.Length > MaxLength.Value)
        {
            validationMessage = $"Must be at most {MaxLength.Value} characters.";

            return false;
        }

        validationMessage = null;

        return true;
    }
}