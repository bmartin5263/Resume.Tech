using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTech.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddJobLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Job",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Job");
        }
    }
}
