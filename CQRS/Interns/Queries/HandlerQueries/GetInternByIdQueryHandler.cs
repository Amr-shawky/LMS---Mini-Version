using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Interns.Queries.HandlerQueries
{
    public class GetInternByIdQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetInternByIdQuery, InternDetailViewModel>
    {
        public async Task<InternDetailViewModel> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
           var intern = await _unitOfWork.Interns.GetTable().Include(i=>i.Track).FirstOrDefaultAsync(i => i.Id == request.Id);
            if (intern == null)
                throw new Exception("Intern is not exist");
            
            var result = intern.ToDto().ToDetailViewModel();

            return result;
        }
    }
}
