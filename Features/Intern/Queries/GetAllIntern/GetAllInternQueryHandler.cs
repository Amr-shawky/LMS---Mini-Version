using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Intern.Queries.GetAllIntern
{
    public class GetAllInternQueryHandler : IRequestHandler<GetAllInternQuery, IEnumerable<InternDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllInternQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<InternDto>> Handle(GetAllInternQuery request, CancellationToken cancellationToken)
        {
            var interns = await _unitOfWork.Interns
                 .GetTable()
                 .Include(i => i.Track)
                 .ToListAsync()
                 .ConfigureAwait(false);

            return interns.Select(i => i.ToDto());
        }
    }
}
