using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries
{
    public record GetEnrollmentQuery(int id): IRequest<RequestResponse<EnrollmentDto>>
    {
    }

    public class GetEnrollmentQueryHandler : IRequestHandler<GetEnrollmentQuery, RequestResponse<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetEnrollmentQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<EnrollmentDto>> Handle(GetEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var info = await _repository.GetAll()
                .Where(e => e.Id == request.id)
                .Select(e => new
                {
                    e.Status,
                    PaymentStatus = e.Payment == null ? null : e.Payment.Status.ToString(),
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (info is null)
                return RequestResponse<EnrollmentDto>.Fail(ErrorCode.EnrollmentNotFound, 404);

            if (string.IsNullOrEmpty(info.PaymentStatus))
                return RequestResponse<EnrollmentDto>.Fail(ErrorCode.PaymentNotFound, 404);

            
            return RequestResponse<EnrollmentDto>.OK(new EnrollmentDto 
            { EnrollmentStatus = info.Status, PaymentStatus = info.PaymentStatus });
        }
    }
}
