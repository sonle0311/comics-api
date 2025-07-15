using System;
using System.Collections.Generic;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// DTO cho entity SeoMeta
    /// </summary>
    public class SeoMetaDto
    {
        public Guid Id { get; set; }
        public string TitleHead { get; set; }
        public string DescriptionHead { get; set; }
        public string OgType { get; set; }
        public List<string> OgImage { get; set; }
        public string OgUrl { get; set; }
        public long? UpdatedTime { get; set; }
    }
}