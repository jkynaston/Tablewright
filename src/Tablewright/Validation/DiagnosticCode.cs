namespace Tablewright.Validation;

/// <summary>
///     Identifies a specific kind of validation diagnostic.
/// </summary>
public readonly record struct DiagnosticCode(string Value)
{
    /// <inheritdoc />
    public override string ToString() => Value;
}
