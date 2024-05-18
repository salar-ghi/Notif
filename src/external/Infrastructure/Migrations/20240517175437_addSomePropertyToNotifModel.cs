using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSomePropertyToNotifModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 17, 17, 54, 35, 896, DateTimeKind.Utc).AddTicks(6995),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 15, 8, 40, 7, 47, DateTimeKind.Utc).AddTicks(4290));

            migrationBuilder.AddColumn<int>(
                name: "Attemp",
                table: "Notifs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attemp",
                table: "Notifs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 15, 8, 40, 7, 47, DateTimeKind.Utc).AddTicks(4290),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 17, 17, 54, 35, 896, DateTimeKind.Utc).AddTicks(6995));
        }
    }
}
