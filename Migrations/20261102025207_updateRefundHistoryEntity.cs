using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS___Mini_Version.Migrations
{
    /// <inheritdoc />
    public partial class updateRefundHistoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "refundHistories",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "refundHistories",
                newName: "CreateAt");
        }
    }
}
