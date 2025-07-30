using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeApiNueva.Migrations
{
    /// <inheritdoc />
    public partial class update23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PokemonId",
                table: "CollectionUserPkms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PokemonId",
                table: "CollectionUserPkms");
        }
    }
}
