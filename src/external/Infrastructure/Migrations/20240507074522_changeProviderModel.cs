using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeProviderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Providers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 7, 7, 45, 22, 119, DateTimeKind.Utc).AddTicks(8517),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 1, 10, 44, 18, 444, DateTimeKind.Utc).AddTicks(6087));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Providers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 1, 10, 44, 18, 444, DateTimeKind.Utc).AddTicks(6087),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 7, 7, 45, 22, 119, DateTimeKind.Utc).AddTicks(8517));
        }
    }
}
