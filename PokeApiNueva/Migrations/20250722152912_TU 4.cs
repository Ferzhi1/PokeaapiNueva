using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeApiNueva.Migrations
{
    /// <inheritdoc />
    public partial class TU4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Userpkmns",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCreditDate",
                table: "Userpkmns",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Userpkmns");

            migrationBuilder.DropColumn(
                name: "LastCreditDate",
                table: "Userpkmns");
        }
    }
}
