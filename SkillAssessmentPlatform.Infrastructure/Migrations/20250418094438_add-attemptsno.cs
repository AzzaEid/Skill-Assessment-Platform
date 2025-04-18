using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addattemptsno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68e6ea95-e9d1-4b9e-a40f-2bae397de7b4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c857971-7ce9-406e-9f30-d757c560640e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "80a68226-59e7-45c4-bec4-587abf0b8625");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cf82805-5baa-4f92-8fd6-ae6ec2c4e05c");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tracks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "NoOfattempts",
                table: "Stages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07fc7761-2eb6-4fad-86bc-f42a3009821c", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "0cdbd9ab-2b35-4792-a5a5-6272936e396a", null, "Admin", "ADMIN" },
                    { "524f6eaf-f322-4c57-8414-391b880f3661", null, "Applicant", "APPLICANT" },
                    { "82f5fc09-a998-4bd2-953d-701c909e89cb", null, "Examiner", "EXAMINER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07fc7761-2eb6-4fad-86bc-f42a3009821c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cdbd9ab-2b35-4792-a5a5-6272936e396a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "524f6eaf-f322-4c57-8414-391b880f3661");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82f5fc09-a998-4bd2-953d-701c909e89cb");

            migrationBuilder.DropColumn(
                name: "NoOfattempts",
                table: "Stages");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tracks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "68e6ea95-e9d1-4b9e-a40f-2bae397de7b4", null, "Admin", "ADMIN" },
                    { "7c857971-7ce9-406e-9f30-d757c560640e", null, "Examiner", "EXAMINER" },
                    { "80a68226-59e7-45c4-bec4-587abf0b8625", null, "Applicant", "APPLICANT" },
                    { "9cf82805-5baa-4f92-8fd6-ae6ec2c4e05c", null, "SeniorExaminer", "SENIOREXAMINER" }
                });
        }
    }
}
