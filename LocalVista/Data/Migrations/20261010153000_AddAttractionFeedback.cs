using LocalVista.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalVista.Data.Migrations;

[DbContext(typeof(LocalVistaDbContext))]
[Migration("20261010153000_AddAttractionFeedback")]
public partial class AddAttractionFeedback : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AttractionFeedback",
            columns: table => new
            {
                AttractionFeedbackId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AttractionId = table.Column<int>(type: "int", nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                Rating = table.Column<int>(type: "int", nullable: false),
                Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                CreatedAtUtc = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AttractionFeedback", x => x.AttractionFeedbackId);
                table.ForeignKey(
                    name: "FK_AttractionFeedback_Attractions_AttractionId",
                    column: x => x.AttractionId,
                    principalTable: "Attractions",
                    principalColumn: "AttractionId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AttractionFeedback_AttractionId_CreatedAtUtc",
            table: "AttractionFeedback",
            columns: new[] { "AttractionId", "CreatedAtUtc" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "AttractionFeedback");
    }

    protected override void BuildTargetModel(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        // The model snapshot stores the complete current model for future migrations.
    }
}
