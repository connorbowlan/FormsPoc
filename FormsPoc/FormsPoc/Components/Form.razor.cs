using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace FormsPoc.Components;

public partial class Form
{
    [Parameter, EditorRequired]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Debug { get; set; } = true;

    public HashSet<IInput> Inputs { get; set; } = [];

    // TODO: Default to false

    internal void AddInput(IInput input)
    {
        Inputs.Add(input);

        // Trigger StateHasChanged() to update the form's debug table
        if (Debug && Debugger.IsAttached)
        {
            StateHasChanged();
        }
    }
}