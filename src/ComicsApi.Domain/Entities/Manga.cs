using System;
using System.Collections.Generic;

namespace ComicsApi.Domain.Entities
{
    /// <summary>
    /// Đại diện cho một bộ truyện tranh
    /// </summary>
    public class Manga
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<string>? OriginNames { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string Status { get; set; } // ongoing, completed, etc.
        public string ThumbUrl { get; set; }
        public bool SubDocQuyen { get; set; }
        public List<string>? Authors { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
        public SeoMeta? Seo { get; set; }
    }
}