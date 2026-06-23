using System.Collections.Immutable;
using Tablewright.Validation;

namespace Tablewright.Schema;

/// <summary>
///     Describes the expected structure of a tabular data source.
/// </summary>
/// <remarks>
///     <para>
///         A <see cref="TableSchema" /> defines the logical columns that make up a table,
///         along with optional key information used to identify rows.
///     </para>
///     <para>
///         This type is independent of any particular storage format. It may describe
///         data read from CSV, JSON, a database result set, or another tabular source.
///     </para>
///     <para>
///         Instances of this type are expected to be valid. Use <see cref="Create" />
///         to construct schemas while collecting validation diagnostics.
///     </para>
/// </remarks>
public sealed class TableSchema
{
    private TableSchema(ImmutableArray<ColumnSchema> columns, ImmutableArray<string> primaryKey)
    {
        Columns = columns;
        PrimaryKey = primaryKey;
    }

    /// <summary>
    ///     Gets the columns defined by this schema.
    /// </summary>
    public ImmutableArray<ColumnSchema> Columns { get; }

    /// <summary>
    ///     Gets the canonical column names that form the table's primary key.
    /// </summary>
    /// <remarks>
    ///     Primary key columns are expected to uniquely identify a row. Primary key
    ///     validation of actual row values is performed by importers or validators,
    ///     not by this schema object.
    /// </remarks>
    public ImmutableArray<string> PrimaryKey { get; }

    /// <summary>
    ///     Creates a table schema from the supplied column and primary key definitions.
    /// </summary>
    /// <param name="columns">
    ///     The columns expected to appear in the table.
    /// </param>
    /// <param name="primaryKey">
    ///     The canonical names of the columns that form the table's primary key,
    ///     or an empty collection if the table has no primary key.
    /// </param>
    /// <returns>
    ///     A successful <see cref="ValidationResult{T}" /> containing a valid
    ///     <see cref="TableSchema" /> when the supplied definition is valid;
    ///     otherwise, a failed result containing schema diagnostics.
    /// </returns>
    /// <remarks>
    ///     This method does not throw for schema validity issues. Invalid schema
    ///     definitions are reported as diagnostics, so callers can display all
    ///     schema problems at once.
    /// </remarks>
    public static ValidationResult<TableSchema> Create(
        ImmutableArray<ColumnSchema> columns,
        ImmutableArray<string> primaryKey = default
    )
    {
        columns = columns.IsDefault ? ImmutableArray<ColumnSchema>.Empty : columns;
        primaryKey = primaryKey.IsDefault ? ImmutableArray<string>.Empty : primaryKey;

        ImmutableArray<TableDiagnostic>.Builder diagnostics =
            ImmutableArray.CreateBuilder<TableDiagnostic>();

        AddColumnNameDiagnostics(columns, diagnostics);
        AddPrimaryKeyDiagnostics(columns, primaryKey, diagnostics);

        return diagnostics.Any(diagnostic => diagnostic.Severity is DiagnosticSeverity.Error)
            ? ValidationResult<TableSchema>.Failure(diagnostics.ToImmutable())
            : ValidationResult<TableSchema>.Success(new TableSchema(columns, primaryKey));
    }

    /// <summary>
    ///     Creates a table schema from the supplied column and primary key definitions,
    ///     throwing an exception if the definition is invalid.
    /// </summary>
    /// <param name="columns">
    ///     The columns expected to appear in the table.
    /// </param>
    /// <param name="primaryKey">
    ///     The canonical names of the columns that form the table's primary key,
    ///     or an empty collection if the table has no primary key.
    /// </param>
    /// <returns>
    ///     A valid <see cref="TableSchema" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the supplied schema definition is invalid.
    /// </exception>
    /// <remarks>
    ///     Prefer <see cref="Create" /> when schema validity is part of normal
    ///     program flow. This method is intended for tests, examples, and call sites
    ///     where invalid schema definitions indicate programmer error.
    /// </remarks>
    public static TableSchema CreateOrThrow(
        ImmutableArray<ColumnSchema> columns,
        ImmutableArray<string> primaryKey = default
    )
    {
        ValidationResult<TableSchema> result = Create(columns, primaryKey);

        if (result is { IsValid: true, Value: not null })
            return result.Value;

        string message = string.Join(
            Environment.NewLine,
            result.Diagnostics.Select(diagnostic => diagnostic.Message)
        );

        throw new ArgumentException(message);
    }

