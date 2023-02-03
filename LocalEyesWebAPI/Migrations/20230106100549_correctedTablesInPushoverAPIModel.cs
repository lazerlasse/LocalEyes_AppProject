using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalEyesWebAPI.Migrations
{
    public partial class correctedTablesInPushoverAPIModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentMessageCount",
                table: "PushoverSenderAPIs");

            migrationBuilder.DropColumn(
                name: "MaxMessageCount",
                table: "PushoverSenderAPIs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentMessageCount",
                table: "PushoverSenderAPIs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxMessageCount",
                table: "PushoverSenderAPIs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
