using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class FixStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_CompleteResult_CompleteResultId",
                schema: "LE",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimitiveResult_Exams_ExamsId",
                schema: "LE",
                table: "PrimitiveResult");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Users_UserId",
                schema: "LE",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                schema: "LE",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrimitiveResult",
                schema: "LE",
                table: "PrimitiveResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompleteResult",
                schema: "LE",
                table: "CompleteResult");

            migrationBuilder.DeleteData(
                schema: "LE",
                table: "ApplicationVersions",
                keyColumn: "Id",
                keyValue: new Guid("a8347345-5d4b-4526-a7ba-53a06b8af0ae"));

            migrationBuilder.RenameTable(
                name: "Ticket",
                schema: "LE",
                newName: "Tickets",
                newSchema: "LE");

            migrationBuilder.RenameTable(
                name: "PrimitiveResult",
                schema: "LE",
                newName: "PrimitiveResults",
                newSchema: "LE");

            migrationBuilder.RenameTable(
                name: "CompleteResult",
                schema: "LE",
                newName: "CompleteResults",
                newSchema: "LE");

            migrationBuilder.RenameColumn(
                name: "Word",
                schema: "LE",
                table: "Words",
                newName: "Content");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_UserId",
                schema: "LE",
                table: "Tickets",
                newName: "IX_Tickets_UserId");

            migrationBuilder.RenameColumn(
                name: "ExamsId",
                schema: "LE",
                table: "PrimitiveResults",
                newName: "ExamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                schema: "LE",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrimitiveResults",
                schema: "LE",
                table: "PrimitiveResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompleteResults",
                schema: "LE",
                table: "CompleteResults",
                column: "Id");

            migrationBuilder.InsertData(
                schema: "LE",
                table: "ApplicationVersions",
                columns: new[] { "Id", "Link", "Version" },
                values: new object[] { new Guid("70c022bb-1a32-48b9-97d9-7ef981407cb2"), "none", "1.0.0.0" });

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_CompleteResults_CompleteResultId",
                schema: "LE",
                table: "Exams",
                column: "CompleteResultId",
                principalSchema: "LE",
                principalTable: "CompleteResults",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimitiveResults_Exams_ExamId",
                schema: "LE",
                table: "PrimitiveResults",
                column: "ExamId",
                principalSchema: "LE",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_UserId",
                schema: "LE",
                table: "Tickets",
                column: "UserId",
                principalSchema: "LE",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_CompleteResults_CompleteResultId",
                schema: "LE",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimitiveResults_Exams_ExamId",
                schema: "LE",
                table: "PrimitiveResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_UserId",
                schema: "LE",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                schema: "LE",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrimitiveResults",
                schema: "LE",
                table: "PrimitiveResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompleteResults",
                schema: "LE",
                table: "CompleteResults");

            migrationBuilder.DeleteData(
                schema: "LE",
                table: "ApplicationVersions",
                keyColumn: "Id",
                keyValue: new Guid("70c022bb-1a32-48b9-97d9-7ef981407cb2"));

            migrationBuilder.RenameTable(
                name: "Tickets",
                schema: "LE",
                newName: "Ticket",
                newSchema: "LE");

            migrationBuilder.RenameTable(
                name: "PrimitiveResults",
                schema: "LE",
                newName: "PrimitiveResult",
                newSchema: "LE");

            migrationBuilder.RenameTable(
                name: "CompleteResults",
                schema: "LE",
                newName: "CompleteResult",
                newSchema: "LE");

            migrationBuilder.RenameColumn(
                name: "Content",
                schema: "LE",
                table: "Words",
                newName: "Word");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_UserId",
                schema: "LE",
                table: "Ticket",
                newName: "IX_Ticket_UserId");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                schema: "LE",
                table: "PrimitiveResult",
                newName: "ExamsId");

            migrationBuilder.RenameIndex(
                name: "IX_PrimitiveResults_ExamId",
                schema: "LE",
                table: "PrimitiveResult",
                newName: "IX_PrimitiveResult_ExamsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                schema: "LE",
                table: "Ticket",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrimitiveResult",
                schema: "LE",
                table: "PrimitiveResult",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompleteResult",
                schema: "LE",
                table: "CompleteResult",
                column: "Id");

            migrationBuilder.InsertData(
                schema: "LE",
                table: "ApplicationVersions",
                columns: new[] { "Id", "Link", "PublishDate", "Version" },
                values: new object[] { new Guid("a8347345-5d4b-4526-a7ba-53a06b8af0ae"), "none", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1.0.0.0" });

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_CompleteResult_CompleteResultId",
                schema: "LE",
                table: "Exams",
                column: "CompleteResultId",
                principalSchema: "LE",
                principalTable: "CompleteResult",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimitiveResult_Exams_ExamsId",
                schema: "LE",
                table: "PrimitiveResult",
                column: "ExamsId",
                principalSchema: "LE",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Users_UserId",
                schema: "LE",
                table: "Ticket",
                column: "UserId",
                principalSchema: "LE",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
