using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public partial class InputNumber<TValue> : Input<TValue>
{
    [Parameter]
    public double? Max { get; set; }

    [Parameter]
    public double? Min { get; set; }

    protected override ConfiguredValidationState ConfiguredValidate()
    {
        if (!string.IsNullOrEmpty(ValueAsString) && double.TryParse(ValueAsString, out var valueAsDouble))
        {
            if (valueAsDouble < Min)
            {
                return new ConfiguredValidationState(false, $"Value must be greater than or equal to {Min}.");
            }

            if (valueAsDouble > Max)
            {
                return new ConfiguredValidationState(false, $"Value must be less than or equal to {Max}.");
            }
        }

        return new ConfiguredValidationState(true);
    }
}