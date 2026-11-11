using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.CreateIntern.handler
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, RequestResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IMediator _mediator;
        public CreateInternCommandHandler(IUnitOfWork unitOfWork, IGeneralRepository<Intern> internRepository, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _internRepository = internRepository;
            _mediator = mediator;
        }
        public async Task<RequestResponse<int>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                errors.Add("FullName", new[] {"Full name is required"});
            }
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors.Add("Email", new[] {"Email is required" });
            }
            if (request.BirthYear > DateTime.UtcNow.Year|| request.BirthYear < 1900)
            {
                errors.Add("BirthYear", new[] {"Invalid birth year"});
            }
            if (request.TrackId <= 0)
            {
                errors.Add("TrackId", new[] {"Track Id must be greater than zero"});
            }
            if (errors.Count > 0)
            {
                return RequestResponse<int>.Fail(400, errors);
            }
            var trackResponse =await _mediator.Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);
            if (trackResponse.Data is null)
            {
                return RequestResponse<int>
                    .Fail(404,new Dictionary<string, string[]>{{ "Track", new[] { "Track data not found" } }});
            }

            if (!trackResponse.Data.IsActive)
            {
                return RequestResponse<int>
                    .Fail(409,new Dictionary<string, string[]>{{ "Track", new[] { "Track is inactive" } }});
            }
            var emailExists = await _internRepository.GetAll().AnyAsync(i => i.Email.ToLower() == request.Email.ToLower(), cancellationToken);
            if (emailExists)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Email", new[] { "Email already exists." } } });
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
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(intern.Id);
        }
    }
}
