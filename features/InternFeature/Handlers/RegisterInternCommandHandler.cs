using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class RegisterInternCommandHandler : IRequestHandler<RegisterInternCommand, RequestResponse<RegisterInternDto>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public RegisterInternCommandHandler(IGeneralRepository<Intern> internRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator )
        {
             _internRepository = internRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResponse<RegisterInternDto>> Handle(RegisterInternCommand request, CancellationToken cancellationToken)
        {
            var existingIntern = await _internRepository.GetAll().AnyAsync(i => i.Email.ToLower() == request.Email.ToLower(), cancellationToken);
            if (existingIntern)
            {
                IDictionary<string, string[]> errors =
                    new Dictionary<string, string[]>
                    {
                        { "Email", new[] { "An intern with this email already exists." } }
                    };

                return RequestResponse<RegisterInternDto>.Fail(409, errors);
            }
            var activeTracks = await _mediator.Send(new GetActiveTracksQuery(), cancellationToken);
            if (!activeTracks.Success || activeTracks.Data is null ||
               !activeTracks.Data.Any(x => x.Id == request.TrackId))
            {
                IDictionary<string, string[]> errors =
                    new Dictionary<string, string[]>
                    {
                        { "Track", new[] { "Cannot assign an intern to an inactive or non-existing track." } }
                    };

                return RequestResponse<RegisterInternDto>.Fail(400, errors);
            }

            var intern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };
            _internRepository.Add(intern);
            var Result =await _unitOfWork.SaveChangesAsync();
            if (Result > 0)
            {
                var dto = new RegisterInternDto()
                {
                    internId = intern.Id,
                };
                return RequestResponse<RegisterInternDto>.OK(dto, 201);
            }
            IDictionary<string, string[]> saveErrors =
             new Dictionary<string, string[]>
             {
                    { "Failure", new[] { "The intern was not saved correctly." } }
             };

            return RequestResponse<RegisterInternDto>.Fail(400, saveErrors);

        }
    }
}
