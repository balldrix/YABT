using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class ForeignKeysAddedToTicketTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Priority_PriorityId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Project_ProjectId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_State_StateId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_ItemType_TypeId",
                table: "Ticket");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Ticket",
                newName: "TypeID");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Ticket",
                newName: "StateID");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Ticket",
                newName: "ProjectID");

            migrationBuilder.RenameColumn(
                name: "PriorityId",
                table: "Ticket",
                newName: "PriorityID");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_TypeId",
                table: "Ticket",
                newName: "IX_Ticket_TypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_StateId",
                table: "Ticket",
                newName: "IX_Ticket_StateID");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_ProjectId",
                table: "Ticket",
                newName: "IX_Ticket_ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_PriorityId",
                table: "Ticket",
                newName: "IX_Ticket_PriorityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Priority_PriorityID",
                table: "Ticket",
                column: "PriorityID",
                principalTable: "Priority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Project_ProjectID",
                table: "Ticket",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_State_StateID",
                table: "Ticket",
                column: "StateID",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_ItemType_TypeID",
                table: "Ticket",
                column: "TypeID",
                principalTable: "ItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Priority_PriorityID",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Project_ProjectID",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_State_StateID",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_ItemType_TypeID",
                table: "Ticket");

            migrationBuilder.RenameColumn(
                name: "TypeID",
                table: "Ticket",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "StateID",
                table: "Ticket",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "ProjectID",
                table: "Ticket",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "PriorityID",
                table: "Ticket",
                newName: "PriorityId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_TypeID",
                table: "Ticket",
                newName: "IX_Ticket_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_StateID",
                table: "Ticket",
                newName: "IX_Ticket_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_ProjectID",
                table: "Ticket",
                newName: "IX_Ticket_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_PriorityID",
                table: "Ticket",
                newName: "IX_Ticket_PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Priority_PriorityId",
                table: "Ticket",
                column: "PriorityId",
                principalTable: "Priority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Project_ProjectId",
                table: "Ticket",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_State_StateId",
                table: "Ticket",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_ItemType_TypeId",
                table: "Ticket",
                column: "TypeId",
                principalTable: "ItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
