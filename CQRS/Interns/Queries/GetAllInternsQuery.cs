using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Queries
{
    public record GetAllInternsQuery : IRequest<IEnumerable<InternDto>>;

    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>
    {
        private readonly IInternService _internService;

        public GetAllInternsQueryHandler(IInternService internService)
        {
            _internService = internService;
        }

        public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            return await _internService.GetAllAsync();
        }
    }
}
