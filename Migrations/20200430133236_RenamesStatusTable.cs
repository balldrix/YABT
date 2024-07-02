using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherBugTracker.Migrations
{
    public partial class RenamesStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Status_StateId",
                table: "Ticket");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "State",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Backlog" },
                    { 2, "Selected for Development" },
                    { 3, "In Progress" },
                    { 4, "Ready for Testing" },
                    { 5, "Testing" },
                    { 6, "Complete" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_State_StateId",
                table: "Ticket",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_State_StateId",
                table: "Ticket");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Backlog" },
                    { 2, "Selected for Development" },
                    { 3, "In Progress" },
                    { 4, "Ready for Testing" },
                    { 5, "Testing" },
                    { 6, "Complete" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Status_StateId",
                table: "Ticket",
                column: "StateId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
