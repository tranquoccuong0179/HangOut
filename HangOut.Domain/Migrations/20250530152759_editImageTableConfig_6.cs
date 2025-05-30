using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangOut.Domain.Migrations
{
    /// <inheritdoc />
    public partial class editImageTableConfig_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                            name: "EntityType",
                            table: "Image",
                            type: "nvarchar(max)",
                            nullable: false,
                            defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ObjectId",
                table: "Image",
                column: "ObjectId");

            // Chỉ giữ khóa ngoại cho Business
            migrationBuilder.AddForeignKey(
                name: "FK_Image_Business_ObjectId",
                table: "Image",
                column: "ObjectId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Xóa khóa ngoại không cần thiết cho Event
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Event_ObjectId",
                table: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Business_ObjectId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_ObjectId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Image");

            // Thêm lại khóa ngoại cho Event nếu cần rollback
            migrationBuilder.AddForeignKey(
                name: "FK_Image_Event_ObjectId",
                table: "Image",
                column: "ObjectId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
