using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateInternCommandHandler(IGeneralRepository<Intern> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<int>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            
            
            var intern = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                TrackId = request.TrackId,
                Status = "Pending"
            };

            _repository.Add(intern);

            await _unitOfWork.SaveChangesAsync();
            

            return RequestResponse<int>.OK(intern.Id);
        }
    }
}
