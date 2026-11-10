
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class AssignInternIntoTrackCommandHandler
        : IRequestHandler<AssignInternIntoTrackCommand, RequestResponse<CreateInternDTO>>
    {
        private readonly IGeneralRepository<Intern> _internRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AssignInternIntoTrackCommandHandler(
            IGeneralRepository<Intern> intern,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _internRepo = intern;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResponse<CreateInternDTO>> Handle(
            AssignInternIntoTrackCommand request,
            CancellationToken cancellationToken)
        {
            

            var existingIntern = await _internRepo.GetAll()
                .AnyAsync(
                    x => x.Email.ToLower() == request.Email.ToLower(),
                    cancellationToken);

            if (existingIntern)
            {
                IDictionary<string, string[]> errors =
                    new Dictionary<string, string[]>
                    {
                        { "Email", new[] { "An intern with this email already exists." } }
                    };

                return RequestResponse<CreateInternDTO>.Fail(409, errors);
            }

            var res = await _mediator.Send(
                new GetActiveTracksQuery(), cancellationToken);


            if (!res.Success || res.Data is null ||
                !res.Data.Any(x => x.Id == request.trackId))
            {
                IDictionary<string, string[]> errors =
                    new Dictionary<string, string[]>
                    {
                        { "Track", new[] { "Cannot assign an intern to an inactive or non-existing track." } }
                    };

                return RequestResponse<CreateInternDTO>.Fail(400, errors);
            }

            

            var intern = new Intern()
            {
                Email = request.Email,
                BirthYear = request.birthYear,
                FullName = request.fullName,
                Status = "Pending",
                TrackId = request.trackId
            };

            _internRepo.Add(intern);

            
            var countResult = await _unitOfWork.SaveChangesAsync();

            if (countResult > 0)
            {
                var dto = new CreateInternDTO()
                {
                    Id = intern.Id
                };

                return RequestResponse<CreateInternDTO>.OK(dto, 201);
            }

           
            IDictionary<string, string[]> saveErrors =
                new Dictionary<string, string[]>
                {
                    { "Failure", new[] { "The intern was not saved correctly." } }
                };

            return RequestResponse<CreateInternDTO>.Fail(400, saveErrors);
        }
    }
}
