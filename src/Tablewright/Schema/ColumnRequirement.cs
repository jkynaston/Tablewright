namespace Tablewright.Schema;

/// <summary>
///     Describes whether values in a schema column are required.
/// </summary>
/// <remarks>
///     A column requirement describes the value-level requirement for a column.
///     It does not necessarily describe whether the column itself must be present
///     in a source file.
/// </remarks>
public abstract record ColumnRequirement
{
    private ColumnRequirement() { }

    /// <summary>
    ///     Gets a value indicating whether this requirement allows a null or
    ///     missing value.
    /// </summary>
    public abstract bool AllowsNull { get; }

    /// <summary>
    ///     Gets a value indicating whether this requirement requires a non-null
    ///     value.
    /// </summary>
    public bool RequiresValue => !AllowsNull;

    /// <summary>
    ///     Indicates that the column may contain a null or missing value.
    /// </summary>
    public sealed record Optional : ColumnRequirement
    {
        /// <inheritdoc />
        public override bool AllowsNull => true;
    }

    /// <summary>
    ///     Indicates that the column must contain a non-null value.
    /// </summary>
    public sealed record Required : ColumnRequirement
    {
        /// <inheritdoc />
        public override bool AllowsNull => false;
    }
}
