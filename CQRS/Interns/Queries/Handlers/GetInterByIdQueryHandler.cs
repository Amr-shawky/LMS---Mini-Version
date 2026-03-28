namespace LMS___Mini.CQRS.Interns.Queries.Handlers
{
    public class GetInterByIdQueryHandler :IRequestHandler<GetInternByIdQuery , InternDto?>
    {
        private readonly IUnitOfWork _UnitOf;
        public GetInterByIdQueryHandler(IUnitOfWork unitOf)
        {
            _UnitOf = unitOf;
        }

        public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
           var intern =  await _UnitOf.Interns.GetWithTrackAsync(request.id);
            return intern?.ToDto();
        }
    }
}
