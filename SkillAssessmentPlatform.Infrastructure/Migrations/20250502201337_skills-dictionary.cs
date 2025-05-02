using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class skillsdictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "186e2534-cadf-4caf-811a-8a86c1913128");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f57e3da-3366-4903-a599-5b6ca7679f26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3eb98afe-97db-446e-af9d-8589fc1fe34a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5683f58f-38e1-4f9d-a904-e060d08f12b5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12572d86-a048-4cd6-999e-ecde7e456220", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "4299ff4e-4342-4bd1-81c2-556e4c3109f7", null, "Applicant", "APPLICANT" },
                    { "6a54ff2c-0865-41ba-a1d3-47e9d332a997", null, "Examiner", "EXAMINER" },
                    { "c97e60a8-6529-4fb1-8f95-f6462485bc65", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12572d86-a048-4cd6-999e-ecde7e456220");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4299ff4e-4342-4bd1-81c2-556e4c3109f7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a54ff2c-0865-41ba-a1d3-47e9d332a997");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c97e60a8-6529-4fb1-8f95-f6462485bc65");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "186e2534-cadf-4caf-811a-8a86c1913128", null, "Examiner", "EXAMINER" },
                    { "2f57e3da-3366-4903-a599-5b6ca7679f26", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "3eb98afe-97db-446e-af9d-8589fc1fe34a", null, "Applicant", "APPLICANT" },
                    { "5683f58f-38e1-4f9d-a904-e060d08f12b5", null, "Admin", "ADMIN" }
                });
        }
    }
}
