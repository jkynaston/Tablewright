namespace Tablewright.Validation;

/// <summary>
///     Describes a validation issue found in a schema or tabular data source.
/// </summary>
public sealed record TableDiagnostic(
    DiagnosticCode Code,
    DiagnosticSeverity Severity,
    DiagnosticLocation Location,
    string Message
);
