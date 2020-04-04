using Microsoft.EntityFrameworkCore.Migrations;

namespace GeocachingToolbelt.Migrations
{
    public partial class Waypoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Variable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MultiId = table.Column<int>(nullable: false),
                    Letter = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variable_Multi_MultiId",
                        column: x => x.MultiId,
                        principalTable: "Multi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Waypoint",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MultiId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: true),
                    Coordinate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waypoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Waypoint_Multi_MultiId",
                        column: x => x.MultiId,
                        principalTable: "Multi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Variable_MultiId",
                table: "Variable",
                column: "MultiId");

            migrationBuilder.CreateIndex(
                name: "IX_Waypoint_MultiId",
                table: "Waypoint",
                column: "MultiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Variable");

            migrationBuilder.DropTable(
                name: "Waypoint");
        }
    }
}
