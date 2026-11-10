using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, RequestResponse<GetInternByIdDTO>>
    {
        private readonly IGeneralRepository<Intern> internRepo;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepo)
        {
            this.internRepo = internRepo;
        }
        public async Task<RequestResponse<GetInternByIdDTO>> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await internRepo.GetByIdAsync(request.internId);
            if(intern==null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    { "Intern", new []{"Intern With that Id  doesnot Exist"} }
                };
                return  RequestResponse<GetInternByIdDTO>.Fail(404, errors);
            }
            else
            {
                var dto = new GetInternByIdDTO()
                {
                    Id = request.internId,
                    FullName = intern.FullName,
                    Status = intern.Status,
                    TrackId = intern.TrackId,

                };
                return RequestResponse<GetInternByIdDTO>.OK( dto, 200);
            }
        }
    }
}
