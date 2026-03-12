using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries.GetAllByTrackIdQuery;

public class GetAllByTrackIdQueryHandler : IRequestHandler<GetAllByTrackIdQuery,IEnumerable<InternDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllByTrackIdQueryHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<IEnumerable<InternDto>> Handle(GetAllByTrackIdQuery request, CancellationToken cancellationToken)
    {
        var interns = await _uow.Interns.GetAllByTrackIdAsync(request.trackId).ConfigureAwait(false);
        return interns.Select(x => x.ToDto());
    }
}