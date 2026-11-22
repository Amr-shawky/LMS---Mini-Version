using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.AddIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeatures.AddIntern.Handlers
{
    public class AddInternCommandHandler(IGeneralRepository<Intern> Internrepository,IUnitOfWork unitOfWork) : IRequestHandler<AddInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> InternRepository = Internrepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<RequestResponse<int>> Handle(AddInternCommand request, CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToLower();
            var emailExists = await InternRepository.GetAll().AnyAsync(i => i.Email.Trim().ToLower() == normalizedEmail);

            if (emailExists)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
                {
                    [nameof(request.Email)] = new[] { "Email already exists" }
                });
            }

            var Intern = new Intern
            {
                FullName = request.FullName,
                Email = normalizedEmail,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };
            InternRepository.Add(Intern);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(data: Intern.Id,statusCode: 201);
        }
    }
}
