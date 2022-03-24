using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LE");


            migrationBuilder.CreateTable(
                name: "CompleteResult",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnanswerCount = table.Column<int>(type: "int", nullable: false),
                    QuestionsCount = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswersCount = table.Column<int>(type: "int", nullable: false),
                    IncorrectAnswersCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteResult", x => x.Id);
                });

          
            migrationBuilder.CreateTable(
                name: "Exams",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PocessingExamDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "FORMAT (getutcdate(), 'yyyy-MM-dd')"),
                    CompleteResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_CompleteResult_CompleteResultId",
                        column: x => x.CompleteResultId,
                        principalSchema: "LE",
                        principalTable: "CompleteResult",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exams_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "LE",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

           
            migrationBuilder.CreateTable(
                name: "PrimitiveResult",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    IsUnanswer = table.Column<bool>(type: "bit", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YourAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimitiveResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimitiveResult_Exams_ExamsId",
                        column: x => x.ExamsId,
                        principalSchema: "LE",
                        principalTable: "Exams",
                        principalColumn: "Id");
                });          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationVersions",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "PrimitiveResult",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "Ticket",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "Words",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "Exams",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "VerbTenses",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "WordTypes",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "CompleteResult",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "LE");
        }
    }
}
