using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.Queries;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.CreateIntern.handler
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, RequestResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IMediator _mediator;
        public CreateInternCommandHandler(IUnitOfWork unitOfWork, IGeneralRepository<Intern> repository, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
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
            var emailExistsResponse = await _mediator.Send(new CheckInternEmailExistsQuery(request.Email),cancellationToken);
   
            if (!emailExistsResponse.Success)
            {
                return RequestResponse<int>.Fail(emailExistsResponse.StatusCode,emailExistsResponse.Errors);
            }
            if (emailExistsResponse.Data)
            {
                return RequestResponse<int>.Fail(409,new Dictionary<string, string[]>{ { "Email", new[] { "Email already exists." } }  });
            }
            var intern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };
            _repository.Add(intern);
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(intern.Id);
        }
    }
}
