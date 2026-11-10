using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LMS___Mini_Version.features.EnrollementFeature.Handler
{
    public class GetEnrollementWithInternIdQueryHandler : IRequestHandler<GetEnrollementWithInternIdQuery, RequestResponse<IEnumerable<EnrollementByInternIdDTO>>>
    {
        private readonly IGeneralRepository<Enrollment> enrollementRepo;
        private readonly IMediator mediator;

        public GetEnrollementWithInternIdQueryHandler(IGeneralRepository<Enrollment> enrollementRepo , IMediator mediator)
        {
            this.enrollementRepo = enrollementRepo;
            this.mediator = mediator;
        }
        public async Task<RequestResponse<IEnumerable<EnrollementByInternIdDTO>>> Handle(GetEnrollementWithInternIdQuery request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetInternByIdQuery(request.internId));
            if (!response.Success || response.Data is null)
                return RequestResponse<IEnumerable<EnrollementByInternIdDTO>>.Fail(response.StatusCode, response.Errors);

          var res = await enrollementRepo.GetAll().Where(x => x.InternId == request.internId).Select(x => new EnrollementByInternIdDTO
            {
                EnrollmentDate = x.EnrollmentDate,
                EnrollmentId = x.Id,
                Status = x.Status,
                TrackId = x.TrackId,
                TrackName = x.Track.Name

            }).OrderByDescending(x => x.EnrollmentDate).ToListAsync();

            return res.Any() ?  RequestResponse<IEnumerable<EnrollementByInternIdDTO>>.OK(res, 200) :  RequestResponse<IEnumerable<EnrollementByInternIdDTO>>.OK([], 200);
        }
    }
}
