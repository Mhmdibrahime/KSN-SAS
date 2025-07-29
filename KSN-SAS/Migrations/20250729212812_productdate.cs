using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSN_SAS.Migrations
{
    /// <inheritdoc />
    public partial class productdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CteationDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CteationDate",
                table: "Products");
        }
    }
}
