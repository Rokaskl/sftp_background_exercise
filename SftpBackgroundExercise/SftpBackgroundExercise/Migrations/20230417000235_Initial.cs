using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SftpBackgroundExercise.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileName = table.Column<string>(type: "text", nullable: false),
                    LocalPath = table.Column<string>(type: "text", nullable: false),
                    RemotePath = table.Column<string>(type: "text", nullable: false),
                    MigratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
