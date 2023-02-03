using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalEyesWebAPI.Migrations
{
    public partial class correctedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Messages",
                newName: "MessageText");

            migrationBuilder.RenameColumn(
                name: "MessageModelID",
                table: "Messages",
                newName: "MessageID");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Messages",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "MessageText",
                table: "Messages",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "MessageID",
                table: "Messages",
                newName: "MessageModelID");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
