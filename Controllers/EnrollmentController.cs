using LMS___Mini_Version.CQRS.Enrollments.Commands;
using LMS___Mini_Version.CQRS.Enrollments.Queries;
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
    public class EnrollmentController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetAll()
        {
           var enrolls = await _mediator.Send(new GetAllEnrollmentQuery());
            return Ok(enrolls);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentViewModel>> GetById(int id)
        {
            var enroll = await _mediator.Send(new GetEnrollmentByIdQuery(id));
            return Ok(enroll);


        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetByIntern(int internId)
        {
            var enrolls = await _mediator.Send(new GetEnrollmentByInternQuery(internId));
            return Ok(enrolls);
        }

        /// <summary>
        /// Enrolls an intern in a track. This is a multi-step action orchestrated by the Mediator:
        ///   1. Validates intern & track
        ///   2. Checks capacity
        ///   3. Creates enrollment + payment (if paid track)
        ///   4. Commits atomically via UoW
        /// </summary>
        [HttpPost("[action]")]
        public async Task<ActionResult<EnrollmentViewModel>> Enroll(EnrollInternViewModel vm)
        {
            var enroll = await _mediator.Send(new CreateEnrollmentCommand(vm.InternId, vm.TrackId));
            return Ok(enroll);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EnrollmentViewModel>> Update(int id, UpdateEnrollmentRequest vm)
        {
            var update = await _mediator.Send(new UpdateEnrollmentCommand(id, vm.InternId, vm.TrackId));
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEnrollmentCommand(id));
            return Ok(result);
        }
    }
}
