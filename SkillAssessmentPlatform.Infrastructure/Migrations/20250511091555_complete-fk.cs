using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class completefk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Interviews");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicantId",
                table: "InterviewBooks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicantId",
                table: "ExamRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewBooks_ApplicantId",
                table: "InterviewBooks",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRequests_ApplicantId",
                table: "ExamRequests",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamRequests_Applicants_ApplicantId",
                table: "ExamRequests",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewBooks_Applicants_ApplicantId",
                table: "InterviewBooks",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamRequests_Applicants_ApplicantId",
                table: "ExamRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewBooks_Applicants_ApplicantId",
                table: "InterviewBooks");

            migrationBuilder.DropIndex(
                name: "IX_InterviewBooks_ApplicantId",
                table: "InterviewBooks");

            migrationBuilder.DropIndex(
                name: "IX_ExamRequests_ApplicantId",
                table: "ExamRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Interviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicantId",
                table: "InterviewBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicantId",
                table: "ExamRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
