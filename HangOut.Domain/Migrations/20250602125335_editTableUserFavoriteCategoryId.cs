using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangOut.Domain.Migrations
{
    /// <inheritdoc />
    public partial class editTableUserFavoriteCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteCategories",
                table: "UserFavoriteCategories");

            migrationBuilder.DropIndex(
                name: "IX_UserFavoriteCategories_CategoryId",
                table: "UserFavoriteCategories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserFavoriteCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteCategories",
                table: "UserFavoriteCategories",
                columns: new[] { "CategoryId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteCategories",
                table: "UserFavoriteCategories");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserFavoriteCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteCategories",
                table: "UserFavoriteCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteCategories_CategoryId",
                table: "UserFavoriteCategories",
                column: "CategoryId");
        }
    }
}
