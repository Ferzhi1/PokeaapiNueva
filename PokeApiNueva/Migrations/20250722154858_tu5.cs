using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeApiNueva.Migrations
{
    /// <inheritdoc />
    public partial class tu5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Userpkmns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Userpkmns");
        }
    }
}
