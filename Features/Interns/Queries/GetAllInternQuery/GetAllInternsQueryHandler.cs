using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries;

public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery,IEnumerable<InternDto>>
{
    private readonly UnitOfWork _uow;
    public GetAllInternsQueryHandler(UnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
    {
        var interns = await _uow.Interns.GetAllWithTrackAsync()
            .ConfigureAwait(false);
        return interns.Select(x => x.ToDto());

    }
}