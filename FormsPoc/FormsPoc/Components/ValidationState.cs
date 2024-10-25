namespace FormsPoc.Components;

public class ValidationState(bool isValid, string? validationMessage = null)
{

    public ValidationType ValidationType { get; set; }

    public bool IsValid { get; set; } = isValid;

    public string? ValidationMessage { get; set; } = validationMessage;
}

public class CustomValidationState : ValidationState
{
    public CustomValidationState(bool isValid, string? validationMessage = null) : base(isValid, validationMessage)
    {
        ValidationType = ValidationType.Custom;
        IsValid = isValid;
        ValidationMessage = validationMessage;
    }
}

public class ConfiguredValidationState : ValidationState
{
    public ConfiguredValidationState(bool isValid, string? validationMessage = null) : base(isValid, validationMessage)
    {
        ValidationType = ValidationType.Configured;
        IsValid = isValid;
        ValidationMessage = validationMessage;
    }
}

public class RequiredValidationState : ValidationState
{
    public RequiredValidationState(bool isValid) : base(isValid)
    {
        ValidationType = ValidationType.Required;
        IsValid = isValid;
    }
}

public enum ValidationType
{
    Configured,
    Custom,
    Required
}