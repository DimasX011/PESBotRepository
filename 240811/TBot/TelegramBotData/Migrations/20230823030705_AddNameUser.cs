using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotData.Migrations
{
    /// <inheritdoc />
    public partial class AddNameUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "userData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "userData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "userData");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "userData");
        }
    }
}
