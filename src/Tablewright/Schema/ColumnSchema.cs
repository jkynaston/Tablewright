using System.Collections.Immutable;

namespace Tablewright.Schema;

/// <summary>
///     Describes a single logical column in a tabular schema.
/// </summary>
/// <remarks>
///     <para>
///         A column schema describes the meaning of a column independently of any
///         particular source format. The column's <see cref="Name" /> is its canonical
///         machine-facing identifier, while <see cref="Titles" /> contains any
///         human-readable source headers or aliases that may identify it.
///     </para>
///     <para>
///         Null tokens are checked before type conversion. For example, a decimal
///         column may treat <c>""</c>, <c>N/A</c>, or <c>null</c> as missing values
///         rather than invalid decimal values.
///     </para>
/// </remarks>
public sealed record ColumnSchema
{
    /// <summary>
    ///     Initialises a new instance of the <see cref="ColumnSchema" /> record.
    /// </summary>
    /// <param name="name">
    ///     The canonical schema name of the column.
    /// </param>
    /// <param name="type">
    ///     The logical type that values in this column are expected to have.
    /// </param>
    /// <param name="requirement">
    ///     Indicates whether values for this column are required or optional.
    /// </param>
    /// <param name="titles">
    ///     The human-readable titles, source headers, or aliases that may identify
    ///     this column in an input source.
    /// </param>
    /// <param name="nullTokens">
    ///     The raw source values that should be interpreted as null for this column.
    /// </param>
    public ColumnSchema(
        string name,
        LogicalType type,
        ColumnRequirement requirement,
        ImmutableArray<string> titles = default,
        ImmutableArray<string> nullTokens = default
    )
    {
        Name = name;
        Type = type;
        Requirement = requirement;
        Titles = titles.IsDefault ? ImmutableArray<string>.Empty : titles;
        NullTokens = nullTokens.IsDefault ? ImmutableArray<string>.Empty : nullTokens;
    }

    /// <summary>
    ///     Gets the canonical schema name of the column.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the logical type that values in this column are expected to have.
    /// </summary>
    public LogicalType Type { get; }

    /// <summary>
    ///     Gets the human-readable titles, source headers, or aliases that may
    ///     identify this column in an input source.
    /// </summary>
    public ImmutableArray<string> Titles { get; }

    /// <summary>
    ///     Gets the raw source values that should be interpreted as null for this
    ///     column.
    /// </summary>
    public ImmutableArray<string> NullTokens { get; }

    /// <summary>
    ///     Gets whether values for this column are required or optional.
    /// </summary>
    public ColumnRequirement Requirement { get; }

    /// <summary>
    ///     Gets a value indicating whether this column requires a non-null value.
    /// </summary>
    public bool IsRequired => Requirement.RequiresValue;
}
