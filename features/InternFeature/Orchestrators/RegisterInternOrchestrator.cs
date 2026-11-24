using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;
using System.Net.Mail;

namespace LMS___Mini_Version.features.InternFeature.Orchestrators
{
    public record RegisterInternOrchestrator(string FullName, string Email, int BirthYear, int TrackId) : IRequest<RequestResponse<CreateInternDto>>;

    public class RegisterInternOrchestratorHandler : IRequestHandler<RegisterInternOrchestrator, RequestResponse<CreateInternDto>>
    {
        private readonly IMediator _mediator;

        public RegisterInternOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse<CreateInternDto>> Handle(RegisterInternOrchestrator request, CancellationToken cancellationToken)
        {
            // Action validation (input only, no DB)
            if (string.IsNullOrWhiteSpace(request.FullName))
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.InvalidFullName);

            if (!IsValidEmail(request.Email))
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.InvalidEmail);

            var currentYear = DateTime.UtcNow.Year;
            if (request.BirthYear < 1900 || request.BirthYear > currentYear - 15)
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.InvalidBirthYear);

            if (request.TrackId <= 0)
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.InvalidTrackId);

            var trackResponse = await _mediator.Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);
            if (!trackResponse.Success || trackResponse.Data is null)
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.TrackNotFound, 404);

            if (!trackResponse.Data.IsActive)
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.TrackNotActive);

            var check = await _mediator.Send(new CheckEmailExistsQuery(request.Email));
            if (!check.Success || check.Data == true)
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.EmailAlreadyExists);

            var created = await _mediator.Send(new CreateInternCommand(
                request.FullName,
                request.Email,
                request.BirthYear,
                request.TrackId));


            if (!created.Success)
                return RequestResponse<CreateInternDto>.Fail(ErrorCode.InternNotCreated);
            return RequestResponse<CreateInternDto>.OK(new CreateInternDto { Id = created.Data });

        }

        private static bool IsValidEmail(string? email) =>
            !string.IsNullOrWhiteSpace(email)
            && MailAddress.TryCreate(email.Trim(), out var addr)
            && addr.Address == email.Trim();
    }
}
