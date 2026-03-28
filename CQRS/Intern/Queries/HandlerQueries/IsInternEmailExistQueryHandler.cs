using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Queries.HandlerQueries
{
    public class IsInternEmailExistQueryHandler : IRequestHandler<IsInternEmailExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        public async Task<RequestResult<bool>> Handle(IsInternEmailExistQuery request, CancellationToken cancellationToken)
        {
            var intern = await _uow.Interns.GetTable().AnyAsync(intern => intern.Email == request.email);
            return RequestResult<bool>.Success(intern);
        }
    }
}
