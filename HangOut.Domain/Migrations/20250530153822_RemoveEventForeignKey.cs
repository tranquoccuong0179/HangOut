using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangOut.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEventForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                    name: "FK_Image_Event_ObjectId",
                    table: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
