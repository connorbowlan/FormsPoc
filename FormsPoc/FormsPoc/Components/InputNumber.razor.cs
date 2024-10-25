using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public partial class InputNumber<TValue> : Input<TValue>
{
    [Parameter]
    public double? Max { get; set; }

    [Parameter]
    public double? Min { get; set; }

    protected override bool IsConfiguredValid(out string? validationMessage)
    {
        if (!string.IsNullOrEmpty(ValueAsString) && double.TryParse(ValueAsString, out var valueAsDouble))
        {
            if (valueAsDouble < Min)
            {
                validationMessage = $"Value must be greater than or equal to {Min}.";

                return false;
            }

            if (valueAsDouble > Max)
            {
                validationMessage = $"Value must be less than or equal to {Max}.";

                return false;
            }
        }

        validationMessage = null;

        return true;
    }
}