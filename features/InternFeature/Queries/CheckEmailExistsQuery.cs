using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Queries
{
    public record CheckEmailExistsQuery(string email): IRequest<RequestResponse<bool>>
    {
    }

    public class CheckEmailExistsQueryHandler : IRequestHandler<CheckEmailExistsQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public CheckEmailExistsQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<bool>> Handle(CheckEmailExistsQuery request, CancellationToken cancellationToken)
        {
            var email = request.email.Trim().ToLower();
            var emailExists = await _repository.GetAll()
                .AnyAsync(i => i.Email.ToLower() == email);
            
            return RequestResponse<bool>.OK(emailExists);
        }
    }
}
