using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDto?>
    {
        private readonly IGeneralRepository<Intern> _internRepo;
        private readonly IMediator _mediator;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepo,IMediator mediator)
        {
            _internRepo = internRepo;
            _mediator = mediator;
        }

        public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern =await _internRepo.GetTable()
                .Where(intern=>intern.Id == request.Id)
                .Select(intern => new InternDto
                {
                    Id = intern.Id,
                    FullName = intern.FullName,
                    Email = intern.Email,
                    BirthYear = intern.BirthYear,
                    Status = intern.Status,
                    TrackId = intern.TrackId,
                    TrackName = intern.Track.Name,
                })
                .FirstOrDefaultAsync();
            return intern;
            
        }
    }
}
