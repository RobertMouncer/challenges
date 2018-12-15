using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class updated_model_merge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Groupid",
                table: "Challenge",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Groupid",
                table: "Challenge",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
