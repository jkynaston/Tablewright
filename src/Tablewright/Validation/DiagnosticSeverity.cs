namespace Tablewright.Validation;

/// <summary>
///     Describes the severity of a validation diagnostic.
/// </summary>
public enum DiagnosticSeverity
{
    /// <summary>
    ///     Indicates that the diagnostic is informational and does not represent
    ///     a validation problem.
    /// </summary>
    Info,

    /// <summary>
    ///     Indicates that the diagnostic describes a potential problem but does
    ///     not make the validated value invalid.
    /// </summary>
    Warning,

    /// <summary>
    ///     Indicates that the diagnostic describes a validation problem that makes
    ///     the validated value invalid.
    /// </summary>
    Error,
}
