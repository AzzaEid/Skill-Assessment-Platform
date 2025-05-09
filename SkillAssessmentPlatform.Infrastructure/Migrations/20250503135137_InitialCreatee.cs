using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b8c6b1b-96e6-4534-bcf5-080ef874d877");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33c59dcc-b9fb-441f-b981-357e173580b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "551beece-77a4-4c4c-9847-ff884f995278");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "670cc7a5-417f-469f-aa7c-18ab00a54f08");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b8c6b1b-96e6-4534-bcf5-080ef874d877", null, "Examiner", "EXAMINER" },
                    { "33c59dcc-b9fb-441f-b981-357e173580b9", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "551beece-77a4-4c4c-9847-ff884f995278", null, "Admin", "ADMIN" },
                    { "670cc7a5-417f-469f-aa7c-18ab00a54f08", null, "Applicant", "APPLICANT" }
                });
        }
    }
}
