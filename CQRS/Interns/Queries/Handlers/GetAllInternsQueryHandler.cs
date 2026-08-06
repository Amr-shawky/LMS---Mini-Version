namespace LMS___Mini.CQRS.Interns.Queries.Handlers
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery , IEnumerable<InternDto>>
    {
        private readonly IUnitOfWork _UnitOf;
        public GetAllInternsQueryHandler(IUnitOfWork unitOf)
        {
            _UnitOf = unitOf;
        }

        public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
           var interns = await _UnitOf.Interns.GetAllWithTrackAsync();
            return interns.Select(i => i.ToDto());
        }
    }
}
