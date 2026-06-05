using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerberApp1.Migrations
{
    /// <inheritdoc />
    public partial class AddSalonNickName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Salons",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Salons_NickName",
                table: "Salons",
                column: "NickName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Salons_NickName",
                table: "Salons");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Salons");
        }
    }
}
