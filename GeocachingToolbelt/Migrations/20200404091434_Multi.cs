using Microsoft.EntityFrameworkCore.Migrations;

namespace GeocachingToolbelt.Migrations
{
    public partial class Multi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GUID",
                table: "Multi",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Multi_GUID",
                table: "Multi",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Multi");
        }
    }
}
