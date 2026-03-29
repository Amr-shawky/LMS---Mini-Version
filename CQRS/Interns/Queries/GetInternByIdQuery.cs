using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Queries
{
    public record GetInternByIdQuery(int id) : IRequest<InternDto?>;

    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDto?>
    {
        private readonly IInternService _internService;

        public GetInternByIdQueryHandler(IInternService internService)
        {
            _internService = internService;
        }

        public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            return await _internService.GetByIdAsync(request.id);
        }
    }
}
