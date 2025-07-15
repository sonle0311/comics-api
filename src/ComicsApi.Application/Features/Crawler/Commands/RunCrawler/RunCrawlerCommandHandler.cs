using System.Threading;
using System.Threading.Tasks;
using ComicsApi.Application.Interfaces;
using MediatR;

namespace ComicsApi.Application.Features.Crawler.Commands.RunCrawler
{
    /// <summary>
    /// Handler xử lý command chạy crawler
    /// </summary>
    public class RunCrawlerCommandHandler : IRequestHandler<RunCrawlerCommand, bool>
    {
        private readonly ICrawlerService _crawlerService;

        public RunCrawlerCommandHandler(ICrawlerService crawlerService)
        {
            _crawlerService = crawlerService;
        }

        public async Task<bool> Handle(RunCrawlerCommand request, CancellationToken cancellationToken)
        {
            return await _crawlerService.CrawlAllAsync(request.Request);
        }
    }
}