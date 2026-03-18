using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Intern.Queries.GetInternById
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInternByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {

            var intern = await _unitOfWork.Interns
                .GetTable()
                .Include(i => i.Track)
                .FirstOrDefaultAsync(i => i.Id ==request.id)
                .ConfigureAwait(false);

            return intern?.ToDto();
        }
    }
}
