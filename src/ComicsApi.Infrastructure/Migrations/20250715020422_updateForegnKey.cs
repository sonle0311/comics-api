using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateForegnKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Mangas_MangaId1",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_MangaId1",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "MangaId1",
                table: "Chapters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MangaId1",
                table: "Chapters",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_MangaId1",
                table: "Chapters",
                column: "MangaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Mangas_MangaId1",
                table: "Chapters",
                column: "MangaId1",
                principalTable: "Mangas",
                principalColumn: "Id");
        }
    }
}
