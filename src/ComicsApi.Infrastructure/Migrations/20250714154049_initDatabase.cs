using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrawlLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MangaSlug = table.Column<string>(type: "text", nullable: false),
                    MangaName = table.Column<string>(type: "text", nullable: false),
                    TotalChapters = table.Column<int>(type: "integer", nullable: false),
                    ChaptersCrawledSuccess = table.Column<int>(type: "integer", nullable: false),
                    ChaptersCrawledFailed = table.Column<int>(type: "integer", nullable: false),
                    CrawledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawlLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mangas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    OriginNames = table.Column<List<string>>(type: "jsonb", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ThumbUrl = table.Column<string>(type: "text", nullable: false),
                    SubDocQuyen = table.Column<bool>(type: "boolean", nullable: false),
                    Authors = table.Column<List<string>>(type: "jsonb", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mangas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChapterLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterName = table.Column<string>(type: "text", nullable: false),
                    ChapterApiData = table.Column<string>(type: "text", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    CrawlLogId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterLogs_CrawlLogs_CrawlLogId",
                        column: x => x.CrawlLogId,
                        principalTable: "CrawlLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MangaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Filename = table.Column<string>(type: "text", nullable: false),
                    ChapterName = table.Column<string>(type: "text", nullable: false),
                    ChapterTitle = table.Column<string>(type: "text", nullable: true),
                    ChapterApiData = table.Column<string>(type: "text", nullable: false),
                    MangaId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chapters_Mangas_MangaId1",
                        column: x => x.MangaId1,
                        principalTable: "Mangas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MangaCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    MangasId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaCategories", x => new { x.CategoriesId, x.MangasId });
                    table.ForeignKey(
                        name: "FK_MangaCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaCategories_Mangas_MangasId",
                        column: x => x.MangasId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeoMetas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TitleHead = table.Column<string>(type: "text", nullable: false),
                    DescriptionHead = table.Column<string>(type: "text", nullable: false),
                    OgType = table.Column<string>(type: "text", nullable: false),
                    OgImage = table.Column<List<string>>(type: "jsonb", nullable: false),
                    OgUrl = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<long>(type: "bigint", nullable: true),
                    MangaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeoMetas_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChapterImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Page = table.Column<int>(type: "integer", nullable: false),
                    ImageFile = table.Column<string>(type: "text", nullable: false),
                    ChapterPath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterImages_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterImages_ChapterId_Page",
                table: "ChapterImages",
                columns: new[] { "ChapterId", "Page" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterLogs_CrawlLogId",
                table: "ChapterLogs",
                column: "CrawlLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_MangaId_ChapterName",
                table: "Chapters",
                columns: new[] { "MangaId", "ChapterName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_MangaId1",
                table: "Chapters",
                column: "MangaId1");

            migrationBuilder.CreateIndex(
                name: "IX_CrawlLogs_MangaSlug",
                table: "CrawlLogs",
                column: "MangaSlug");

            migrationBuilder.CreateIndex(
                name: "IX_MangaCategories_MangasId",
                table: "MangaCategories",
                column: "MangasId");

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_Slug",
                table: "Mangas",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeoMetas_MangaId",
                table: "SeoMetas",
                column: "MangaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterImages");

            migrationBuilder.DropTable(
                name: "ChapterLogs");

            migrationBuilder.DropTable(
                name: "MangaCategories");

            migrationBuilder.DropTable(
                name: "SeoMetas");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "CrawlLogs");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Mangas");
        }
    }
}
