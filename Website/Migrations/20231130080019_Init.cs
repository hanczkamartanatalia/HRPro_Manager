using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_Categories_Id_Category",
                table: "WorkTimes");

            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_Id_Category",
                table: "WorkTimes");

            migrationBuilder.DropColumn(
                name: "Id_Category",
                table: "WorkTimes");

            migrationBuilder.AlterColumn<decimal>(
                name: "WorkingHours",
                table: "WorkTimes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WorkingHours",
                table: "WorkTimes",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "Id_Category",
                table: "WorkTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_Id_Category",
                table: "WorkTimes",
                column: "Id_Category");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_Categories_Id_Category",
                table: "WorkTimes",
                column: "Id_Category",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
