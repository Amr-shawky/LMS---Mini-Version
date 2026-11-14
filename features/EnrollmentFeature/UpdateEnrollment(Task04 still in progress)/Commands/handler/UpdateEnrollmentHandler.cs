//using LMS___Mini_Version.Domain.Entities;
//using LMS___Mini_Version.Domain.Enums;
//using LMS___Mini_Version.Domain.Repositories;
//using LMS___Mini_Version.features.EnrollmentFeature.UpdateEnrollment.Commands;
//using LMS___Mini_Version.features.shared;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace LMS___Mini_Version.features.EnrollmentFeature.Complete_Enrollment.Handler
//{
//    public class UpdateEnrollmentHandler
//        : IRequestHandler<UpdateEnrollmentCommand, EndpointResponse>
//    {
//        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
//        private readonly IUnitOfWork _unitOfWork;

//        public UpdateEnrollmentHandler(
//            IGeneralRepository<Enrollment> enrollmentRepository,
//            IUnitOfWork unitOfWork)
//        {
//            _enrollmentRepository = enrollmentRepository;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<EndpointResponse> Handle(
//            UpdateEnrollmentCommand request,
//            CancellationToken cancellationToken)
//        {
//            var enrollment = await _enrollmentRepository
//                .GetAll()
//                .Include(e => e.Payment)
//                .FirstOrDefaultAsync(
//                    e => e.Id == request.EnrollmentId,
//                    cancellationToken
//                );

//            if (enrollment == null)
//            {
//                return EndpointResponse.Fail(404, "Enrollment not found.");
//            }

//            return EndpointResponse.OK(200, "Enrollment found.");
//        }
//    }
//}