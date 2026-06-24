using Shouldly;
using Tablewright.Schema;
using Tablewright.Validation;

namespace Tablewright.Tests.Schema;

public sealed class TableSchemaTests
{
    [Fact]
    public void Create_ReturnsDiagnostic_WhenPrimaryKeyReferencesUnknownColumn()
    {
        // Arrange
        ColumnSchema idColumn = new ColumnSchema(
            name: "id",
            type: new LogicalType.Int32(),
            requirement: ColumnRequirements.Required
        );

        // Act
        ValidationResult<TableSchema> result = TableSchema.Create(
            columns: [idColumn],
            primaryKey: ["missing_id"]
        );

        // Assert
        result.IsValid.ShouldBeFalse();

        TableDiagnostic diagnostic = result.Diagnostics.ShouldHaveSingleItem();

        diagnostic.ShouldSatisfyAllConditions(
            () => diagnostic.Code.ShouldBe(SchemaDiagnosticCodes.UnknownPrimaryKeyColumn),
            () => diagnostic.Severity.ShouldBe(DiagnosticSeverity.Error)
        );
    }
}
