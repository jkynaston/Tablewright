using System.Collections.Immutable;

namespace Tablewright.Schema;

/// <summary>
///     Represents the logical type of values in a schema column.
/// </summary>
/// <remarks>
///     <para>
///         A logical type describes how raw source values should be interpreted before they are exposed as .NET values.
///         For example, a column may be logically a date even though the original CSV cell is just text.
///     </para>
///     <para>
///         Logical types are deliberately separate from CLR types. This allows schema concepts such as formatting,
///         culture, and accepted boolean tokens to be represented directly.
///     </para>
/// </remarks>
public abstract record LogicalType
{
    private LogicalType() { }

    /// <summary>
    ///     Represents unrestricted textual data.
    /// </summary>
    public sealed record Text : LogicalType;

    /// <summary>
    ///     Represents a 32-bit signed integer value.
    /// </summary>
    public sealed record Int32 : LogicalType;

    /// <summary>
    ///     Represents a 64-bit signed integer value.
    /// </summary>
    public sealed record Int64 : LogicalType;

    /// <summary>
    ///     Represents a fixed-point decimal number.
    /// </summary>
    /// <param name="Format">
    ///     An optional format string used when parsing decimal values.
    /// </param>
    /// <param name="CultureName">
    ///     An optional culture name used when parsing culture-sensitive decimal values, such as values with locale-specific
    ///     decimal separators.
    /// </param>
    public sealed record Decimal(string? Format = null, string? CultureName = null) : LogicalType;

    /// <summary>
    ///     Represents a boolean value with configurable source tokens.
    /// </summary>
    /// <param name="TrueTokens">
    ///     The raw source values that should be interpreted as <see langword="true" />.
    /// </param>
    /// <param name="FalseTokens">
    ///     The raw source values that should be interpreted as <see langword="false" />.
    /// </param>
    /// <remarks>
    ///     Boolean tokens allow schemas to support source values such as <c>true</c>/<c>false</c>, <c>yes</c>/<c>no</c>,
    ///     <c>1</c>/<c>0</c>, or other domain-specific representations.
    /// </remarks>
    public sealed record Boolean(
        ImmutableArray<string> TrueTokens = default,
        ImmutableArray<string> FalseTokens = default
    ) : LogicalType;

    /// <summary>
    ///     Represents a calendar date without a time component.
    /// </summary>
    /// <param name="Format">
    ///     An optional format string used when parsing date values.
    /// </param>
    /// <param name="CultureName">
    ///     An optional culture name used when parsing culture-sensitive date values.
    /// </param>
    public sealed record Date(string? Format = null, string? CultureName = null) : LogicalType;

    /// <summary>
    ///     Represents a timestamp value.
    /// </summary>
    /// <param name="Format">
    ///     An optional format string used when parsing timestamp values.
    /// </param>
    /// <param name="CultureName">
    ///     An optional culture name used when parsing culture-sensitive timestamp
    ///     values.
    /// </param>
    /// <remarks>
    ///     Timestamp values may include both date and time information. The exact runtime representation can be chosen by the
    ///     importer, for example <see cref="DateTime" /> or <see cref="DateTimeOffset" />.
    /// </remarks>
    public sealed record Timestamp(string? Format = null, string? CultureName = null) : LogicalType;
}
