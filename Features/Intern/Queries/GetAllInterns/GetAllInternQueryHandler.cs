
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Intern.Queries.GetAllInterns

{
    public class GetAllInternQueryHandler : IRequestHandler<GetAllInternQuery, IEnumerable<InternDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllInternQueryHandler(IUnitOfWork unitOfWork  )
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InternDto>> Handle(GetAllInternQuery request, CancellationToken cancellationToken)
        {
            var interns = await unitOfWork.Interns
                 .GetTable()
                 .Include(i => i.Track)
                 .ToListAsync(cancellationToken);

            return interns.Select(i => i.ToDto());

        }
    }
}
