using System;
using System.Collections.Generic;

namespace ComicsApi.Domain.Entities
{
    /// <summary>
    /// Đại diện cho một thể loại truyện
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        // Navigation property
        public ICollection<Manga> Mangas { get; set; } = new List<Manga>();
    }
}