using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Errors.Exeptions;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LMS___Mini_Version.Features.Interns.Commands.Handler
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, CreateInternViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateInternCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<CreateInternViewModel> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            // Validation 
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ValidationException("Full name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("Email is required");

            if (dto.TrackId <= 0)
                throw new ValidationException("TrackId is invalid");


            var entity = new Intern
            {
                FullName = dto.FullName,
                Email = dto.Email,
                BirthYear = dto.BirthYear,
                Status = dto.Status,
                TrackId = dto.TrackId
            };

            _unitOfWork.Interns.Add(entity);
            await _unitOfWork.CompleteAsync();

           return new CreateInternViewModel
            {
                
                FullName = entity.FullName,
                Email = entity.Email,
                BirthYear = entity.BirthYear,
                Status = entity.Status,
                TrackId= entity.TrackId 
            };
        }
    }
}
