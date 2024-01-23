using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCatDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAccess",
                table: "PermissionsGrid");

            migrationBuilder.AlterColumn<string>(
                name: "Page",
                table: "PermissionsGrid",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeCatDate",
                table: "Applications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeCatDate",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "Page",
                table: "PermissionsGrid",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "HasAccess",
                table: "PermissionsGrid",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
