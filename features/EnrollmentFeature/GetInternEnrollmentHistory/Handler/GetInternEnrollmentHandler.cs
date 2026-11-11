using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Handler
{
    public class GetInternEnrollmentHandler : IRequestHandler<GetInternEnrollmentHistoryQuery, RequestResponse<IEnumerable<EnrollmentHistoryResponseDto>>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetInternEnrollmentHandler(IGeneralRepository<Intern> internRepository , IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _internRepository = internRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<IEnumerable<EnrollmentHistoryResponseDto>>> Handle(GetInternEnrollmentHistoryQuery request, CancellationToken cancellationToken)
        {
            // check if Intern is Exist
            var internExist = await _internRepository.GetAll().AnyAsync( x => x.Id == request.internId , cancellationToken);
            if(!internExist)
            {
                return RequestResponse<IEnumerable<EnrollmentHistoryResponseDto>>.Fail(404);
            }

            // Get Enrollment History
            var Enrollments = await _enrollmentRepository
                             .GetAll()
                             .Where(x => x.InternId == request.internId)
                             .OrderByDescending(x => x.EnrollmentDate)
                             .Select(x => new EnrollmentHistoryResponseDto
                                 (
                                    x.Id,
                                    x.TrackId,
                                    x.Track.Name,
                                    x.EnrollmentDate,
                                    x.Status.ToString()
                                 )
                             ).ToListAsync(cancellationToken);
            return RequestResponse<IEnumerable<EnrollmentHistoryResponseDto>>.OK(Enrollments, 200);
        }
    }
}
