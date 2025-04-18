using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProgressStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "183a5882-5c4d-4931-9857-bee593c05796");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30b7692e-e594-4893-bcf0-aa59d877d63f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "357d5151-ba42-40fd-b143-33589d691514");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0e1d467-f09d-4165-a796-ad3e412fbd0c");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "183a5882-5c4d-4931-9857-bee593c05796", null, "Examiner", "EXAMINER" },
                    { "30b7692e-e594-4893-bcf0-aa59d877d63f", null, "SeniorExaminer", "SENIOREXAMINER" },
                    { "357d5151-ba42-40fd-b143-33589d691514", null, "Applicant", "APPLICANT" },
                    { "e0e1d467-f09d-4165-a796-ad3e412fbd0c", null, "Admin", "ADMIN" }
                });
        }
    }
}
