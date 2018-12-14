using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class goalIsNowDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Goal",
                table: "Challenge",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Goal",
                table: "Challenge",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
