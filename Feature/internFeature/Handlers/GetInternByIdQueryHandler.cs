using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.internFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.internFeature.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, RequestResponse<InternDto>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<RequestResponse<InternDto>> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _internRepository.GetAll()
                .Include(i => i.Track)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (intern == null)
            {
                return RequestResponse<InternDto>.Fail($"Intern with ID {request.Id} not found", 404);
            }

            var dto = new InternDto
            {
                Id = intern.Id,
                FullName = intern.FullName,
                Email = intern.Email,
                BirthYear = intern.BirthYear,
                Status = intern.Status,
                TrackId = intern.TrackId,
                TrackName = intern.Track?.Name ?? string.Empty
            };

            return RequestResponse<InternDto>.Ok(dto);
        }
    }
}
