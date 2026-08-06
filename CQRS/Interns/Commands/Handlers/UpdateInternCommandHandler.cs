namespace LMS___Mini.CQRS.Interns.Commands.Handlers
{
    public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, int>
    {
        private readonly UpdateInternValidator _validator;
        private readonly IUnitOfWork _unitOf;
        public UpdateInternCommandHandler(UpdateInternValidator validator , IUnitOfWork unitOf)
        {
            _validator = validator;
            _unitOf = unitOf;
        }

        public async Task<int> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
        {
            var error = await _validator.ValidateAsync(request);
            if (error != null) throw new InvalidOperationException();

            var existingIntern = await _unitOf.Interns.GetByIdAsync(request.InternId);

            existingIntern.FullName = request.Name;
            existingIntern.Email = request.Email;
            existingIntern.PhoneNumber = request.phone;
            existingIntern.TrackId = request.TrackId;

            _unitOf.Interns.Update(existingIntern); // update exist intern with new values
            await _unitOf.CompleteAsync();

            return existingIntern.Id;
        }

    }
}
