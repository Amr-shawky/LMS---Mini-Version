using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Queries.GetById;

public class GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepository)
    : IRequestHandler<GetInternByIdQuery, Intern?>
{
    public async Task<Intern?> Handle(GetInternByIdQuery request, CancellationToken ct)
        => await internRepository.GetByIdAsync(request.InternId);
}