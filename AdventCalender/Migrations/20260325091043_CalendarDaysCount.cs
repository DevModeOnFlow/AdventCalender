using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdventCalender.Migrations
{
    /// <inheritdoc />
    public partial class CalendarDaysCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalendarDaysCount",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalendarDaysCount",
                table: "AspNetUsers");
        }
    }
}
