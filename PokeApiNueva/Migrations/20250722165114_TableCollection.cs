using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeApiNueva.Migrations
{
    /// <inheritdoc />
    public partial class TableCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectionUserPkms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CaughtAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionUserPkms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionUserPkms_Userpkmns_UserId",
                        column: x => x.UserId,
                        principalTable: "Userpkmns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionUserPkms_UserId",
                table: "CollectionUserPkms",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionUserPkms");
        }
    }
}
