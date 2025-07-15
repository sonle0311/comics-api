using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ComicsApi.Application.Features.Crawler.Commands;
using ComicsApi.Application.Interfaces;
using ComicsApi.Domain.Entities;
using ComicsApi.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ComicsApi.Infrastructure.Services
{
    /// <summary>
    /// Service crawl dữ liệu từ Otruyen API
    /// </summary>
    public class CrawlerService : ICrawlerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMangaRepository _mangaRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IChapterImageRepository _chapterImageRepository;
        private readonly ICrawlLogRepository _crawlLogRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CrawlerService> _logger;

        private readonly string _baseUrl;
        private readonly string _thumbnailBaseUrl;
        private readonly string _chapterCdnBaseUrl;

        public CrawlerService(
            IHttpClientFactory httpClientFactory,
            ICategoryRepository categoryRepository,
            IMangaRepository mangaRepository,
            IChapterRepository chapterRepository,
            IChapterImageRepository chapterImageRepository,
            ICrawlLogRepository crawlLogRepository,
            IConfiguration configuration,
            ILogger<CrawlerService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _categoryRepository = categoryRepository;
            _mangaRepository = mangaRepository;
            _chapterRepository = chapterRepository;
            _chapterImageRepository = chapterImageRepository;
            _crawlLogRepository = crawlLogRepository;
            _configuration = configuration;
            _logger = logger;

            _baseUrl = _configuration["OtruyenApi:BaseUrl"];
            _thumbnailBaseUrl = _configuration["Cdn:ThumbnailBaseUrl"];
            _chapterCdnBaseUrl = _configuration["Cdn:ChapterCdnBaseUrl"];
        }

        /// <summary>
        /// Crawl tất cả dữ liệu theo request
        /// </summary>
        public async Task<bool> CrawlAllAsync(CrawlRequest request)
        {
            try
            {
                _logger.LogInformation("Bắt đầu crawl dữ liệu từ trang {PageStart} đến trang {PageEnd}", request.PageStart, request.PageEnd);

                // Bước 1: Crawl danh mục
                await CrawlCategoriesAsync();

                // Bước 2: Crawl danh sách slug từ các trang
                var slugs = await CrawlSlugsFromPagesAsync(request.PageStart, request.PageEnd);
                _logger.LogInformation("Đã tìm thấy {Count} truyện để crawl", slugs.Length);

                // Bước 3: Crawl thông tin manga theo slug
                var mangaSlugs = await CrawlMangasBySlugsAsync(slugs, request);
                _logger.LogInformation("Đã crawl thông tin của {Count} truyện", mangaSlugs.Length);

                // Bước 4: Crawl ảnh của tất cả chapter
                await CrawlAllChapterImagesAsync(mangaSlugs, request);

                _logger.LogInformation("Hoàn thành crawl dữ liệu");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi crawl dữ liệu: {Message}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Crawl danh mục
        /// </summary>
        public async Task<bool> CrawlCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Bắt đầu crawl danh mục");

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<OtruyenCategoryResponse>($"{_baseUrl}/the-loai");

                if (response?.Status != "success" || response.Data?.Items == null)
                {
                    _logger.LogWarning("Không thể lấy danh mục từ API");
                    return false;
                }

                // Lấy tất cả category hiện có để kiểm tra
                var existingCategories = await _categoryRepository.GetAllAsync();
                var existingSlugs = existingCategories.Select(c => c.Slug).ToHashSet();
                
                // Chuẩn bị danh sách category mới để bulk insert
                var newCategories = new List<Category>();
                
                foreach (var categoryData in response.Data.Items)
                {
                    // Kiểm tra category đã tồn tại chưa
                    if (!existingSlugs.Contains(categoryData.Slug))
                    {
                        var category = new Category
                        {
                            Id = Guid.NewGuid(),
                            Name = categoryData.Name,
                            Slug = categoryData.Slug
                        };

                        newCategories.Add(category);
                    }
                }
                
                // Bulk insert các category mới
                if (newCategories.Count > 0)
                {
                    await _categoryRepository.BulkInsertAsync(newCategories);
                    _logger.LogInformation("Đã thêm {Count} danh mục mới", newCategories.Count);
                }

                _logger.LogInformation("Hoàn thành crawl danh mục");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi crawl danh mục: {Message}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Crawl danh sách slug từ các trang
        /// </summary>
        public async Task<string[]> CrawlSlugsFromPagesAsync(int pageStart, int pageEnd)
        {
            var slugs = new List<string>();

            try
            {
                _logger.LogInformation("Bắt đầu crawl danh sách slug từ trang {PageStart} đến trang {PageEnd}", pageStart, pageEnd);

                var client = _httpClientFactory.CreateClient();

                for (int page = pageStart; page <= pageEnd; page++)
                {
                    _logger.LogInformation("Đang crawl trang {Page}", page);

                    var response = await client.GetFromJsonAsync<OtruyenMangaListResponse>($"{_baseUrl}/the-loai/co-dai?page={page}");

                    if (response?.Status != "success" || response.Data?.Items == null)
                    {
                        _logger.LogWarning("Không thể lấy danh sách truyện từ trang {Page}", page);
                        continue;
                    }

                    var pageSlugs = response.Data.Items.Select(m => m.Slug).ToArray();
                    slugs.AddRange(pageSlugs);

                    _logger.LogInformation("Đã tìm thấy {Count} truyện từ trang {Page}", pageSlugs.Length, page);
                }

                _logger.LogInformation("Hoàn thành crawl danh sách slug, tổng cộng {Count} truyện", slugs.Count);
                return slugs.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi crawl danh sách slug: {Message}", ex.Message);
                return slugs.Distinct().ToArray();
            }
        }

        /// <summary>
        /// Crawl thông tin manga theo danh sách slug
        /// </summary>
        public async Task<string[]> CrawlMangasBySlugsAsync(string[] slugs, CrawlRequest request)
        {
            var successSlugs = new List<string>();

            try
            {
                _logger.LogInformation("Bắt đầu crawl thông tin manga theo {Count} slug", slugs.Length);

                var client = _httpClientFactory.CreateClient();

                foreach (var slug in slugs)
                {
                    try
                    {
                        _logger.LogInformation("Đang crawl thông tin manga: {Slug}", slug);

                        var response = await client.GetFromJsonAsync<OtruyenMangaDetailResponse>($"{_baseUrl}/truyen-tranh/{slug}");

                        if (response?.Status != "success" || response.Data?.Item == null)
                        {
                            _logger.LogWarning("Không thể lấy thông tin manga: {Slug}", slug);
                            continue;
                        }

                        var mangaData = response.Data.Item;

                        // Kiểm tra manga đã tồn tại chưa
                        var existingManga = await _mangaRepository.GetBySlugAsync(slug);
                        Manga manga;

                        if (existingManga == null)
                        {
                            // Tạo manga mới
                            manga = new Manga
                            {
                                Id = Guid.NewGuid(),
                                Name = mangaData.Name,
                                Slug = mangaData.Slug,
                                OriginNames = mangaData.OriginName?.ToList(),
                                Description = mangaData.Description,
                                Content = mangaData.Content,
                                Status = mangaData.Status,
                                ThumbUrl = mangaData.ThumbUrl,
                                SubDocQuyen = mangaData.SubDocQuyen,
                                Authors = mangaData.Author?.ToList(),
                                UpdatedAt = mangaData.UpdatedAt,
                                Categories = new List<Category>(),
                                Chapters = new List<Chapter>(),
                                Seo = new SeoMeta
                                {
                                    Id = Guid.NewGuid(),
                                    TitleHead = mangaData.Name,
                                    DescriptionHead = mangaData.Description ?? mangaData.Name,
                                    OgType = "website",
                                    OgImage = new List<string> { $"{_thumbnailBaseUrl}{mangaData.ThumbUrl}" },
                                    OgUrl = $"/truyen-tranh/{mangaData.Slug}",
                                    UpdatedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                                }
                            };

                            // Thêm categories
                            if (mangaData.Category != null)
                            {
                                foreach (var categoryData in mangaData.Category)
                                {
                                    var category = await _categoryRepository.GetBySlugAsync(categoryData.Slug);
                                    if (category != null)
                                    {
                                        manga.Categories.Add(category);
                                    }
                                }
                            }

                            await _mangaRepository.AddAsync(manga);
                            _logger.LogInformation("Đã thêm manga mới: {Name}", manga.Name);
                        }
                        else
                        {
                            // Cập nhật manga hiện có
                            manga = existingManga;
                            manga.Name = mangaData.Name;
                            manga.OriginNames = mangaData.OriginName?.ToList();
                            manga.Description = mangaData.Description;
                            manga.Content = mangaData.Content;
                            manga.Status = mangaData.Status;
                            manga.ThumbUrl = mangaData.ThumbUrl;
                            manga.SubDocQuyen = mangaData.SubDocQuyen;
                            manga.Authors = mangaData.Author?.ToList();
                            manga.UpdatedAt = mangaData.UpdatedAt;

                            // Cập nhật categories
                            if (mangaData.Category != null)
                            {
                                // Lấy manga với categories
                                var mangaWithCategories = await _mangaRepository.GetBySlugWithDetailsAsync(slug);
                                manga.Categories = mangaWithCategories.Categories;

                                // Xóa tất cả categories hiện có
                                manga.Categories.Clear();

                                // Thêm lại categories
                                foreach (var categoryData in mangaData.Category)
                                {
                                    var category = await _categoryRepository.GetBySlugAsync(categoryData.Slug);
                                    if (category != null)
                                    {
                                        manga.Categories.Add(category);
                                    }
                                }
                            }

                            await _mangaRepository.UpdateAsync(manga);
                            _logger.LogInformation("Đã cập nhật manga: {Name}", manga.Name);
                        }

                        // Crawl chapters
                        if (mangaData.Chapters != null && mangaData.Chapters.Any())
                        {
                            var serverData = mangaData.Chapters.FirstOrDefault()?.ServerData;
                            if (serverData != null)
                            {
                                int totalChapters = serverData.Count;
                                int successChapters = 0;
                                int failedChapters = 0;
                                var failedChapterLogs = new List<ChapterLog>();

                                // Lấy tất cả chapter hiện có của manga để kiểm tra
                                var existingChapters = (await _chapterRepository.GetByMangaIdAsync(manga.Id)).ToList();
                                var existingChapterNames = existingChapters.Select(c => c.ChapterName).ToHashSet();
                                
                                // Chuẩn bị danh sách chapter mới để bulk insert
                                var newChapters = new List<Chapter>();
                                // Chuẩn bị danh sách chapter cần cập nhật để bulk update
                                var chaptersToUpdate = new List<Chapter>();

                                foreach (var chapterData in serverData)
                                {
                                    try
                                    {
                                        // Kiểm tra chapter đã tồn tại chưa
                                        var existingChapter = existingChapters.FirstOrDefault(c => c.ChapterName == chapterData.ChapterName);

                                        if (existingChapter == null)
                                        {
                                            // Tạo chapter mới
                                            var chapter = new Chapter
                                            {
                                                Id = Guid.NewGuid(),
                                                MangaId = manga.Id,
                                                Filename = chapterData.Filename,
                                                ChapterName = chapterData.ChapterName,
                                                ChapterTitle = chapterData.ChapterTitle,
                                                ChapterApiData = chapterData.ChapterApiData,
                                                Images = new List<ChapterImage>()
                                            };

                                            newChapters.Add(chapter);
                                            successChapters++;
                                        }
                                        else if (request.AllowOverwriteChapters)
                                        {
                                            // Cập nhật chapter hiện có
                                            existingChapter.Filename = chapterData.Filename;
                                            existingChapter.ChapterTitle = chapterData.ChapterTitle;
                                            existingChapter.ChapterApiData = chapterData.ChapterApiData;

                                            chaptersToUpdate.Add(existingChapter);
                                            successChapters++;
                                        }
                                        else
                                        {
                                            _logger.LogInformation("Bỏ qua chapter đã tồn tại: {Filename}", chapterData.Filename);
                                            successChapters++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Lỗi khi crawl chapter {ChapterName}: {Message}", chapterData.ChapterName, ex.Message);
                                        failedChapters++;

                                        // Ghi log chapter lỗi
                                        failedChapterLogs.Add(new ChapterLog
                                        {
                                            Id = Guid.NewGuid(),
                                            ChapterName = chapterData.ChapterName,
                                            ChapterApiData = chapterData.ChapterApiData,
                                            ErrorMessage = ex.Message
                                        });
                                    }
                                }
                                
                                // Bulk insert các chapter mới (sử dụng InsertOrUpdate để tránh duplicate)
                                if (newChapters.Count > 0)
                                {
                                    await _chapterRepository.BulkInsertOrUpdateAsync(newChapters);
                                    _logger.LogInformation("Đã thêm {Count} chapter mới cho manga: {Name}", newChapters.Count, manga.Name);
                                }
                                
                                // Bulk update các chapter cần cập nhật
                                if (chaptersToUpdate.Count > 0)
                                {
                                    await _chapterRepository.BulkUpdateAsync(chaptersToUpdate);
                                    _logger.LogInformation("Đã cập nhật {Count} chapter cho manga: {Name}", chaptersToUpdate.Count, manga.Name);
                                }

                                // Ghi log crawl
                                if (request.EnableLogging)
                                {
                                    var crawlLog = new CrawlLog
                                    {
                                        Id = Guid.NewGuid(),
                                        MangaSlug = manga.Slug,
                                        MangaName = manga.Name,
                                        TotalChapters = totalChapters,
                                        ChaptersCrawledSuccess = successChapters,
                                        ChaptersCrawledFailed = failedChapters,
                                        CrawledAt = DateTime.UtcNow,
                                        FailedChapters = failedChapterLogs
                                    };

                                    await _crawlLogRepository.AddAsync(crawlLog);
                                    _logger.LogInformation("Đã ghi log crawl cho manga: {Name}", manga.Name);
                                }
                            }
                        }

                        successSlugs.Add(slug);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi crawl manga {Slug}: {Message}", slug, ex.Message);
                    }
                }

                _logger.LogInformation("Hoàn thành crawl thông tin manga, thành công: {SuccessCount}/{TotalCount}", successSlugs.Count, slugs.Length);
                return successSlugs.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi crawl thông tin manga: {Message}", ex.Message);
                return successSlugs.ToArray();
            }
        }

        /// <summary>
        /// Crawl ảnh của tất cả chapter
        /// </summary>
        public async Task<bool> CrawlAllChapterImagesAsync(string[] mangaSlugs, CrawlRequest request)
        {
            try
            {
                _logger.LogInformation("Bắt đầu crawl ảnh của tất cả chapter cho {Count} manga", mangaSlugs.Length);

                var client = _httpClientFactory.CreateClient();

                foreach (var slug in mangaSlugs)
                {
                    try
                    {
                        _logger.LogInformation("Đang crawl ảnh chapter cho manga: {Slug}", slug);

                        // Lấy manga theo slug
                        var manga = await _mangaRepository.GetBySlugWithDetailsAsync(slug);
                        if (manga == null)
                        {
                            _logger.LogWarning("Không tìm thấy manga: {Slug}", slug);
                            continue;
                        }

                        // Lấy danh sách chapter của manga
                        var chapters = await _chapterRepository.GetByMangaIdAsync(manga.Id);
                        _logger.LogInformation("Tìm thấy {Count} chapter cho manga: {Name}", chapters.Count(), manga.Name);

                        // Crawl ảnh cho từng chapter
                        foreach (var chapter in chapters)
                        {
                            try
                            {
                                // Kiểm tra chapter đã có ảnh chưa
                                var existingImages = await _chapterImageRepository.GetByChapterIdAsync(chapter.Id);
                                if (existingImages.Any() && !request.AllowOverwriteChapters)
                                {
                                    _logger.LogInformation("Bỏ qua chapter đã có ảnh: {Filename}", chapter.Filename);
                                    continue;
                                }

                                // Gọi API lấy thông tin chapter
                                var response = await client.GetFromJsonAsync<OtruyenChapterDetailResponse>(chapter.ChapterApiData);

                                if (response?.Status != "success" || response.Data?.Item == null)
                                {
                                    _logger.LogWarning("Không thể lấy thông tin chapter: {Filename}", chapter.Filename);
                                    continue;
                                }

                                var chapterData = response.Data.Item;
                                var domainCdn = response.Data.DomainCdn;

                                // Xóa ảnh cũ nếu cho phép ghi đè
                                if (existingImages.Any() && request.AllowOverwriteChapters)
                                {
                                    await _chapterImageRepository.RemoveRangeAsync(existingImages);
                                    _logger.LogInformation("Đã xóa {Count} ảnh cũ của chapter: {Filename}", existingImages.Count(), chapter.Filename);
                                }

                                // Thêm ảnh mới
                                if (chapterData.ChapterImage != null)
                                {
                                    var chapterImages = chapterData.ChapterImage.Select(imageData => new ChapterImage
                                    {
                                        Id = Guid.NewGuid(),
                                        ChapterId = chapter.Id,
                                        Page = imageData.ImagePage,
                                        ImageFile = imageData.ImageFile,
                                        ChapterPath = chapterData.ChapterPath,
                                        DomainCdn = domainCdn
                                    }).ToList();

                                    await _chapterImageRepository.BulkInsertOrUpdateAsync(chapterImages);
                                    _logger.LogInformation("Đã thêm {Count} ảnh mới cho chapter: {Filename}", chapterImages.Count, chapter.Filename);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Lỗi khi crawl ảnh chapter {Filename}: {Message}", chapter.Filename, ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi crawl ảnh chapter cho manga {Slug}: {Message}", slug, ex.Message);
                    }
                }

                _logger.LogInformation("Hoàn thành crawl ảnh của tất cả chapter");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi crawl ảnh của tất cả chapter: {Message}", ex.Message);
                return false;
            }
        }

        #region Response Models

        private class OtruyenCategoryResponse
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
            [JsonPropertyName("data")]
            public OtruyenCategoryData Data { get; set; }
        }

        private class OtruyenCategoryData
        {
            [JsonPropertyName("items")]
            public List<OtruyenCategory> Items { get; set; }
        }

        private class OtruyenCategory
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("slug")]
            public string Slug { get; set; }
        }

        private class OtruyenMangaListResponse
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }
            [JsonPropertyName("data")]
            public OtruyenMangaListData Data { get; set; }
        }

        private class OtruyenMangaListData
        {
            [JsonPropertyName("items")]
            public List<OtruyenMangaListItem> Items { get; set; }
            [JsonPropertyName("pagination")]
            public OtruyenPagination Pagination { get; set; }
        }

        private class OtruyenMangaListItem
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("slug")]
            public string Slug { get; set; }
            [JsonPropertyName("origin_name")]
            public string[] OriginName { get; set; }
            [JsonPropertyName("status")]
            public string Status { get; set; }
            [JsonPropertyName("thumb_url")]
            public string ThumbUrl { get; set; }
            [JsonPropertyName("sub_docquyen")]
            public bool SubDocQuyen { get; set; }
            [JsonPropertyName("category")]
            public List<OtruyenCategory> Category { get; set; }
            [JsonPropertyName("updatedAt")]
            public DateTime UpdatedAt { get; set; }
            [JsonPropertyName("chaptersLatest")]
            public List<OtruyenChapterListItem> ChaptersLatest { get; set; }
        }

        private class OtruyenPagination
        {
            [JsonPropertyName("totalItems")]
            public int TotalItems { get; set; }
            [JsonPropertyName("totalItemsPerPage")]
            public int TotalItemsPerPage { get; set; }
            [JsonPropertyName("currentPage")]
            public int CurrentPage { get; set; }
        }

        private class OtruyenMangaDetailResponse
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
            [JsonPropertyName("data")]
            public OtruyenMangaDetailData Data { get; set; }
        }

        private class OtruyenMangaDetailData
        {
            [JsonPropertyName("seoOnPage")]
            public OtruyenSeoOnPage SeoOnPage { get; set; }
            [JsonPropertyName("breadCrumb")]
            public List<OtruyenBreadCrumb> BreadCrumb { get; set; }
            [JsonPropertyName("params")]
            public OtruyenParams Params { get; set; }
            [JsonPropertyName("item")]
            public OtruyenMangaDetail Item { get; set; }
            [JsonPropertyName("APP_DOMAIN_CDN_IMAGE")]
            public string AppDomainCdnImage { get; set; }
        }

        private class OtruyenSeoOnPage
        {
            [JsonPropertyName("og_type")]
            public string OgType { get; set; }
            [JsonPropertyName("titleHead")]
            public string TitleHead { get; set; }
            [JsonPropertyName("seoSchema")]
            public OtruyenSeoSchema SeoSchema { get; set; }
            [JsonPropertyName("descriptionHead")]
            public string DescriptionHead { get; set; }
            [JsonPropertyName("og_image")]
            public List<string> OgImage { get; set; }
            [JsonPropertyName("updated_time")]
            public long UpdatedTime { get; set; }
            [JsonPropertyName("og_url")]
            public string OgUrl { get; set; }
        }

        private class OtruyenSeoSchema
        {
            [JsonPropertyName("@context")]
            public string Context { get; set; }
            [JsonPropertyName("@type")]
            public string Type { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("url")]
            public string Url { get; set; }
            [JsonPropertyName("image")]
            public string Image { get; set; }
            [JsonPropertyName("director")]
            public string Director { get; set; }
        }

        private class OtruyenBreadCrumb
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("slug")]
            public string Slug { get; set; }
            [JsonPropertyName("isCurrent")]
            public bool? IsCurrent { get; set; }
            [JsonPropertyName("position")]
            public int Position { get; set; }
        }

        private class OtruyenParams
        {
            [JsonPropertyName("slug")]
            public string Slug { get; set; }
            [JsonPropertyName("crawl_check_url")]
            public string CrawlCheckUrl { get; set; }
        }

        private class OtruyenMangaDetail
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("slug")]
            public string Slug { get; set; }
            [JsonPropertyName("origin_name")]
            public string[] OriginName { get; set; }
            [JsonPropertyName("description")]
            public string Description { get; set; }
            [JsonPropertyName("content")]
            public string Content { get; set; }
            [JsonPropertyName("status")]
            public string Status { get; set; }
            [JsonPropertyName("thumb_url")]
            public string ThumbUrl { get; set; }
            [JsonPropertyName("sub_docquyen")]
            public bool SubDocQuyen { get; set; }
            [JsonPropertyName("author")]
            public string[] Author { get; set; }
            [JsonPropertyName("updatedAt")]
            public DateTime UpdatedAt { get; set; }
            [JsonPropertyName("category")]
            public List<OtruyenCategory> Category { get; set; }
            [JsonPropertyName("chapters")]
            public List<OtruyenChapterServer> Chapters { get; set; }
        }

        private class OtruyenChapterServer
        {
            [JsonPropertyName("server_name")]
            public string ServerName { get; set; }
            [JsonPropertyName("server_data")]
            public List<OtruyenChapterListItem> ServerData { get; set; }
        }

        private class OtruyenChapterListItem
        {
            [JsonPropertyName("filename")]
            public string Filename { get; set; }
            [JsonPropertyName("chapter_name")]
            public string ChapterName { get; set; }
            [JsonPropertyName("chapter_title")]
            public string ChapterTitle { get; set; }
            [JsonPropertyName("chapter_api_data")]
            public string ChapterApiData { get; set; }
        }

        private class OtruyenChapterDetailResponse
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }
            [JsonPropertyName("data")]
            public OtruyenChapterDetailData Data { get; set; }
        }

        private class OtruyenChapterDetailData
        {
            [JsonPropertyName("domain_cdn")]
            public string DomainCdn { get; set; }
            [JsonPropertyName("item")]
            public OtruyenChapterDetail Item { get; set; }
        }

        private class OtruyenChapterDetail
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; }
            [JsonPropertyName("comic_name")]
            public string ComicName { get; set; }
            [JsonPropertyName("chapter_name")]
            public string ChapterName { get; set; }
            [JsonPropertyName("chapter_title")]
            public string ChapterTitle { get; set; }
            [JsonPropertyName("chapter_path")]
            public string ChapterPath { get; set; }
            [JsonPropertyName("chapter_image")]
            public List<OtruyenChapterImage> ChapterImage { get; set; }
        }

        private class OtruyenChapterImage
        {
            [JsonPropertyName("image_page")]
            public int ImagePage { get; set; }
            [JsonPropertyName("image_file")]
            public string ImageFile { get; set; }
        }

        #endregion
    }
}