using System;
using System.Collections.Generic;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// Represents a summary of a manga, used for lists to avoid circular references.
    /// </summary>
    public class MangaSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ThumbUrl { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}