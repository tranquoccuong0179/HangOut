using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangOut.Domain.Migrations
{
    /// <inheritdoc />
    public partial class editTableAccountVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountVoucher",
                table: "AccountVoucher");

            migrationBuilder.DropIndex(
                name: "IX_AccountVoucher_AccountId",
                table: "AccountVoucher");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AccountVoucher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountVoucher",
                table: "AccountVoucher",
                columns: new[] { "AccountId", "VoucherId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountVoucher",
                table: "AccountVoucher");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AccountVoucher",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountVoucher",
                table: "AccountVoucher",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccountVoucher_AccountId",
                table: "AccountVoucher",
                column: "AccountId");
        }
    }
}
