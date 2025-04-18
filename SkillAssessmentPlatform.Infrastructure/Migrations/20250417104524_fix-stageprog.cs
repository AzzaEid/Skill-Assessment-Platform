using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixstageprog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2559a1b5-de86-4ec1-8c90-3ff00df02c70");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a988fe4d-de6a-4872-9470-9673ddc79b96");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df121f77-d94f-424a-ada3-ee873900db49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f52f0eb3-149e-49ba-b6f0-c9ffc918e3db");

            migrationBuilder.DropColumn(
                name: "EnrollmentId",
                table: "StageProgresses");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e4524b0-d3bf-464b-937b-517897d7156b", null, "Applicant", "APPLICANT" },
                    { "76980fb9-06a6-478c-aa32-92ef7e658344", null, "Examiner", "EXAMINER" },
                    { "77e28085-0519-4d92-9294-9c6fca27a279", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "d42be8c0-b853-426c-8214-57b02041868c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e4524b0-d3bf-464b-937b-517897d7156b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76980fb9-06a6-478c-aa32-92ef7e658344");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77e28085-0519-4d92-9294-9c6fca27a279");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d42be8c0-b853-426c-8214-57b02041868c");

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentId",
                table: "StageProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2559a1b5-de86-4ec1-8c90-3ff00df02c70", null, "Examiner", "EXAMINER" },
                    { "a988fe4d-de6a-4872-9470-9673ddc79b96", null, "Applicant", "APPLICANT" },
                    { "df121f77-d94f-424a-ada3-ee873900db49", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "f52f0eb3-149e-49ba-b6f0-c9ffc918e3db", null, "Admin", "ADMIN" }
                });
        }
    }
}
