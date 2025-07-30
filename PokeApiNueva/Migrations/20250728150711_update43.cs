using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeApiNueva.Migrations
{
    /// <inheritdoc />
    public partial class update43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Userpkmns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiry",
                table: "Userpkmns",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Userpkmns");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiry",
                table: "Userpkmns");
        }
    }
}
