using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetInternById.Dtos;
using LMS___Mini_Version.features.InternFeature.GetInternById.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetInternById.handler
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
            if (request.Id <= 0)
            {
                return RequestResponse<GetInternByIdDto>.Fail(400,new Dictionary<string, string[]>{{ "InternId", new[] { "Intern ID must be greater than zero." } }});
            }

            var intern = await _repository.GetByIdAsync(request.Id);
            if (intern == null)
            {
                return RequestResponse<GetInternByIdDto>.Fail(404, new Dictionary<string, string[]> { { "Intern", new string[] { "Intern not found." } } });
            }

            var dto = new GetInternByIdDto
            {
                Id = intern.Id,
                TrackId = intern.TrackId,
                Status = intern.Status
            };

            return RequestResponse<GetInternByIdDto>.OK(dto);
        }
    }
}
