using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class CheckInternExistsQueryHandler:IRequestHandler<CheckInternExistsQuery,RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Intern> _internRepo;

        public CheckInternExistsQueryHandler(IGeneralRepository<Intern> internRepo)=>
            _internRepo = internRepo;

        public async Task<RequestResponse<bool>> Handle(CheckInternExistsQuery request,CancellationToken cancellationToken)
        {
            var internExists = await _internRepo.GetAll()
                .AnyAsync(i => i.Id == request.internId,cancellationToken);
            return RequestResponse<bool>.Ok(internExists);
        }
    }
}
