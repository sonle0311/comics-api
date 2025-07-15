using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Interfaces;
using MediatR;

namespace ComicsApi.Application.Features.Chapters.Queries.GetChapterDetail
{
    /// <summary>
    /// Handler xử lý query lấy chi tiết chapter theo manga id và chapter name
    /// </summary>
    public class GetChapterDetailQueryHandler : IRequestHandler<GetChapterDetailQuery, ChapterDto>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IChapterImageRepository _chapterImageRepository;
        private readonly IMapper _mapper;

        public GetChapterDetailQueryHandler(
            IChapterRepository chapterRepository,
            IChapterImageRepository chapterImageRepository,
            IMapper mapper)
        {
            _chapterRepository = chapterRepository;
            _chapterImageRepository = chapterImageRepository;
            _mapper = mapper;
        }

        public async Task<ChapterDto> Handle(GetChapterDetailQuery request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByMangaIdAndChapterNameAsync(request.MangaId, request.ChapterName);
            if (chapter == null) return null;
            
            // Nếu chapter không có images, lấy thêm images
            if (chapter.Images == null || chapter.Images.Count == 0)
            {
                var images = await _chapterImageRepository.GetByChapterIdAsync(chapter.Id);
                chapter.Images = new List<Domain.Entities.ChapterImage>(images);
            }
            
            return _mapper.Map<ChapterDto>(chapter);
        }
    }
}