using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotData.Migrations
{
    /// <inheritdoc />
    public partial class adminfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "userData",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "userData");
        }
    }
}
