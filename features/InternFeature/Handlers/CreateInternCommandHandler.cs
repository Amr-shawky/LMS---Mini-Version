using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, RequestResponse<CreateInternDto>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public CreateInternCommandHandler(IGeneralRepository<Intern> repository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResponse<CreateInternDto>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var trackResponse = await _mediator.Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);
            if (!trackResponse.Success || trackResponse.Data is null)
                return RequestResponse<CreateInternDto>.Fail(404, "TrackId", "Track not found");

            if (!trackResponse.Data.IsActive)
                return RequestResponse<CreateInternDto>.Fail(400, "TrackId", "Track is not active");

            var email = request.Email.Trim().ToLower();
            var emailExists = await _repository.GetAll()
                .AnyAsync(i => i.Email.ToLower() == email);
            if (emailExists)
                return RequestResponse<CreateInternDto>.Fail(409, "Email", "Email is already registered");

            var intern = new Intern
            {
                FullName = request.FullName,
                Email = email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };

            _repository.Add(intern);
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse<CreateInternDto>.OK(new CreateInternDto { Id = intern.Id });
        }
    }
}
