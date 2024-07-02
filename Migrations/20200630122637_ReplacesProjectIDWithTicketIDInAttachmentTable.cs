using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class ReplacesProjectIDWithTicketIDInAttachmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Project_ProjectID",
                table: "Attachment");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Ticket_TicketId",
                table: "Attachment");

            migrationBuilder.DropIndex(
                name: "IX_Attachment_ProjectID",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Attachment");

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "Attachment",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Ticket_TicketId",
                table: "Attachment",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Ticket_TicketId",
                table: "Attachment");

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "Attachment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Attachment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ProjectID",
                table: "Attachment",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Project_ProjectID",
                table: "Attachment",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Ticket_TicketId",
                table: "Attachment",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
