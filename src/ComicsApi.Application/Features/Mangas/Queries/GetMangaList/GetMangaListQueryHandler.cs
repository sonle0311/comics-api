using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Common;
using ComicsApi.Domain.Interfaces;
using MediatR;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangaList
{
    /// <summary>
    /// Handler xử lý query lấy danh sách manga theo trang
    /// </summary>
    public class GetMangaListQueryHandler : IRequestHandler<GetMangaListQuery, PagedResult<MangaDto>>
    {
        private readonly IMangaRepository _mangaRepository;
        private readonly IMapper _mapper;

        public GetMangaListQueryHandler(IMangaRepository mangaRepository, IMapper mapper)
        {
            _mangaRepository = mangaRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<MangaDto>> Handle(GetMangaListQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _mangaRepository.GetByPageAsync(request.Page, request.PageSize);
            return _mapper.Map<PagedResult<MangaDto>>(pagedResult);
        }
    }
}