    /// <summary>
    ///     Adds diagnostics for invalid column names.
    /// </summary>
    /// <param name="columns">
    ///     The columns to validate.
    /// </param>
    /// <param name="diagnostics">
    ///     The diagnostic collection to append to.
    /// </param>
    private static void AddColumnNameDiagnostics(
        ImmutableArray<ColumnSchema> columns,
        ImmutableArray<TableDiagnostic>.Builder diagnostics
    )
    {
        diagnostics.AddRange(
            columns
                .Where(column => string.IsNullOrWhiteSpace(column.Name))
                .Select(_ => new TableDiagnostic(
                    SchemaDiagnosticCodes.EmptyColumnName,
                    DiagnosticSeverity.Error,
                    new DiagnosticLocation.Schema(),
                    "Schema contains a column with an empty or whitespace-only name."
                ))
        );

        diagnostics.AddRange(
            columns
                .Where(column => !string.IsNullOrWhiteSpace(column.Name))
                .GroupBy(column => column.Name, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => new TableDiagnostic(
                    SchemaDiagnosticCodes.DuplicateColumnName,
                    DiagnosticSeverity.Error,
                    new DiagnosticLocation.SchemaColumn(group.Key),
                    $"Schema contains duplicate column name '{group.Key}'."
                ))
        );
    }

    /// <summary>
    ///     Adds diagnostics for invalid primary key definitions.
    /// </summary>
    /// <param name="columns">
    ///     The schema columns available for primary key resolution.
    /// </param>
    /// <param name="primaryKey">
    ///     The primary key column names to validate.
    /// </param>
    /// <param name="diagnostics">
    ///     The diagnostic collection to append to.
    /// </param>
    private static void AddPrimaryKeyDiagnostics(
        ImmutableArray<ColumnSchema> columns,
        ImmutableArray<string> primaryKey,
        ImmutableArray<TableDiagnostic>.Builder diagnostics
    )
    {
        if (primaryKey.IsEmpty)
            return;

        diagnostics.AddRange(
            primaryKey
                .Where(columnName => !string.IsNullOrWhiteSpace(columnName))
                .Select(columnName => new TableDiagnostic(
                    SchemaDiagnosticCodes.EmptyPrimaryKeyColumn,
                    DiagnosticSeverity.Error,
                    new DiagnosticLocation.Schema(),
                    "Primary key contains an empty or whitespace-only column name."
                ))
        );

        diagnostics.AddRange(
            primaryKey
                .Where(columnName => !string.IsNullOrWhiteSpace(columnName))
                .GroupBy(columnName => columnName, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => new TableDiagnostic(
                    SchemaDiagnosticCodes.DuplicatePrimaryKeyColumn,
                    DiagnosticSeverity.Error,
                    new DiagnosticLocation.SchemaColumn(group.Key),
                    $"Primary key contains duplicate column name '{group.Key}'."
                ))
        );

        HashSet<string> columnNames = columns
            .Where(column => !string.IsNullOrWhiteSpace(column.Name))
            .Select(column => column.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        diagnostics.AddRange(
            primaryKey
                .Where(primaryKeyColumn => !string.IsNullOrWhiteSpace(primaryKeyColumn))
                .Where(primaryKeyColumn => !columnNames.Contains(primaryKeyColumn))
                .Select(primaryKeyColumn => new TableDiagnostic(
                    SchemaDiagnosticCodes.UnknownPrimaryKeyColumn,
                    DiagnosticSeverity.Error,
                    new DiagnosticLocation.SchemaColumn(primaryKeyColumn),
                    $"Primary key references unknown column '{primaryKeyColumn}'."
                ))
        );
    }

    /// <summary>
    ///     Finds a column by its canonical schema name.
    /// </summary>
    /// <param name="name">
    ///     The canonical name of the column to find.
    /// </param>
    /// <returns>
    ///     The matching <see cref="ColumnSchema" /> if one exists; otherwise, <see langword="null" />.
    /// </returns>
    /// <remarks>
    ///     The comparison uses <see cref="StringComparer.OrdinalIgnoreCase" />. Display names, source headers, and
    ///     aliases are not considered by this method unless they are also represented as canonical column names.
    /// </remarks>
    public ColumnSchema? FindColumn(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return Columns.FirstOrDefault(column =>
            StringComparer.OrdinalIgnoreCase.Equals(column.Name, name)
        );
    }
}
