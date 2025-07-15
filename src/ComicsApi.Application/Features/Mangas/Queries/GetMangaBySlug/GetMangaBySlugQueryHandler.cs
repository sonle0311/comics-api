using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Interfaces;
using MediatR;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangaBySlug
{
    /// <summary>
    /// Handler xử lý query lấy thông tin manga theo slug
    /// </summary>
    public class GetMangaBySlugQueryHandler : IRequestHandler<GetMangaBySlugQuery, MangaDto>
    {
        private readonly IMangaRepository _mangaRepository;
        private readonly IMapper _mapper;

        public GetMangaBySlugQueryHandler(IMangaRepository mangaRepository, IMapper mapper)
        {
            _mangaRepository = mangaRepository;
            _mapper = mapper;
        }

        public async Task<MangaDto> Handle(GetMangaBySlugQuery request, CancellationToken cancellationToken)
        {
            var manga = await _mangaRepository.GetBySlugWithDetailsAsync(request.Slug);
            return _mapper.Map<MangaDto>(manga);
        }
    }
}