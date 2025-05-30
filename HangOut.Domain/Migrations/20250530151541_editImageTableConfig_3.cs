using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangOut.Domain.Migrations
{
    /// <inheritdoc />
    public partial class editImageTableConfig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Image_ObjectId",
                table: "Image",
                column: "ObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Business_ObjectId",
                table: "Image",
                column: "ObjectId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Event_ObjectId",
                table: "Image",
                column: "ObjectId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Business_ObjectId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Event_ObjectId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_ObjectId",
                table: "Image");
        }
    }
}
