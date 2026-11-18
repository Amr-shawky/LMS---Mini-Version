using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Handlers
{
    public class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteEnrollmentCommandHandler(IGeneralRepository<Enrollment> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _repository.GetByIdAsync(request.id);
            enrollment!.Status = EnrollmentStatus.Completed;
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
