using System;
using System.Collections.Generic;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// DTO cho entity Category
    /// </summary>
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        
        public List<MangaDto> Mangas { get; set; }
    }
}