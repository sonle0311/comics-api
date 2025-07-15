using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainCdnToChapterImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomainCdn",
                table: "ChapterImages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomainCdn",
                table: "ChapterImages");
        }
    }
}
