using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.CreateIntern.Handler
{
    public class CreateInternCommandHandler
        : IRequestHandler<CreateInternCommand, RequestResponse<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGeneralRepository<Intern> internRepo;

        public CreateInternCommandHandler(
            IUnitOfWork unitOfWork,
            IGeneralRepository<Intern> internRepo
           )
        {
            this.unitOfWork = unitOfWork;
            this.internRepo = internRepo;
            
        }

        public async Task<RequestResponse<int>> Handle(
            CreateInternCommand request,
            CancellationToken cancellationToken)
        {
            var intern = new Intern
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };
            internRepo.Add(intern);
            await unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(intern.Id, 201);
        }
    }
}
