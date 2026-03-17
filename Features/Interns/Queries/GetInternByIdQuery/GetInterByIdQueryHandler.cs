using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries.GetInternByIdQuery;

public class GetInterByIdQueryHandler : IRequestHandler<GetInternByIdQuery,InternDto?>
{
    private readonly IUnitOfWork _uow;
    public GetInterByIdQueryHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
    {
        var intern = await _uow.Interns.GetWithTrackAsync(request.Id).ConfigureAwait(false);
        return intern?.ToDto();
    }
}