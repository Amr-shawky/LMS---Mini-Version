using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Enrollment.Commands
{
    public record CreateEnrollmentCommand(string track) : IRequest<bool>;

    public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, bool>
    {
        private readonly IGeneralRepository<Entities.Enrollment> _enrollmentRepository;
        public CreateEnrollmentCommandHandler(IGeneralRepository<Entities.Enrollment> repository)
        {
            _enrollmentRepository = repository;
        }
        public Task<bool> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
        {


            try
            {
                var enroll=new Entities.Enrollment()
                {
                    //Track=request.track,
                    // Set other properties as needed
                };
                _enrollmentRepository.Add(enroll);
                

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Task.FromResult(false);
            }
            throw new NotImplementedException();

        }
    }


}
