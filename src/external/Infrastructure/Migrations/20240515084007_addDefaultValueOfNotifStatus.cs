using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDefaultValueOfNotifStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 15, 8, 40, 7, 47, DateTimeKind.Utc).AddTicks(4290),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 15, 7, 23, 46, 446, DateTimeKind.Utc).AddTicks(6193));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 15, 7, 23, 46, 446, DateTimeKind.Utc).AddTicks(6193),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 15, 8, 40, 7, 47, DateTimeKind.Utc).AddTicks(4290));
        }
    }
}
