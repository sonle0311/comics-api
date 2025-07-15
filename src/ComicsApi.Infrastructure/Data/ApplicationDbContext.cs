using ComicsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Infrastructure.Data
{
    /// <summary>
    /// DbContext chính của ứng dụng
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Manga> Mangas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ChapterImage> ChapterImages { get; set; }
        public DbSet<SeoMeta> SeoMetas { get; set; }
        public DbSet<CrawlLog> CrawlLogs { get; set; }
        public DbSet<ChapterLog> ChapterLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình các entity
            modelBuilder.Entity<Manga>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Slug).IsRequired();
                entity.HasIndex(e => e.Slug).IsUnique();
                
                // Quan hệ 1-1 với SeoMeta
                entity.HasOne(e => e.Seo)
                    .WithOne()
                    .HasForeignKey<SeoMeta>("MangaId");

                // Chuyển đổi List<string> thành JSON
                entity.Property(e => e.OriginNames).HasColumnType("jsonb");
                entity.Property(e => e.Authors).HasColumnType("jsonb");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Slug).IsRequired();
                entity.HasIndex(e => e.Slug).IsUnique();

                // Quan hệ nhiều-nhiều với Manga
                entity.HasMany(e => e.Mangas)
                    .WithMany(e => e.Categories)
                    .UsingEntity(j => j.ToTable("MangaCategories"));
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Filename).IsRequired();
                entity.Property(e => e.ChapterName).IsRequired();
                entity.Property(e => e.ChapterApiData).IsRequired();
                
                // Tạo index cho tìm kiếm nhanh theo MangaId + ChapterName
                entity.HasIndex(e => new { e.MangaId, e.ChapterName }).IsUnique();

                // Quan hệ 1-nhiều với Manga
                entity.HasOne(e => e.Manga)
                    .WithMany(e => e.Chapters)
                    .HasForeignKey(e => e.MangaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChapterImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageFile).IsRequired();
                entity.Property(e => e.ChapterPath).IsRequired();
                
                // Tạo index cho tìm kiếm nhanh theo ChapterId + Page
                entity.HasIndex(e => new { e.ChapterId, e.Page }).IsUnique();

                // Quan hệ 1-nhiều với Chapter
                entity.HasOne(e => e.Chapter)
                    .WithMany(e => e.Images)
                    .HasForeignKey(e => e.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SeoMeta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TitleHead).IsRequired();
                entity.Property(e => e.DescriptionHead).IsRequired();
                entity.Property(e => e.OgType).IsRequired();
                
                // Chuyển đổi List<string> thành JSON
                entity.Property(e => e.OgImage).HasColumnType("jsonb");
            });

            modelBuilder.Entity<CrawlLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MangaSlug).IsRequired();
                entity.Property(e => e.MangaName).IsRequired();
                entity.HasIndex(e => e.MangaSlug);
            });

            modelBuilder.Entity<ChapterLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ChapterName).IsRequired();
                entity.Property(e => e.ChapterApiData).IsRequired();
            });
        }
    }
}