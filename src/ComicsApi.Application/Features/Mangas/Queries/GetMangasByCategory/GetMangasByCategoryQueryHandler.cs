using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Common;
using ComicsApi.Domain.Interfaces;
using MediatR;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangasByCategory
{
    /// <summary>
    /// Handler xử lý query lấy danh sách manga theo danh mục
    /// </summary>
    public class GetMangasByCategoryQueryHandler : IRequestHandler<GetMangasByCategoryQuery, PagedResult<MangaDto>>
    {
        private readonly IMangaRepository _mangaRepository;
        private readonly IMapper _mapper;

        public GetMangasByCategoryQueryHandler(IMangaRepository mangaRepository, IMapper mapper)
        {
            _mangaRepository = mangaRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<MangaDto>> Handle(GetMangasByCategoryQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _mangaRepository.GetByCategoryAsync(request.CategorySlug, request.Page, request.PageSize);
            return _mapper.Map<PagedResult<MangaDto>>(pagedResult);
        }
    }
}