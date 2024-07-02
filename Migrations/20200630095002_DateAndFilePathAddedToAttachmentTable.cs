using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class DateAndFilePathAddedToAttachmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Attachment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Attachment",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Attachment");
        }
    }
}
