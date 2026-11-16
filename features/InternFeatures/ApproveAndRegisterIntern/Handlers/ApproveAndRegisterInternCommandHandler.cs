using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.ApproveAndRegisterIntern.Commands;
using LMS___Mini_Version.features.InternFeatures.GetallIntern.ViewModels;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS___Mini_Version.features.InternFeatures.ApproveAndRegisterIntern.Handlers;

public class ApproveAndRegisterInternCommandHandler
    (IGeneralRepository<Intern> Internrepository,
    IGeneralRepository<Enrollment> Enrollmentrepo,
    IGeneralRepository<Payment> Paymentmentrepo,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ApproveAndRegisterInternCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(ApproveAndRegisterInternCommand request, CancellationToken cancellationToken)
    {
        var intern = await Internrepository.GetAll()
            .Include(x => x.Track)
            .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

        
        if (intern is null)
        {
            return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
            {
                ["Intern"] = ["Intern not found."]
            });
        }

        if (intern.Status is not "Pending")
        {
            return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
            {
                ["Status"] = ["Only pending interns can be approved."]
            });
        }
        if (intern.Track is  null)
        {
            return RequestResponse<int>.Fail(404, new Dictionary<string, string[]>
            {
                ["TrackId"] = ["Track not found or inactive."]
            });
        }

        if (!intern.Track.IsActive)
            return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>
            {
                ["TrackId"] = ["Track is not active."]
            });
        var reservedSeats = await Enrollmentrepo.GetAll().CountAsync(e =>
    e.TrackId == intern.TrackId &&
    (e.Status == EnrollmentStatus.Active || e.Status == EnrollmentStatus.Pending),
    cancellationToken);

        if (reservedSeats >= intern.Track.MaxCapacity)
            return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
            {
                ["Track"] = ["Track has no available seats."]
            });

        //ما تعملش Enrollment تاني بحالة Pending أو Active لنفس المتدرب ونفس الـ Track.

        var HasOpenEnrollment = await Enrollmentrepo.GetAll().AnyAsync(e =>
              e.InternId == intern.Id &&
              e.TrackId == intern.TrackId &&
               (e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active),
                cancellationToken);

        if (HasOpenEnrollment)
        {
            return RequestResponse<int>.Fail(409, new Dictionary<string, string[]>
            {
                ["Enrollment"] =
                ["Intern already has a pending or active enrollment in this track."]
            });
        }

        // 6) Create everything, then ONE SaveChanges (atomic) == transaction

        var enrollment = new Enrollment{
            InternId = intern.Id,
            EnrollmentDate = DateTime.Now,
            Status = EnrollmentStatus.Active,
            TrackId = intern.TrackId,
        
        };

        var payment = new Payment
        {
            Enrollment = enrollment,
            Amount = intern.Track.Fees,
            Method = request.PaymentMethod,
            Status = PaymentStatus.Completed,
            PaymentDate = DateTime.Now,
        };

        intern.Status = "Accepted";

        Enrollmentrepo.Add(enrollment);
        Paymentmentrepo.Add(payment);

        await unitOfWork.SaveChangesAsync();

        return RequestResponse<int>.OK(enrollment.Id, 201);

    }
}
