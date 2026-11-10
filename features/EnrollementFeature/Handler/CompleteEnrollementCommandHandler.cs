using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollementFeature.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollementFeature.Handler
{
    public class CompleteEnrollementCommandHandler : IRequestHandler<CompleteEnrollementCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> enrollementRepo;
        private readonly IUnitOfWork unitOfWork;

        public CompleteEnrollementCommandHandler(IGeneralRepository<Enrollment> enrollementRepo , IUnitOfWork unitOfWork)
        {
            this.enrollementRepo = enrollementRepo;
            this.unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(CompleteEnrollementCommand request, CancellationToken cancellationToken)
        {

            var enrollement = await enrollementRepo.GetAll().Where(x => x.Id == request.enrollementId).Select(x => new Enrollment
            {
                Id = x.Id,
                Payment = x.Payment,
                EnrollmentDate = x.EnrollmentDate,
                Intern = x.Intern,
                Status = x.Status,
                InternId = x.InternId,
                Track = x.Track,
                TrackId = x.TrackId,

            }).FirstOrDefaultAsync();
            if (enrollement == null)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement" , new[]{" Cannot Find Enrollement with that id"} }
                };
                return RequestResponse.Fail(404, erros);

            }
             if (enrollement.Status != Domain.Enums.EnrollmentStatus.Active)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement Status" , new[]{" Cannot Complete Enrollement with InActive Status"} }
                };
                return RequestResponse.Fail(400, erros);
            }
             if(enrollement.Payment == null || enrollement.Payment.Status== Domain.Enums.PaymentStatus.Failed || enrollement.Payment.Status == Domain.Enums.PaymentStatus.Refunded || enrollement.Payment.Status == Domain.Enums.PaymentStatus.Pending)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement Payment" , new[]{" Cannot Complete Enrollement with Pending or Refunded or Failed Payment Status"} }
                };
                return RequestResponse.Fail(400, erros);
            }
             if(enrollement.Status == Domain.Enums.EnrollmentStatus.Completed || enrollement.Status == Domain.Enums.EnrollmentStatus.Cancelled)
            {
                IDictionary<string, string[]> erros = new Dictionary<string, string[]>()
                {
                    {"Enrollement Status" , new[]{" Cannot Complete Enrollement with Completed or Cancelled Status"} }
                };
                return RequestResponse.Fail(400, erros);
            }
             enrollement.Status = Domain.Enums.EnrollmentStatus.Completed;
            enrollementRepo.Update(enrollement);
            var res = await unitOfWork.SaveChangesAsync();
            if(res>0)
            {
                return RequestResponse.OK(200);
            }
            else
            {
                return RequestResponse.Fail(400);
            }

              

     


            

          
        }
    }
}
