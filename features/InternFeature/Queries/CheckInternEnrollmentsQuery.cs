using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Queries
{
    public record CheckInternEnrollmentsQuery(int Id): IRequest<RequestResponse<bool>>
    {
    }

    public class CheckInternEnrollmentsQueryHandler : IRequestHandler<CheckInternEnrollmentsQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public CheckInternEnrollmentsQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<bool>> Handle(CheckInternEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var hasEnrollments = await _repository.GetAll()
        .AnyAsync(i => i.Id == request.Id && i.Enrollments.Any(), cancellationToken);

            return RequestResponse<bool>.OK(hasEnrollments);

        }
    }
}
