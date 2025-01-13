using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace StudentManagementSystem.Migrations
{
    public partial class UpdateMastersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // If you need to add new columns to the Masters table, do it here.
            // Example: Add a new column called "Email".
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Masters",
                maxLength: 100,
                nullable: true); // Allow null values for existing rows
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the changes made in the Up method.
            // Example: Drop the "Email" column.
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Masters");
        }
    }
}