using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTech.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateOnlyRangeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dates",
                table: "Position");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Position",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Position",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Position");

            migrationBuilder.AddColumn<string>(
                name: "Dates",
                table: "Position",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
