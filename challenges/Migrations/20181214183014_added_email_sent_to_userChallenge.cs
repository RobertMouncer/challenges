using Microsoft.EntityFrameworkCore.Migrations;

namespace challenges.Migrations
{
    public partial class added_email_sent_to_userChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailSent",
                table: "UserChallenge",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSent",
                table: "UserChallenge");
        }
    }
}
