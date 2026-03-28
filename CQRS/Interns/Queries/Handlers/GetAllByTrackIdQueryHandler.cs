namespace LMS___Mini.CQRS.Interns.Queries.Handlers
{
    public class GetAllByTrackIdQueryHandler : IRequestHandler<GetAllByTrackIdQuery, IEnumerable<InternDto>>
    {
        private readonly IUnitOfWork _unitOf;
        public GetAllByTrackIdQueryHandler(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<IEnumerable<InternDto>> Handle(GetAllByTrackIdQuery request, CancellationToken cancellationToken)
        {
            var interns = await _unitOf.Interns.GetAllByTrackIdAsync(request.trackId);
            return interns.Select(i => i.ToDto());
        }
    }
}
