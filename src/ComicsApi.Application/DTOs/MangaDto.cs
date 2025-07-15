using System;
using System.Collections.Generic;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// DTO cho entity Manga
    /// </summary>
    public class MangaDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<string> OriginNames { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string ThumbUrl { get; set; }
        public bool SubDocQuyen { get; set; }
        public List<string> Authors { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<CategoryDto> Categories { get; set; }
        public List<ChapterDto> Chapters { get; set; }
        public SeoMetaDto Seo { get; set; }
    }
}