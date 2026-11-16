using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeatures.UpdateStatusEnrollment.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeatures.UpdateStatusEnrollment.Handlers;

public class UpdateStatusEnrollmentCommandHandler(IGeneralRepository<Enrollment> _enrolmentRepo,IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateStatusEnrollmentCommand, RequestResponse>
{
    
    public async Task<RequestResponse> Handle(UpdateStatusEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enroll = await _enrolmentRepo.GetAll().
             Include(x  =>x.Payment).
             FirstOrDefaultAsync(x=>x.Id == request.id ,cancellationToken);


        //var enroll = await _enrolmentRepo.GetByIdAsync(request.id);
        if (enroll is null)
        {
            return RequestResponse.Fail(404, new Dictionary<string, string[]>
            {
                ["Enrollment"] = ["Enrollment not found"]
            });
        }

        if (enroll.Status != EnrollmentStatus.Active)
        {
            return RequestResponse.Fail(409, new Dictionary<string, string[]>
            {
                ["EnrollmentStatus"] =
                [$"{enroll.Status} ِalready applied"]
            });
        }

        
        
        if(enroll.Payment?.Status != PaymentStatus.Completed )
        {
            return RequestResponse.Fail(404, new Dictionary<string, string[]>
            {
                ["CustomeError"] = ["not valid paramter"]
            });
        }
        enroll.Status = EnrollmentStatus.Completed;
        await unitOfWork.SaveChangesAsync();

        return RequestResponse.OK(200);

    }
}
