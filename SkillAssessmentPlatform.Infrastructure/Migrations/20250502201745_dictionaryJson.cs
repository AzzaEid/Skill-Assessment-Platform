using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dictionaryJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "64a3f2a3-ac2d-4cad-a60f-8164bd95818c", null, "Admin", "ADMIN" },
                    { "a8fcecd4-2807-4fbb-902c-020fd49dcbd0", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "b3f1f931-4026-4c1b-893d-7fa57d2919ec", null, "Examiner", "EXAMINER" },
                    { "fae275f3-58b1-43ff-868d-9f1242bc8084", null, "Applicant", "APPLICANT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64a3f2a3-ac2d-4cad-a60f-8164bd95818c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8fcecd4-2807-4fbb-902c-020fd49dcbd0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3f1f931-4026-4c1b-893d-7fa57d2919ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fae275f3-58b1-43ff-868d-9f1242bc8084");

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
    }
}
