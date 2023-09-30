using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTech.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_Profile_ProfileId",
                table: "ContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_Profile_ProfileId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Profile_ProfileId",
                table: "Job");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Job_ProfileId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Education_ProfileId",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Education");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "ContactInfo",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactInfo_ProfileId",
                table: "ContactInfo",
                newName: "IX_ContactInfo_OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_OwnerId",
                table: "Job",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_OwnerId",
                table: "Education",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_User_OwnerId",
                table: "ContactInfo",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_User_OwnerId",
                table: "Education",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_User_OwnerId",
                table: "Job",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_User_OwnerId",
                table: "ContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_User_OwnerId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_User_OwnerId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Job_OwnerId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Education_OwnerId",
                table: "Education");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "ContactInfo",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactInfo_OwnerId",
                table: "ContactInfo",
                newName: "IX_ContactInfo_ProfileId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Job",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Education",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.ProfileId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Job_ProfileId",
                table: "Job",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_ProfileId",
                table: "Education",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_Profile_ProfileId",
                table: "ContactInfo",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_Profile_ProfileId",
                table: "Education",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Profile_ProfileId",
                table: "Job",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
