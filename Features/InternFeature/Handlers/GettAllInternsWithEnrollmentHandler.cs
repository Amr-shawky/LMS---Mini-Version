using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.InternFeature.Dtos;
using LMS___Mini_Version.Features.InternFeature.Quries;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.InternFeature.Handlers
{
    public class GettAllInternsWithEnrollmentHandler : IRequestHandler<GettAllInternsWithEnrollment, RequestResponse<IEnumerable<Interndto>>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public GettAllInternsWithEnrollmentHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;

        }
        public async Task<RequestResponse<IEnumerable<Interndto>>> Handle(GettAllInternsWithEnrollment request, CancellationToken cancellationToken)
        {

            var query = await _repository.GetAll()
                .Where(x => x.Status != "Pending")
                .Skip((request.pageindex -1) * request.pagesize)
                .Take(request.pagesize)
                .Select(x => new Interndto
                {
                    Email = x.Email,
                    FullName = x.FullName,
                    Enrollments = x.Enrollments,
                    EnrollmentDate = x.Enrollments.Select(n => n.EnrollmentDate).ToList(),
                    EnrollmentCount = x.Enrollments.Count(),
                    TrackName = x.Track.Name
                }).ToListAsync();


            

            var query1 =
               from i in _repository.GetAll().AsNoTracking()
               where i.Status != "Pending"

               from e in i.Enrollments.DefaultIfEmpty()

               group e by new
               {
                   i.Id,
                   i.FullName,
                   i.Email,
                   TrackName = i.Track.Name
               }
                into g

               select new Interndto
               {
                   FullName = g.Key.FullName,
                   Email = g.Key.Email,
                   TrackName = g.Key.TrackName,

                   EnrollmentCount = g.Count(e => e != null)
               };

            return RequestResponse<IEnumerable<Interndto>>.OK(200, query);

        }
    }
}
