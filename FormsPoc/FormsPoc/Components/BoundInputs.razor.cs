using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public partial class BoundInputs : ComponentBase
{

    [Parameter, EditorRequired]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    public Form? Form { get; set; }

    public HashSet<IInput> Inputs { get; set; } = [];

    internal void AddInput(IInput input)
    {
        Inputs.Add(input);
    }
}