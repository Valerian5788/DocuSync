using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocuSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPortalCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId1",
                table: "Requirements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PortalCredentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PortalType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EncryptedPassword = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortalCredentials_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_DocumentTypeId1",
                table: "Requirements",
                column: "DocumentTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_PortalCredentials_ClientId",
                table: "PortalCredentials",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_DocumentTypes_DocumentTypeId1",
                table: "Requirements",
                column: "DocumentTypeId1",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_DocumentTypes_DocumentTypeId1",
                table: "Requirements");

            migrationBuilder.DropTable(
                name: "PortalCredentials");

            migrationBuilder.DropIndex(
                name: "IX_Requirements_DocumentTypeId1",
                table: "Requirements");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId1",
                table: "Requirements");
        }
    }
}
