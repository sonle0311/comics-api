namespace ComicsApi.Application.Features.Crawler.Commands
{
    /// <summary>
    /// Request model cho việc crawl dữ liệu
    /// </summary>
    public class CrawlRequest
    {
        public int PageStart { get; set; } = 1;
        public int PageEnd { get; set; } = 5;
        public bool AllowOverwriteChapters { get; set; } = false;
        public bool EnableLogging { get; set; } = true;
        public bool ReCrawlFailedChapters { get; set; } = true;
    }
}