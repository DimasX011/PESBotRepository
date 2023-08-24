using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotData.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumberPhone",
                table: "userData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberPhone",
                table: "userData");
        }
    }
}
