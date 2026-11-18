using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Queries
{
    public record CheckInternExistsQuery(int internId): IRequest<RequestResponse<bool>>
    {
    }

    public class CheckInternExistsQueryHandler : IRequestHandler<CheckInternExistsQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public CheckInternExistsQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<bool>> Handle(CheckInternExistsQuery request, CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(request.internId);
            
            return RequestResponse<bool>.OK(exists);


        }
    }
}
