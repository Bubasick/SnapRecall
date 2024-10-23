using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapRecall.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLastCommandToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastExecutedCommand",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastExecutedCommand",
                table: "Users");
        }
    }
}
