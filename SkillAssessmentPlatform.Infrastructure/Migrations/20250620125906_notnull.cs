using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class notnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskApplicants_StageProgressId",
                table: "TaskApplicants");

            migrationBuilder.AlterColumn<int>(
                name: "StageProgressId",
                table: "TaskApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskApplicants_StageProgressId",
                table: "TaskApplicants",
                column: "StageProgressId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskApplicants_StageProgressId",
                table: "TaskApplicants");

            migrationBuilder.AlterColumn<int>(
                name: "StageProgressId",
                table: "TaskApplicants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TaskApplicants_StageProgressId",
                table: "TaskApplicants",
                column: "StageProgressId",
                unique: true,
                filter: "[StageProgressId] IS NOT NULL");
        }
    }
}
