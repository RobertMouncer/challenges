using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class movedGoalMetric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalMetric",
                table: "Activity");

            migrationBuilder.AddColumn<string>(
                name: "GoalMetric",
                table: "Challenge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalMetric",
                table: "Challenge");

            migrationBuilder.AddColumn<string>(
                name: "GoalMetric",
                table: "Activity",
                nullable: true);
        }
    }
}
