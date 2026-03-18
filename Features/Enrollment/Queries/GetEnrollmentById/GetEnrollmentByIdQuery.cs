using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.Queries.GetEnrollmentById
{
    public record GetEnrollmentByIdQuery(int id):IRequest<EnrollmentDto?>
    {
    }
}
