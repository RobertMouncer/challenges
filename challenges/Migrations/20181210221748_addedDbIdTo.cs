using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class addedDbIdTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DbActivityId",
                table: "Activity",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DbActivityId",
                table: "Activity");
        }
    }
}
