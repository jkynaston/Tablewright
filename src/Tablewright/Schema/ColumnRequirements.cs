namespace Tablewright.Schema;

/// <summary>
///     Provides shared instances of common column requirements.
/// </summary>
public static class ColumnRequirements
{
    /// <summary>
    ///     Gets a shared requirement indicating that values must be non-null.
    /// </summary>
    public static readonly ColumnRequirement Required = new ColumnRequirement.Required();

    /// <summary>
    ///     Gets a shared requirement indicating that values may be null or missing.
    /// </summary>
    public static readonly ColumnRequirement Optional = new ColumnRequirement.Optional();
}
