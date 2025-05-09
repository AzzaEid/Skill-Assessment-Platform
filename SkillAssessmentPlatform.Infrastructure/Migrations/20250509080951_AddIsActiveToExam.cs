using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07696748-fd20-4e3d-b693-db7b85e9dcbe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31d93f16-1c37-4863-b93a-1f9a37a4a254");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c0df495-9de3-49c0-b5a6-3c8824400955");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8575285d-95f1-4f37-a57c-8c33d51710c2");

            migrationBuilder.RenameColumn(
                name: "NoOfattempts",
                table: "Stages",
                newName: "NoOfAttempts");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "EvaluationCriteria",
                newName: "IsActive");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24373dbf-75b7-4398-b802-5941037b69e4", null, "Applicant", "APPLICANT" },
                    { "56574ea8-b54d-4a6c-a3f2-ffe4709411b3", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "a322ebd2-1eeb-4aab-b9ef-a95772f38b1f", null, "Examiner", "EXAMINER" },
                    { "b4032537-a9f6-4cd0-a3dc-5daa7e0dbea1", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24373dbf-75b7-4398-b802-5941037b69e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56574ea8-b54d-4a6c-a3f2-ffe4709411b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a322ebd2-1eeb-4aab-b9ef-a95772f38b1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4032537-a9f6-4cd0-a3dc-5daa7e0dbea1");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "NoOfAttempts",
                table: "Stages",
                newName: "NoOfattempts");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "EvaluationCriteria",
                newName: "isActive");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07696748-fd20-4e3d-b693-db7b85e9dcbe", null, "Admin", "ADMIN" },
                    { "31d93f16-1c37-4863-b93a-1f9a37a4a254", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "5c0df495-9de3-49c0-b5a6-3c8824400955", null, "Examiner", "EXAMINER" },
                    { "8575285d-95f1-4f37-a57c-8c33d51710c2", null, "Applicant", "APPLICANT" }
                });
        }
    }
}
