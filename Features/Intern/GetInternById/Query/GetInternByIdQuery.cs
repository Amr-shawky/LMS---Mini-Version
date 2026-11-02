using LMS___Mini_Version.Common;

using LMS___Mini_Version.Features.Intern.GetInternById.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Intern.GetInternById.Query
{
    public record GetInternByIdQuery(int internId) : IRequest<GetInternDto>;
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, GetInternDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetInternByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetInternDto> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Intern, int>().GetByIdAsync(request.internId);
            if(intern == null) { return null; }
            var internDto = new GetInternDto()
            {
                FullName = intern.FullName,
                Id = intern.Id,
                TrackId = intern.TrackId,
                TrackName = intern.Track.Name
            };
            return internDto;
        }
    }
}
