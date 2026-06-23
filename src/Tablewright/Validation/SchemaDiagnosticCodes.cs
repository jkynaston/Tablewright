namespace Tablewright.Validation;

/// <summary>
///     Contains diagnostic codes produced when validating schemas.
/// </summary>
public static class SchemaDiagnosticCodes
{
    /// <summary>
    /// Represents a diagnostic code indicating that a schema contains duplicate column names. Case-insensitive.
    /// </summary>
    public static DiagnosticCode DuplicateColumnName { get; } = new DiagnosticCode("TW_SCHEMA_001");

    /// <summary>
    /// Represents a diagnostic code indicating that a schema's primary key contains duplicate column names.
    /// </summary>
    public static DiagnosticCode DuplicatePrimaryKeyColumn { get; } =
        new DiagnosticCode("TW_SCHEMA_002");

    /// <summary>
    /// Represents a diagnostic code indicating that a primary key contains an empty or whitespace-only column name.
    /// </summary>
    public static DiagnosticCode EmptyPrimaryKeyColumn { get; } =
        new DiagnosticCode("TW_SCHEMA_003");

    /// <summary>
    /// Represents a diagnostic code indicating that a schema contains a column
    /// with an empty or whitespace-only name.
    /// </summary>
    public static DiagnosticCode EmptyColumnName { get; } = new DiagnosticCode("TW_SCHEMA_004");

    /// <summary>
    /// Represents a diagnostic code indicating that a primary key references a column
    /// that does not exist in the schema definition.
    /// </summary>
    public static DiagnosticCode UnknownPrimaryKeyColumn { get; } =
        new DiagnosticCode("TW_SCHEMA_005");
}
