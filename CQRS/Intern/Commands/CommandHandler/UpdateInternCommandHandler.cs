using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Commands.CommandHandler
{
    public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand, ResultResponse<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateInternCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResultResponse<bool>> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
        {
            var intern =await unitOfWork.Interns.GetByIdAsync(request.id);
            if (intern == null)
            {
                return ResultResponse<bool>.Faild("The Intern Not Found");
            }
            intern.FullName = request.FullName;
            intern.Email = request.Email;
            intern.BirthYear = request.BirthYear;
            intern.Status = request.Status;
            intern.TrackId = request.TrackId;

            unitOfWork.Interns.Update(intern);

            try
            {
               await unitOfWork.CompleteAsync();
            }
            catch(DbUpdateException ex)
            {
                return ResultResponse<bool>.Faild(ex.Message);

            }
            return ResultResponse<bool>.Success(true,"the intern update sucesfully");


        }
    }
}
