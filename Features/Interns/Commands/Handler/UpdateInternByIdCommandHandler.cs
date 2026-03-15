using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Errors.Exeptions;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.Handler
{
    public class UpdateInternByIdCommandHandler : IRequestHandler<UpdateInternByIdCommand,Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInternByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateInternByIdCommand request, CancellationToken cancellationToken)
        {
            var intern = await _unitOfWork.Interns.GetByIdAsync(request.Id);

            if (intern == null) 
                throw new NotFoundException("Intern", request.Id);


            intern.FullName = request.Vm.FullName;
            intern.Email = request.Vm.Email;
            intern.BirthYear = request.Vm.BirthYear;
            intern.Status = request.Vm.Status;
            intern.TrackId = request.Vm.TrackId;





            _unitOfWork.Interns.Update(intern);

           
            await _unitOfWork.CompleteAsync();

            
            return Unit.Value;


        }

       
    }
}
