using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project1.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    OfficeLocation = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckedIn = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkPattern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPattern", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPattern_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Absent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    workPatternId = table.Column<Guid>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absent_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Absent_WorkPattern_workPatternId",
                        column: x => x.workPatternId,
                        principalTable: "WorkPattern",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkPatternPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    WorkPatternId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPatternPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPatternPart_WorkPattern_WorkPatternId",
                        column: x => x.WorkPatternId,
                        principalTable: "WorkPattern",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absent_UserId",
                table: "Absent",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Absent_workPatternId",
                table: "Absent",
                column: "workPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPattern_UserId",
                table: "WorkPattern",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPatternPart_WorkPatternId",
                table: "WorkPatternPart",
                column: "WorkPatternId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absent");

            migrationBuilder.DropTable(
                name: "WorkPatternPart");

            migrationBuilder.DropTable(
                name: "WorkPattern");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
