using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class UserAddedToAttachmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Attachment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_UserId",
                table: "Attachment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_AspNetUsers_UserId",
                table: "Attachment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_AspNetUsers_UserId",
                table: "Attachment");

            migrationBuilder.DropIndex(
                name: "IX_Attachment_UserId",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Attachment");
        }
    }
}
