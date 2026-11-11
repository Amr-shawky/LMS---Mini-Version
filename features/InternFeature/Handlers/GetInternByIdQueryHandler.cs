using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, RequestResponse<GetInternByIdDTO>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public GetInternByIdQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<GetInternByIdDTO>> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _repository.GetAll()
           .Where(x => x.Id == request.InternId)
           .Select(x => new GetInternByIdDTO()
           {
               Id = x.Id,
               FullName = x.FullName,
               Email = x.Email,
               Status = x.Status
           }).FirstOrDefaultAsync();
            if(intern is null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    { "Intern", new []{"Intern With that Id  doesnot Exist"} }
                };
                return RequestResponse<GetInternByIdDTO>.Fail(404, errors);
            }
            return RequestResponse<GetInternByIdDTO>.OK(intern);


        }
    }
}
