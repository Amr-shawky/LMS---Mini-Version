using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.TrackFeature.Handlers
{
    public class GetActiveEnrollmentCountByTrackQueryHandler : IRequestHandler<GetActiveEnrollmentCountByTrackQuery,RequestResponse<int>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        public GetActiveEnrollmentCountByTrackQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<int>> Handle(GetActiveEnrollmentCountByTrackQuery request, CancellationToken cancellationToken)
        {
            var count =await _repository.GetAll()
                .Where(x => x.TrackId == request.Id && x.Track.IsActive).CountAsync();


            return RequestResponse<int>.OK(200,count);
        }
    }
}
