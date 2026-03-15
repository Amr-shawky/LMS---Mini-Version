using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Errors.Exeptions;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Queries.Handlers
{
    public class GetByIdInternQueryHandler : IRequestHandler<GetByIdInternQuery, InternSummaryViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdInternQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<InternSummaryViewModel> Handle(GetByIdInternQuery request, CancellationToken cancellationToken)
        {
            var intern = await _unitOfWork.Interns.
                                GetTable()
                                .Include(i => i.Track)
                                .FirstOrDefaultAsync(i => i.Id == request.id, cancellationToken);

            if (intern == null)
                throw new NotFoundException("Intern", request.id);


            var internDto = new InternDto
            {
                Id = intern.Id,
                FullName = intern.FullName,
                Email = intern.Email,
                Status = intern.Status.ToString(),
                TrackName = intern.Track?.Name 
            };

           
            var internViewModel = internDto.ToSummaryViewModel();

            return internViewModel;

        }
    }
}
