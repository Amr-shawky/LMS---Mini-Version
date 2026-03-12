using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS___Mini_Version.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstrain_Intern_Track : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollments_InternId",
                table: "Enrollments");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_InternId_TrackId",
                table: "Enrollments",
                columns: new[] { "InternId", "TrackId" },
                unique: true,
                filter: "[Status] != 'Cancelled'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollments_InternId_TrackId",
                table: "Enrollments");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_InternId",
                table: "Enrollments",
                column: "InternId");
        }
    }
}
