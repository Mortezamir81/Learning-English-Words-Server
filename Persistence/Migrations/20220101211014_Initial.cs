using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LE");

            migrationBuilder.CreateTable(
                name: "ApplicationVersions",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompleteResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionsCount = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswersCount = table.Column<int>(type: "int", nullable: false),
                    IncorrectAnswersCount = table.Column<int>(type: "int", nullable: false),
                    UnanswerCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerbTenses",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tense = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerbTenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordTypes",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPhoneNumber = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TimeRegistered = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    TimeUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SecurityStamp = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "LE",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
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
                        principalTable: "CompleteResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "LE",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SentDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    Direction = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "ltr"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "LE",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "LE",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "LE",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                schema: "LE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerb = table.Column<bool>(type: "bit", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WordTypeId = table.Column<int>(type: "int", nullable: false),
                    VerbTenseId = table.Column<int>(type: "int", nullable: false),
                    LearningDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    PersianTranslation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishTranslation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "LE",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Words_VerbTenses_VerbTenseId",
                        column: x => x.VerbTenseId,
                        principalSchema: "LE",
                        principalTable: "VerbTenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Words_WordTypes_WordTypeId",
                        column: x => x.WordTypeId,
                        principalSchema: "LE",
                        principalTable: "WordTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimitiveResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YourAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    IsUnanswer = table.Column<bool>(type: "bit", nullable: false),
                    ExamsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimitiveResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimitiveResult_Exams_ExamsId",
                        column: x => x.ExamsId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "LE",
                table: "ApplicationVersions",
                columns: new[] { "Id", "Link", "Version" },
                values: new object[] { new Guid("880907b6-5037-4c45-b87f-1f5f547b4468"), "none", "1.0.0.0" });

            migrationBuilder.InsertData(
                schema: "LE",
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Vip" },
                    { 3, "User" }
                });

            migrationBuilder.InsertData(
                schema: "LE",
                table: "VerbTenses",
                columns: new[] { "Id", "Tense" },
                values: new object[,]
                {
                    { 17, "Future Perfect Continuous in the Past" },
                    { 16, "Future Perfect in the Past" },
                    { 15, "Future Continuous in the Past" },
                    { 14, "Future Simple in the Past" },
                    { 13, "Future Perfect Continuous" },
                    { 12, "Future Perfect" },
                    { 11, "Future Continuous" },
                    { 9, "Past Perfect Continuous" },
                    { 10, "Future Simple" },
                    { 7, "Past Continuous" },
                    { 6, "Past Simple" },
                    { 5, "Present Perfect Continuous" },
                    { 4, "Present Perfect" },
                    { 3, "Present Continuous" },
                    { 2, "Present Simple" },
                    { 1, "None" },
                    { 8, "Past Perfect" }
                });

            migrationBuilder.InsertData(
                schema: "LE",
                table: "WordTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 5, "Verb" },
                    { 1, "Noun" },
                    { 2, "Letters" },
                    { 3, "Pronoun" },
                    { 4, "Adverb" },
                    { 6, "Adjective" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CompleteResultId",
                table: "Exams",
                column: "CompleteResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_UserId",
                table: "Exams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "LE",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimitiveResult_ExamsId",
                table: "PrimitiveResult",
                column: "ExamsId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserId",
                schema: "LE",
                table: "Ticket",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "LE",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "LE",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "LE",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_UserId",
                schema: "LE",
                table: "Words",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_VerbTenseId",
                schema: "LE",
                table: "Words",
                column: "VerbTenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_WordTypeId",
                schema: "LE",
                table: "Words",
                column: "WordTypeId");
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
                name: "PrimitiveResult");

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
                name: "Exams");

            migrationBuilder.DropTable(
                name: "VerbTenses",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "WordTypes",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "CompleteResult");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "LE");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "LE");
        }
    }
}
