using LMS___Mini_Version.Common;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Enrollment.GetAllEnrollment.DTOs;
using LMS___Mini_Version.Features.Intern.GetInternById.Dtos;
using LMS___Mini_Version.Features.Track.GetTrackById.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace LMS___Mini_Version.Features.Enrollment.GetAllEnrollment.Queries
{
    public record GetAllEnrollmentsQuery(int pageIndex,int pageSize) : IRequest<IEnumerable<GetAllEnrollmentsDto>>;

    public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, IEnumerable<GetAllEnrollmentsDto>>
    {
        private readonly IGenericRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int> _genericRepository;
        public GetAllEnrollmentsQueryHandler(IGenericRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IEnumerable<GetAllEnrollmentsDto>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {
          var enrollments=  _genericRepository.GetTable(request.pageIndex,request.pageSize).Include(e=>e.Intern).ThenInclude(s=>s.Track).Include(e=>e.Track);


            var enrollmentsDto = new List<GetAllEnrollmentsDto>();
            foreach( var enrollment in enrollments)
            {
                enrollmentsDto.Add(new GetAllEnrollmentsDto()
                {
                    Id = enrollment.Id,
                    PaymentStatus = enrollment.PaymentStatus,
                    InternDto = new GetInternDto()
                    {
                        FullName = enrollment.Intern.FullName,
                        Id = enrollment.Intern.Id,
                        TrackId = enrollment.Intern.TrackId,
                        TrackName = enrollment.Intern.Track.Name
                    },
                    TrackId = enrollment.TrackId,
                    TrackDto = new GetTrackDto()
                    {
                        Fees = enrollment.Track.Fees,
                        Name = enrollment.Track.Name,
                        IsActive = enrollment.Track.IsActive

                    },
                    
                });

                
            }
            return enrollmentsDto;
        }
    }
}
