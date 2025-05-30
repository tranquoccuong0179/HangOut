using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangOut.Domain.Migrations
{
    /// <inheritdoc />
    public partial class editImageTableConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Image",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Image",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_BusinessId",
                table: "Image",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_EventId",
                table: "Image",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Business_BusinessId",
                table: "Image",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Event_EventId",
                table: "Image",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Business_BusinessId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Event_EventId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_BusinessId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_EventId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Image");

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
    }
}
