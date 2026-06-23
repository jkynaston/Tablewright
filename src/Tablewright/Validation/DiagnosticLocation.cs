namespace Tablewright.Validation;

/// <summary>
///     Describes where a validation diagnostic occurred.
/// </summary>
public abstract record DiagnosticLocation
{
    private DiagnosticLocation() { }

    /// <summary>
    ///     Indicates that the diagnostic applies to the schema as a whole.
    /// </summary>
    public sealed record Schema : DiagnosticLocation;

    /// <summary>
    ///     Indicates that the diagnostic applies to a specific schema column.
    /// </summary>
    public sealed record SchemaColumn(string ColumnName) : DiagnosticLocation;

    /// <summary>
    ///     Indicates that the diagnostic applies to a specific source cell.
    /// </summary>
    public sealed record Cell(long RowNumber, int ColumnIndex, string? ColumnName = null)
        : DiagnosticLocation;
}
