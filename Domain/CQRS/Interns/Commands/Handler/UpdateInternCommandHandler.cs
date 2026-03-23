using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Interns.Commands.Handler
{
    public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, bool>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public UpdateInternCommandHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
        {
            var intern =await _repository.GetById(request.Id);
            if (intern == null)
            {
                throw new NotImplementedException();
            }
            intern.FullName=request.FullName;
            intern.Email = request.Email;
            intern.Status = request.Status;
            intern.TrackId= request.TrackId;

            _repository.Update(intern);
            return true;
        }
    }
}
