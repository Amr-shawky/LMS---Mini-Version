using LMS___Mini_Version.CQRS.Enrollment.Queries;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Mediators;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [Trap 5 Fix] The POST action delegates to EnrollInternMediator — the action coordinator.
    ///              The Controller does NOT orchestrate multi-step business logic itself.
    /// [Trap 6 Fix] The Mediator handles the atomic commit via UoW.CompleteAsync().
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator mediator;

        public EnrollmentController( IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetAll(int page,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllEnrollmentQuery(cancellationToken,page));
            if (result.IsSuccess)
            {
                var viewModels = result.Data.Select(e => e.ToViewModel());
                return Ok(viewModels);
            }
            return BadRequest(result.Message);

        }

        [HttpGet("GetById")]
        public async Task<ActionResult<EnrollmentViewModel>> GetById(int id,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetByIdEnrollmentQuery(id,cancellationToken));
            if (result.IsSuccess)
            {
                var viewModels = result.Data.ToViewModel();
                return Ok(viewModels);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetByIntern(int internId,int page , CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetByInternIdQuery(internId,cancellationToken, page));
            if (result.IsSuccess)
            {
                var viewModels = result.Data.Select(e => e.ToViewModel());
                return Ok(viewModels);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Enrolls an intern in a track. This is a multi-step action orchestrated by the Mediator:
        ///   1. Validates intern & track
        ///   2. Checks capacity
        ///   3. Creates enrollment + payment (if paid track)
        ///   4. Commits atomically via UoW
        /// </summary>
        //[HttpPost]
        //public async Task<ActionResult<EnrollmentViewModel>> Enroll(EnrollInternViewModel vm)
        //{
        //    var result = await _mediator.ExecuteAsync(new CreateEnrollmentDto
        //    {
        //        InternId = vm.InternId,
        //        TrackId = vm.TrackId
        //    }).ConfigureAwait(false);

        //    if (!result.IsSuccess)
        //    {
        //        return BadRequest(new { error = result.ErrorMessage });
        //    }

        //    return Ok(result.Enrollment!.ToViewModel());
        //}
    }
}
