using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.Handler
{
    public class CheckInternEmailExistsHandler : IRequestHandler<CheckInternEmailExistsQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Intern> internRepo;
        public CheckInternEmailExistsHandler(IGeneralRepository<Intern> internRepo)
        {
            this.internRepo = internRepo;
        }
        public async Task<RequestResponse<bool>> Handle(
            CheckInternEmailExistsQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.InternEmail))
            {
                return RequestResponse<bool>.Fail(
                     400,
                     new Dictionary<string, string[]>
                     {
                         ["InternEmail"] = new[] { "Intern email is required." }
                     });
            }
            var email = request.InternEmail.Trim().ToLowerInvariant();
            var emailExists = await internRepo.GetAll()
                .AnyAsync(
                    i => i.Email.ToLower() == email,
                    cancellationToken);
            return RequestResponse<bool>.OK(emailExists);
        }
    
    }
}
