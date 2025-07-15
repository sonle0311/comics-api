using MediatR;

namespace ComicsApi.Application.Features.Crawler.Commands.RunCrawler
{
    /// <summary>
    /// Command để chạy crawler
    /// </summary>
    public class RunCrawlerCommand : IRequest<bool>
    {
        public CrawlRequest Request { get; set; }
        
        public RunCrawlerCommand(CrawlRequest request)
        {
            Request = request;
        }
    }
}