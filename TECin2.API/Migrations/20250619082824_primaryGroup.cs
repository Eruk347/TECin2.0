using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TECin2.API.Migrations
{
    /// <inheritdoc />
    public partial class primaryGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrimaryGroupId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ComputerLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovedComputers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MACaddress = table.Column<string>(type: "nvarchar(17)", nullable: false),
                    ComputerLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovedComputers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovedComputers_ComputerLocation_ComputerLocationId",
                        column: x => x.ComputerLocationId,
                        principalTable: "ComputerLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedComputers_ComputerLocationId",
                table: "ApprovedComputers",
                column: "ComputerLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovedComputers");

            migrationBuilder.DropTable(
                name: "ComputerLocation");

            migrationBuilder.DropColumn(
                name: "PrimaryGroupId",
                table: "User");
        }
    }
}
