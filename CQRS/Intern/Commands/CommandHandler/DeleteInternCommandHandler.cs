using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Commands.CommandHandler
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, ResultResponse<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteInternCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResultResponse<bool>> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await unitOfWork.Interns.GetByIdAsync(request.id);
            if (intern == null)
            {
                return ResultResponse<bool>.Faild("The Intern Not Found");
            }

            unitOfWork.Interns.Delete(intern);
            try
            {
                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException ex)
            {
                return ResultResponse<bool>.Faild(ex.Message);

            }
            return ResultResponse<bool>.Success(true, "the intern Deleted sucesfully");
        }
    }
}
