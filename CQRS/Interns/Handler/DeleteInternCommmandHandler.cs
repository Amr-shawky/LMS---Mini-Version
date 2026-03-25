using LMS___Mini_Version.CQRS.Interns.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Handler
{
    public class DeleteInternCommmandHandler : IRequestHandler<DeleteInternCommand, bool>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public DeleteInternCommmandHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _repository.GetById(request.Id);
            if (intern == null)
                throw new NotImplementedException();

            _repository.Delete(intern.Id);
            return true;
        }
    }
}
