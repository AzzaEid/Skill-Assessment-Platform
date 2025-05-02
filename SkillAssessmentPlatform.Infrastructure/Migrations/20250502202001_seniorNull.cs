using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seniorNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks");

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

            migrationBuilder.AlterColumn<string>(
                name: "SeniorExaminerID",
                table: "Tracks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks",
                column: "SeniorExaminerID",
                principalTable: "Examiners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks");

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

            migrationBuilder.AlterColumn<string>(
                name: "SeniorExaminerID",
                table: "Tracks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks",
                column: "SeniorExaminerID",
                principalTable: "Examiners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
