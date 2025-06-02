using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangOut.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Change_Type_Of_Time_In_Table_PlanItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Time",
                table: "PlanItem",
                type: "time",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "PlanItem",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldMaxLength: 100);
        }
    }
}
