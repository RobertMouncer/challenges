using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class removed_repeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repeat",
                table: "Challenge");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Repeat",
                table: "Challenge",
                nullable: false,
                defaultValue: false);
        }
    }
}
