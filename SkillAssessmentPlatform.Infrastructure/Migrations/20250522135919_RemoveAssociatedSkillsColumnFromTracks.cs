using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAssociatedSkillsColumnFromTracks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // حذف العمود AssociatedSkills من جدول Tracks فقط
            migrationBuilder.DropColumn(
                name: "AssociatedSkills",
                table: "Tracks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // إعادة إضافة العمود (إذا كنت تريد التراجع)
            migrationBuilder.AddColumn<string>(
                name: "AssociatedSkills",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
