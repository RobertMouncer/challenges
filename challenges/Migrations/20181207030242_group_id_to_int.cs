using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class group_id_to_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isGroupChallenge",
                table: "Challenge",
                newName: "IsGroupChallenge");

            migrationBuilder.AlterColumn<int>(
                name: "Groupid",
                table: "Challenge",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsGroupChallenge",
                table: "Challenge",
                newName: "isGroupChallenge");

            migrationBuilder.AlterColumn<string>(
                name: "Groupid",
                table: "Challenge",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
