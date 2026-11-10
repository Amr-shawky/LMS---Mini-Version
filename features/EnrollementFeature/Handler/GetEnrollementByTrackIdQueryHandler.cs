using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTO;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Query;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LMS___Mini_Version.features.EnrollementFeature.Handler
{
    public class GetEnrollementByTrackIdQueryHandler : IRequestHandler<GetEnrollementByTrackIdQuery, RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>>
    {
        private readonly IGeneralRepository<Enrollment> enrollementRepo;
        private readonly IMediator mediator;

        public GetEnrollementByTrackIdQueryHandler(IGeneralRepository<Enrollment> enrollementRepo ,IMediator mediator )
        {
            this.enrollementRepo = enrollementRepo;
            this.mediator = mediator;
        }
        public async Task<RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>> Handle(GetEnrollementByTrackIdQuery request, CancellationToken cancellationToken)
        {
            var trackResponse = await mediator.Send(new GetTrackByIdQuery(request.trackId));
            if (!trackResponse.Success || trackResponse.Data == null)
            {
                return RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>.Fail(trackResponse.StatusCode , trackResponse.Errors);
            }
            else
            {
                var trackdto = trackResponse.Data;
                var res = await  enrollementRepo.GetAll().Where(x => x.TrackId == trackdto.Id)
               .Select(x => new EnrollementByTrackIdDTO()
                {
                    TrackId = x.TrackId,
                    enrollementStatus = x.Status,
                    InternFullName = x.Intern.FullName,
                     EnrollementId = x.Id ,
                     EnrollmentDate = x.EnrollmentDate,
                     PaymentStatus = x.Payment == null ? null : x.Payment.Status,
                     PaymentAmount = x.Payment == null? null : x.Payment.Amount,

                }).ToListAsync();

                return res.Any() ? RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>.OK(res, 200) : RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>.OK([], 200);


            }
        }
    }

}
