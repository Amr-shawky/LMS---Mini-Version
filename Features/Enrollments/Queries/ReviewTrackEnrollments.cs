using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.DTOs;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record ReviewTrackEnrollments(int trackId) : IRequest<RequestResponse<IEnumerable<ReviewTrackEnrollmentsDto>>>;

    public class ReviewTrackEnrollmentsHandler : IRequestHandler<ReviewTrackEnrollments, RequestResponse<IEnumerable<ReviewTrackEnrollmentsDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        private readonly IGeneralRepository<Payment> _paymentRepo;
        private readonly IGeneralRepository<Track> _trackRepo;
        private readonly IGeneralRepository<Intern> _internRepo;

        public ReviewTrackEnrollmentsHandler(IGeneralRepository<Enrollment> enrollmentRepo, IGeneralRepository<Payment> paymentRepo,IGeneralRepository<Track> trackRepo, IGeneralRepository<Intern> internRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _paymentRepo = paymentRepo;
            _trackRepo = trackRepo;
            _internRepo = internRepo;
        }

        public async Task<RequestResponse<IEnumerable<ReviewTrackEnrollmentsDto>>> Handle(ReviewTrackEnrollments query,CancellationToken cancellationToken)
        {
            var track = _trackRepo.GetById(query.trackId);
            if (track == null)
                return RequestResponse<IEnumerable<ReviewTrackEnrollmentsDto>>.Fail();

            var enrollments = _enrollmentRepo.GetTable().Where(e => e.TrackId == query.trackId)
            .Include(e => e.Intern)
            .ToList();

            var result = new List<ReviewTrackEnrollmentsDto>();
            
            foreach (var i in enrollments)
            {
                var payments = await _paymentRepo.GetTable()
                    .Where(p => p.EnrollmentId == i.Id)
                    .FirstOrDefaultAsync();

                result.Add(new ReviewTrackEnrollmentsDto
                {
                    EnrollmentId = i.Id,
                    InternFullName = i.Intern.FullName,
                    EnrollmentDate = i.EnrollmentDate,
                    enrollmentStatus = i.Status,
                    PaymentAmount = payments?.Amount ?? 0.0m,
                    paymentStatus = payments?.Status.ToString()
                });
            }
            return RequestResponse<IEnumerable<ReviewTrackEnrollmentsDto>>.OK(result);
        
        }
    }
}
