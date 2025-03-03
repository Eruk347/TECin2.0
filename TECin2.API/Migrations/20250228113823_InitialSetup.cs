using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TECin2.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(350)", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    User = table.Column<string>(type: "nvarchar(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Deactivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Deactivated = table.Column<bool>(type: "bit", nullable: false),
                    Principal = table.Column<string>(type: "nvarchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Deactivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkHoursInDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Monday = table.Column<TimeOnly>(type: "time", nullable: false),
                    Tuesday = table.Column<TimeOnly>(type: "time", nullable: false),
                    Wednesday = table.Column<TimeOnly>(type: "time", nullable: false),
                    Thursday = table.Column<TimeOnly>(type: "time", nullable: false),
                    Friday = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkHoursInDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Phonenumber = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    Deactivated = table.Column<bool>(type: "bit", nullable: false),
                    IsStudent = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    LastCheckin = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Deactivated = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentHead = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckInStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    ArrivalDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ArrivalTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Departure = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckInStatus_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SettingUser",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingUser", x => new { x.SettingsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SettingUser_Setting_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "Setting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SettingUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Deactivated = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    WorkHoursInDayId = table.Column<int>(type: "int", nullable: true),
                    FlexibleArrivalEnabled = table.Column<bool>(type: "bit", nullable: false),
                    FlexibleAmount = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsLateBuffer = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsLateMessage = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_WorkHoursInDay_WorkHoursInDayId",
                        column: x => x.WorkHoursInDayId,
                        principalTable: "WorkHoursInDay",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Group_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckInStatus_User_Id",
                table: "CheckInStatus",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Department_SchoolId",
                table: "Department",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_DepartmentId",
                table: "Group",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_WorkHoursInDayId",
                table: "Group",
                column: "WorkHoursInDayId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UsersId",
                table: "GroupUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingUser_UsersId",
                table: "SettingUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckInStatus");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "SettingUser");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "WorkHoursInDay");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "School");
        }
    }
}
