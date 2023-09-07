using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "taskData",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaskStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskData", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "taskUsers",
                columns: table => new
                {
                    TaskUsersId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaskNumberPhone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskUsers", x => x.TaskUsersId);
                });

            migrationBuilder.CreateTable(
                name: "unRegisteredUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberPhone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    ExpDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unRegisteredUser", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "userData",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserINN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberPhone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    CodeZup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    ExpDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userData", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "usersDataEntry",
                columns: table => new
                {
                    UsersDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberPhone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    LoginBitrix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoginYandex = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoginMail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoginServer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordBitrix = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PasswordYandex = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PasswordMail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PasswordServer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersDataEntry", x => x.UsersDataId);
                });

            migrationBuilder.CreateTable(
                name: "userYandex",
                columns: table => new
                {
                    UserYandexIdToDataBase = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdYandex = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userYandex", x => x.UserYandexIdToDataBase);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "taskData");

            migrationBuilder.DropTable(
                name: "taskUsers");

            migrationBuilder.DropTable(
                name: "unRegisteredUser");

            migrationBuilder.DropTable(
                name: "userData");

            migrationBuilder.DropTable(
                name: "usersDataEntry");

            migrationBuilder.DropTable(
                name: "userYandex");
        }
    }
}
