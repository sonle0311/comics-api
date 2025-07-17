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
        
        /// <summary>
        /// Danh sách manga thuộc category này, sử dụng MangaSummaryDto để tránh circular reference
        /// </summary>
        public List<MangaSummaryDto> Mangas { get; set; }
    }
}