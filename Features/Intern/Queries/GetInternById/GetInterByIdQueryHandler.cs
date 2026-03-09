using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Intern.Queries.GetInternById
{
    public class GetInterByIdQueryHandler : IRequestHandler<GetInterByIdQuery, InternDto?>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetInterByIdQueryHandler(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;

        }
        public async Task<InternDto?> Handle(GetInterByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await unitOfWork.Interns
                .GetTable()
                .Include(i => i.Track)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

                return intern?.ToDto();



        }
    }
}
