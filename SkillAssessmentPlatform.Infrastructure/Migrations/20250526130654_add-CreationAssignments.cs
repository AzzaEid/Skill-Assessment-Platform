using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillAssessmentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCreationAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CreationAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExaminerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBySeniorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreationAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreationAssignments_Examiners_AssignedBySeniorId",
                        column: x => x.AssignedBySeniorId,
                        principalTable: "Examiners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreationAssignments_Examiners_ExaminerId",
                        column: x => x.ExaminerId,
                        principalTable: "Examiners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreationAssignments_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_CreationAssignments_AssignedBySeniorId",
                table: "CreationAssignments",
                column: "AssignedBySeniorId");

            migrationBuilder.CreateIndex(
                name: "IX_CreationAssignments_ExaminerId",
                table: "CreationAssignments",
                column: "ExaminerId");

            migrationBuilder.CreateIndex(
                name: "IX_CreationAssignments_StageId",
                table: "CreationAssignments",
                column: "StageId");



        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CreationAssignments");
        }
    }
}
