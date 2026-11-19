using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.InternsFeature.Dtos;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.InternsFeature.Quries
{
    public record GetInternsWithEnrollemntQuery : IRequest<RequestResponse<IEnumerable<InternDto>>>;

    public class GetInternsWithEnrollemntQueryHandler : IRequestHandler<GetInternsWithEnrollemntQuery, RequestResponse<IEnumerable<InternDto>>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public GetInternsWithEnrollemntQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<InternDto>>> Handle(GetInternsWithEnrollemntQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAll()
                .AsSplitQuery()
                .Select(x => new InternDto
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    BirthYear = x.BirthYear,
                    TrackName = x.Track.Name,
                    CountEnrollment = x.Enrollments.Count(),
                    Enrollments = x.Enrollments

                }).ToListAsync();

            var query2 =
                from i in _repository.GetAll()
                where i.Status != "Pending"
                from e in i.Enrollments.DefaultIfEmpty()

                group e by new
                {
                    i.Id,
                    i.FullName,
                    i.Email,
                    i.BirthYear,
                    TrackName = i.Track.Name
                } into g
                select new InternDto
                {
                    BirthYear = g.Key.BirthYear,
                    Email = g.Key.Email,
                    FullName = g.Key.FullName,
                    CountEnrollment = g.Count()

                };



            return RequestResponse<IEnumerable<InternDto>>.OK(200,query);
        }
    }

}
