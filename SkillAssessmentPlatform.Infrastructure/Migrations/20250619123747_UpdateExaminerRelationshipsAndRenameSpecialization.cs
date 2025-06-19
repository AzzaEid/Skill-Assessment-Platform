using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExaminerRelationshipsAndRenameSpecialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Examiners");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Examiners",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks",
                column: "SeniorExaminerID",
                principalTable: "Examiners",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Examiners");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Examiners",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Examiners_SeniorExaminerID",
                table: "Tracks",
                column: "SeniorExaminerID",
                principalTable: "Examiners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
