using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class GoalMetric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalMetric",
                table: "Challenge");

            migrationBuilder.AddColumn<int>(
                name: "GoalMetricId",
                table: "Challenge",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GoalMetric",
                columns: table => new
                {
                    GoalMetricId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GoalMetricDisplay = table.Column<string>(nullable: true),
                    GoalMetricDbName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalMetric", x => x.GoalMetricId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_GoalMetricId",
                table: "Challenge",
                column: "GoalMetricId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenge_GoalMetric_GoalMetricId",
                table: "Challenge",
                column: "GoalMetricId",
                principalTable: "GoalMetric",
                principalColumn: "GoalMetricId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenge_GoalMetric_GoalMetricId",
                table: "Challenge");

            migrationBuilder.DropTable(
                name: "GoalMetric");

            migrationBuilder.DropIndex(
                name: "IX_Challenge_GoalMetricId",
                table: "Challenge");

            migrationBuilder.DropColumn(
                name: "GoalMetricId",
                table: "Challenge");

            migrationBuilder.AddColumn<string>(
                name: "GoalMetric",
                table: "Challenge",
                nullable: true);
        }
    }
}
