using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examiners_Users_UserId",
                table: "Examiners");

            migrationBuilder.DropIndex(
                name: "IX_Examiners_UserId",
                table: "Examiners");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a8bd976-c7a5-43cf-996f-c4c14da4ac1c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c442c60-5fd2-4051-a706-7cd15b6ab9e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cce6b340-fbc8-42fd-b66d-e3a29fdd0b05");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9686f29-e4e1-4def-9b1d-1722f3bf7ce3");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Examiners");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Stages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Enrollments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Stages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Examiners",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a8bd976-c7a5-43cf-996f-c4c14da4ac1c", null, "Admin", "ADMIN" },
                    { "2c442c60-5fd2-4051-a706-7cd15b6ab9e5", null, "Examiner", "EXAMINER" },
                    { "cce6b340-fbc8-42fd-b66d-e3a29fdd0b05", null, "Applicant", "APPLICANT" },
                    { "e9686f29-e4e1-4def-9b1d-1722f3bf7ce3", null, "SeniorExaminer", "SENIOREXAMINER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Examiners_UserId",
                table: "Examiners",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Examiners_Users_UserId",
                table: "Examiners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
