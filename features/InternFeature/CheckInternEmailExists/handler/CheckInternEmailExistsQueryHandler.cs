using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.handler
{
    public class CheckInternEmailExistsQueryHandler : IRequestHandler<CheckInternEmailExistsQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public CheckInternEmailExistsQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<bool>> Handle(CheckInternEmailExistsQuery request, CancellationToken cancellationToken)
        {
            var emailExists = await _repository.GetAll().AnyAsync(i => i.Email.ToLower() == request.Email.ToLower(), cancellationToken);
            if (emailExists)
            {
                return RequestResponse<bool>.Fail(409, new Dictionary<string, string[]> { { "Email", new[] { "Email already exists." } } });
            }
            return RequestResponse<bool>.OK(emailExists);
        }
    }
}
