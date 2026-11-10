using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollementFeature.Commands;
using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollementFeature.Handler
{
    public class CreateEnrollementCommandHandler : IRequestHandler<CreateEnrollementCommand, RequestResponse<CreateEnrollementDTO>>
    {
        private readonly IGeneralRepository<Enrollment> enrollementrepo;
        private readonly IUnitOfWork unitOfWork;

        public CreateEnrollementCommandHandler(IGeneralRepository<Enrollment> enrollementrepo,IUnitOfWork unitOfWork)
        {
            this.enrollementrepo = enrollementrepo;
            this.unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<CreateEnrollementDTO>> Handle(CreateEnrollementCommand request, CancellationToken cancellationToken)
        {
            var enrollement = new Enrollment()
            {
                InternId = request.internId,
                TrackId = request.trackId,
                EnrollmentDate = request.date,
                Status = request.status,

            };
            enrollementrepo.Add(enrollement);
            var res = await unitOfWork.SaveChangesAsync();
            int id = await enrollementrepo.GetAll().Where(x => x.InternId == request.internId && x.InternId == request.internId).Select(x => x.Id).FirstOrDefaultAsync();
            var dto = new CreateEnrollementDTO()
            {
                Id = id
            };

            return res > 0 ? RequestResponse<CreateEnrollementDTO>.OK(dto , 200) : RequestResponse<CreateEnrollementDTO>.Fail(400);
            
        }
    }
}
