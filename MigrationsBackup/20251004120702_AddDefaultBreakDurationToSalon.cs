using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerberApp1.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultBreakDurationToSalon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultBreakDurationInMinutes",
                table: "Salons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBreak",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultBreakDurationInMinutes",
                table: "Salons");

            migrationBuilder.DropColumn(
                name: "IsBreak",
                table: "Appointments");
        }
    }
}
