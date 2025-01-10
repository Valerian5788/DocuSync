using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocuSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentTrackingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobId",
                table: "Requirements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UploadedAt",
                table: "Requirements",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobId",
                table: "Requirements");

            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "Requirements");
        }
    }
}
