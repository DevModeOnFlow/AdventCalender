using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdventCalender.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeRangeToAdventDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "AdventDays",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "AdventDays",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "AdventDays");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "AdventDays");
        }
    }
}
