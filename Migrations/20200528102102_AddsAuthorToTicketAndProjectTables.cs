using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class AddsAuthorToTicketAndProjectTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Ticket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AuthorId",
                table: "Ticket",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_AuthorId",
                table: "Project",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_AuthorId",
                table: "Project",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_AuthorId",
                table: "Ticket",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_AuthorId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_AuthorId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_AuthorId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Project_AuthorId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Project");
        }
    }
}
