using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.Commands.CreateIntern
{
    public class CreateInternCommandHandler: IRequestHandler<CreateInternCommand, InternDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateInternCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<InternDto> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Intern()
            {
                FullName = request.dto.FullName,
                Email = request.dto.Email,
                BirthYear = request.dto.BirthYear,
                Status = request.dto.Status,
                TrackId = request.dto.TrackId
            };

              _unitOfWork.Interns.Add(entity);
            return entity.ToDto();  
        }
    }
}
