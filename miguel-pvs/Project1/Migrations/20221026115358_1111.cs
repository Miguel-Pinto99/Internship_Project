using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project1.Migrations
{
    public partial class _1111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absent_WorkPattern_workPatternId",
                table: "Absent");

            migrationBuilder.DropIndex(
                name: "IX_Absent_workPatternId",
                table: "Absent");

            migrationBuilder.DropColumn(
                name: "workPatternId",
                table: "Absent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "workPatternId",
                table: "Absent",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Absent_workPatternId",
                table: "Absent",
                column: "workPatternId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absent_WorkPattern_workPatternId",
                table: "Absent",
                column: "workPatternId",
                principalTable: "WorkPattern",
                principalColumn: "Id");
        }
    }
}
