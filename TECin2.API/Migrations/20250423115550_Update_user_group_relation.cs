using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TECin2.API.Migrations
{
    /// <inheritdoc />
    public partial class Update_user_group_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "User",
                type: "nvarchar(36)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Role",
                type: "nvarchar(32)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Role",
                type: "nvarchar(250)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "User",
                type: "nvarchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Role",
                type: "nvarchar(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Role",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)");
        }
    }
}
