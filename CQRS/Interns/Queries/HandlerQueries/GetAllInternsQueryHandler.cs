using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Interns.Queries.HandlerQueries
{
    public class GetAllInternsQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAllInternsQuery, IEnumerable<InternSummaryViewModel>>
    {
        public async Task<IEnumerable<InternSummaryViewModel>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns =  _unitOfWork.Interns.GetTable().Include(i=>i.Track).ToList();
            return interns.Select(i => i.ToDto().ToSummaryViewModel());
        }
    }
}
