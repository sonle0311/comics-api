using AutoMapper;
using ComicsApi.Application.DTOs;
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
            
            // Manga mapping - map Categories but ignore Chapters to avoid circular references
            CreateMap<Manga, MangaDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Chapters, opt => opt.Ignore())
                .MaxDepth(2);
            CreateMap<MangaDto, Manga>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.Chapters, opt => opt.Ignore())
                .MaxDepth(1);
            
            // Category mapping - ignore circular references
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Mangas, opt => opt.Ignore())
                .MaxDepth(1);
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Mangas, opt => opt.Ignore())
                .MaxDepth(1);
            
            // Chapter mapping
            CreateMap<Chapter, ChapterDto>();
            CreateMap<ChapterDto, Chapter>();
            
            // ChapterImage mapping
            CreateMap<ChapterImage, ChapterImageDto>();
            CreateMap<ChapterImageDto, ChapterImage>();
            
            // SeoMeta mapping
            CreateMap<SeoMeta, SeoMetaDto>();
            CreateMap<SeoMetaDto, SeoMeta>();
            
            // CrawlLog mapping
            CreateMap<CrawlLog, CrawlLogDto>();
            CreateMap<CrawlLogDto, CrawlLog>();
            
            // ChapterLog mapping
            CreateMap<ChapterLog, ChapterLogDto>();
            CreateMap<ChapterLogDto, ChapterLog>();
        }
    }
}