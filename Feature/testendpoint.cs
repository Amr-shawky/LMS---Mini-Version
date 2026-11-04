using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Feature
{
    public static class testendpoint
    {
        public static void maptestendpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("test/v2/{id}", async (IMediator mediator, [AsParameters] testviewmodel vm) =>
            {

                var result = await mediator.Send(new GetAllTrackQuery());
                var listofdto = result.Data.Items;
                
                var listviewmodel = listofdto.Select(t => new TrackSummaryViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Fees = t.Fees,
                    IsActive = t.IsActive
                }).ToList();


                int count = result.Data.TotalCount;
                int pageindex = result.Data.PageIndex;
                int pagesize = result.Data.PageSize;
                var paginatedresultvm = PaginatedResult<TrackSummaryViewModel>.Create(listviewmodel, count, pageindex, pagesize);

                return result;
            });
        }

        public class testviewmodel
        {
            public int id { set; get; }
            public string name { set; get; }


        }
    }
}
