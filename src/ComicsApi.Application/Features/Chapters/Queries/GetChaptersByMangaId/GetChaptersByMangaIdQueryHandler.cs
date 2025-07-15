using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Interfaces;
using MediatR;

namespace ComicsApi.Application.Features.Chapters.Queries.GetChaptersByMangaId
{
    /// <summary>
    /// Handler xử lý query lấy danh sách chapter theo manga id
    /// </summary>
    public class GetChaptersByMangaIdQueryHandler : IRequestHandler<GetChaptersByMangaIdQuery, IEnumerable<ChapterDto>>
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IMapper _mapper;

        public GetChaptersByMangaIdQueryHandler(IChapterRepository chapterRepository, IMapper mapper)
        {
            _chapterRepository = chapterRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChapterDto>> Handle(GetChaptersByMangaIdQuery request, CancellationToken cancellationToken)
        {
            var chapters = await _chapterRepository.GetByMangaIdAsync(request.MangaId);
            return _mapper.Map<IEnumerable<ChapterDto>>(chapters);
        }
    }
}