using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, RequestResponse<GetInternByIdDto>>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<GetInternByIdDto>> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _repository.GetByIdAsync(request.id);
            if (intern == null)
            {
                return RequestResponse<GetInternByIdDto>.Fail(404, "InternId", "Intern Not Found");
            }
            var internDto = new GetInternByIdDto()
            {
                FullName = intern.FullName,
                Email = intern.Email,
                BirthYear = intern.BirthYear,
                Status = intern.Status,
            };

            return RequestResponse<GetInternByIdDto>.OK(internDto);
        }
    }
}
