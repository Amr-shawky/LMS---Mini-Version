using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Commands.CommandHandler
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, ResultResponse<String>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateInternCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResultResponse<String>> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            
            var intern = new Domain.Entities.Intern
            {
                FullName = request.fullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                Status = request.Status,
                TrackId = request.TrackId,

            };

            unitOfWork.Interns.Add(intern);
            try
            {
                await unitOfWork.CompleteAsync();

            }
            catch (DbUpdateException ex)
            {
                return ResultResponse<String>.Faild(ex.Message);
                
            }
            return ResultResponse<String>.Success(intern.Id.ToString());

        }
    }
}
