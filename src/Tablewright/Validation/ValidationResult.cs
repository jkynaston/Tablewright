using System.Collections.Immutable;

namespace Tablewright.Validation;

/// <summary>
///     Represents the result of validating or importing tabular data.
/// </summary>
/// <typeparam name="T">
///     The type of value produced when validation succeeds.
/// </typeparam>
public sealed record ValidationResult<T>
{
    private ValidationResult(T? value, ImmutableArray<TableDiagnostic> diagnostics)
    {
        Value = value;
        Diagnostics = diagnostics.IsDefault ? ImmutableArray<TableDiagnostic>.Empty : diagnostics;
    }

    /// <summary>
    ///     The imported value; <see langword="null" /> when validation failed.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    ///     All diagnostics emitted during validation. Never a default (uninitialized) array.
    /// </summary>
    public ImmutableArray<TableDiagnostic> Diagnostics { get; }

    /// <summary>
    ///     <see langword="true" /> when no <see cref="DiagnosticSeverity.Error" /> diagnostics are present.
    ///     Warnings do not affect validity.
    /// </summary>
    public bool IsValid =>
        Diagnostics.All(diagnostic => diagnostic.Severity is not DiagnosticSeverity.Error);

    /// <summary>
    ///     Creates a successful result with the given value and no diagnostics.
    /// </summary>
    public static ValidationResult<T> Success(T value) =>
        new ValidationResult<T>(value, ImmutableArray<TableDiagnostic>.Empty);

    /// <summary>
    ///     Creates a failed result with no value and the given diagnostics.
    /// </summary>
    public static ValidationResult<T> Failure(ImmutableArray<TableDiagnostic> diagnostics) =>
        new ValidationResult<T>(default, diagnostics);
}
