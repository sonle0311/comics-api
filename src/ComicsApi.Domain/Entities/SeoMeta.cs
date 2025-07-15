using System;
using System.Collections.Generic;

namespace ComicsApi.Domain.Entities
{
    /// <summary>
    /// Đại diện cho thông tin SEO của truyện
    /// </summary>
    public class SeoMeta
    {
        public Guid Id { get; set; }
        public string TitleHead { get; set; }
        public string DescriptionHead { get; set; }
        public string OgType { get; set; }
        public List<string> OgImage { get; set; }
        public string? OgUrl { get; set; }
        public long? UpdatedTime { get; set; }
    }
}