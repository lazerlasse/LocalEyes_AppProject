using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalEyesWebAPI.Migrations
{
    public partial class correctionsInPushoverAPIModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PushoverSenderAPIKey",
                table: "PushoverSenderAPIs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PushoverSenderAPIKey",
                table: "PushoverSenderAPIs");
        }
    }
}
