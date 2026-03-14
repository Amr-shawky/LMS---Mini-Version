using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Commands;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, InternSummaryViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeneralRepository<Intern> _internRepository;
        public CreateInternCommandHandler(IUnitOfWork unitOfWork, IGeneralRepository<Intern> internRepository)
        {
            _unitOfWork = unitOfWork;
            _internRepository = internRepository;
        }
        public async Task<InternSummaryViewModel> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var intern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                Status = request.status,
                TrackId = request.TrackId
            };

            _internRepository.Add(intern);
            await _unitOfWork.CompleteAsync();

            return intern.ToDto().ToSummaryViewModel();
        }
    }
}
