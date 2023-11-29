using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Namee",
                table: "Users",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "Id_Category",
                table: "WorkTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "LoginData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_Id_Category",
                table: "WorkTimes",
                column: "Id_Category");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTimes_Category_Id_Category",
                table: "WorkTimes",
                column: "Id_Category",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTimes_Category_Id_Category",
                table: "WorkTimes");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_Id_Category",
                table: "WorkTimes");

            migrationBuilder.DropColumn(
                name: "Id_Category",
                table: "WorkTimes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Namee");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "LoginData",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
