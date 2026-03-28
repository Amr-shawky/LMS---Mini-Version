namespace LMS___Mini.CQRS.Interns.Commands.Handlers
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, int>
    {

        private readonly IUnitOfWork _unitOf;
        private readonly CreateInternValidators _validators;
        public CreateInternCommandHandler(CreateInternValidators validators , IUnitOfWork unitOf)
        {
            _validators = validators;
            _unitOf = unitOf;
        }

        public async Task<int> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var error = _validators.ValidateAsync(request);
            if (error != null) throw new InvalidOperationException();

            var intern = new Intern()
            {
                FullName = request.Name,
                Email = request.Email,
                PhoneNumber = request.Phone,
                TrackId = request.TrackId
            };

             _unitOf.Interns.Add(intern);
            await  _unitOf.CompleteAsync();

            return intern.Id;
        }

    }
}
