using AutoMapper;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Common;
using ComicsApi.Domain.Entities;

namespace ComicsApi.Application.Mappings
{
    /// <summary>
    /// Cấu hình mapping giữa entity và DTO
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cấu hình để tránh circular reference
            this.AllowNullCollections = true;
            this.AllowNullDestinationValues = true;

            // Manga mapping
            CreateMap<Manga, MangaDto>();
            CreateMap<MangaDto, Manga>();

            // MangaSummaryDto mapping
            CreateMap<Manga, MangaSummaryDto>();

            // Category mapping
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            
            // Chapter mapping
            CreateMap<Chapter, ChapterDto>();
            CreateMap<ChapterDto, Chapter>();
            
            // ChapterImage mapping
            CreateMap<ChapterImage, ChapterImageDto>();
            CreateMap<ChapterImageDto, ChapterImage>();
            
            // SeoMeta mapping
            CreateMap<SeoMeta, SeoMetaDto>();
            CreateMap<SeoMetaDto, SeoMeta>();

            // PagedResult mapping
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
            
            // CrawlLog mapping
            CreateMap<CrawlLog, CrawlLogDto>();
            CreateMap<CrawlLogDto, CrawlLog>();
            
            // ChapterLog mapping
            CreateMap<ChapterLog, ChapterLogDto>();
            CreateMap<ChapterLogDto, ChapterLog>();
        }
    }
}