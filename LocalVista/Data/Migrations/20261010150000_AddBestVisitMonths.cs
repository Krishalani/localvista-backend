using LocalVista.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalVista.Data.Migrations;

[DbContext(typeof(LocalVistaDbContext))]
[Migration("20261010150000_AddBestVisitMonths")]
public partial class AddBestVisitMonths : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "BestVisitMonths",
            table: "Attractions",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            defaultValue: "");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "BestVisitMonths",
            table: "Attractions");
    }

    protected override void BuildTargetModel(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        // The model snapshot contains the complete target model for future migrations.
    }
}
