using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlackList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MessageType = table.Column<byte>(type: "tinyint", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    HangfireJobId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    NextTry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 15, 7, 23, 46, 446, DateTimeKind.Utc).AddTicks(6193)),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    JsonConfig = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipient",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotifId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipient_Notifs_NotifId",
                        column: x => x.NotifId,
                        principalTable: "Notifs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotifId = table.Column<long>(type: "bigint", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotifLog_Notifs_NotifId",
                        column: x => x.NotifId,
                        principalTable: "Notifs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifLog_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotifLog_NotifId",
                table: "NotifLog",
                column: "NotifId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotifLog_ProviderId",
                table: "NotifLog",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_NotifId",
                table: "Recipient",
                column: "NotifId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackList");

            migrationBuilder.DropTable(
                name: "NotifLog");

            migrationBuilder.DropTable(
                name: "Recipient");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "Notifs");
        }
    }
}
