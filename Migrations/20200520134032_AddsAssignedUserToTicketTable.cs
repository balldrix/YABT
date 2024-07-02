using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class AddsAssignedUserToTicketTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserID",
                table: "Ticket",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AssignedUserID",
                table: "Ticket",
                column: "AssignedUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_AssignedUserID",
                table: "Ticket",
                column: "AssignedUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_AssignedUserID",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_AssignedUserID",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "AssignedUserID",
                table: "Ticket");
        }
    }
}
