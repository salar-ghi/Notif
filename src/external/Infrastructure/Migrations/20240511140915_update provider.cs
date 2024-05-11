using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateprovider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Providers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Providers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Providers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Priority",
                table: "Providers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 11, 14, 9, 15, 393, DateTimeKind.Utc).AddTicks(5310),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 7, 7, 45, 22, 119, DateTimeKind.Utc).AddTicks(8517));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Providers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 5, 7, 7, 45, 22, 119, DateTimeKind.Utc).AddTicks(8517),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 5, 11, 14, 9, 15, 393, DateTimeKind.Utc).AddTicks(5310));
        }
    }
}
