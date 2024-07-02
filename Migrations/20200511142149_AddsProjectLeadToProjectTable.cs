using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class AddsProjectLeadToProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectLeadId",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectLeadId",
                table: "Project",
                column: "ProjectLeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_ProjectLeadId",
                table: "Project",
                column: "ProjectLeadId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_ProjectLeadId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectLeadId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectLeadId",
                table: "Project");
        }
    }
}
