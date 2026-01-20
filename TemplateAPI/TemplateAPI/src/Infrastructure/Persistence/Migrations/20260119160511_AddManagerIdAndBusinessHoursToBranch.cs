using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateAPI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerIdAndBusinessHoursToBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessHours",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Branches",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessHours",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Branches");
        }
    }
}
