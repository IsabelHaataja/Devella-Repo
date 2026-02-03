using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devella.DataAccessLayer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedDateDateOnlyToUserAndListOfDevIdsToCompanyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeveloperUsers_CompanyUsers_CompanyUserId",
                table: "DeveloperUsers");

            migrationBuilder.DropIndex(
                name: "IX_DeveloperUsers_CompanyUserId",
                table: "DeveloperUsers");

            migrationBuilder.DropColumn(
                name: "CompanyUserId",
                table: "DeveloperUsers");

            migrationBuilder.AddColumn<string>(
                name: "DeveloperIds",
                table: "CompanyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Created",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeveloperIds",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CompanyUserId",
                table: "DeveloperUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeveloperUsers_CompanyUserId",
                table: "DeveloperUsers",
                column: "CompanyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeveloperUsers_CompanyUsers_CompanyUserId",
                table: "DeveloperUsers",
                column: "CompanyUserId",
                principalTable: "CompanyUsers",
                principalColumn: "Id");
        }
    }
}
