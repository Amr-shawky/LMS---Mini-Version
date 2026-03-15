using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Errors.Exeptions;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Queries.Handlers
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDetailViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllInternsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       public async Task<IEnumerable<InternDetailViewModel>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = await _unitOfWork.Interns.GetTable()
                .Include(i => i.Track) 
                .ToListAsync(cancellationToken);

            if (!interns.Any())
                throw new NotFoundException("Intern", "All");

            var internsDto = interns.Select(i => new InternDto
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                Status = i.Status,
                BirthYear = i.BirthYear,
                TrackId = i.TrackId,
                TrackName = i.Track?.Name
            }).ToList();

            var internsView = internsDto.Select(dto => dto.ToDetailViewModel());

            return (IEnumerable<InternDetailViewModel>)internsView;

        }
    }
}
