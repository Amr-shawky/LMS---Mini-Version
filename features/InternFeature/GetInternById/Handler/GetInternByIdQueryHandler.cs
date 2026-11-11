using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetInternById.Dtos;
using LMS___Mini_Version.features.InternFeature.GetInternById.Query;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetInternById.Handler
{
    public class GetInternByIdQueryHandler
     : IRequestHandler<GetInternByIdQuery, RequestResponse<InternDtoById>>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<InternDtoById>> Handle(
            GetInternByIdQuery request,
            CancellationToken cancellationToken)
        {
            var intern = await _repository.GetByIdAsync(request.Id);

            if (intern is null)
            {
                return RequestResponse<InternDtoById>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["InternId"] = new[] { "Intern not found." }
                    });
            }

            var dto = new InternDtoById(intern.Id, intern.FullName);

            return RequestResponse<InternDtoById>.OK(dto);
        }
    }
}
