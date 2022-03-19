using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddNewWordTypes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Noun");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Letters");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Pronoun");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "Adverb");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Type",
                value: "Verb");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Type",
                value: "Adjective");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Type",
                value: "Undefined");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Undefined");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Noun");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Letters");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "Pronoun");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Type",
                value: "Adverb");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Type",
                value: "Verb");

            migrationBuilder.UpdateData(
                schema: "LE",
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Type",
                value: "Adjective");
        }
    }
}
