using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Commands.CancelEnrollment;
using LMS___Mini_Version.Features.Enrollments.Commands.EnrollmentIntern;
using LMS___Mini_Version.Features.Enrollments.Queries.GetActiveEnrollmentByInternId;
using LMS___Mini_Version.Features.Enrollments.Queries.GetAllEnrollments;
using LMS___Mini_Version.Features.Enrollments.Queries.GetAllEnrollmentsByInternId;
using LMS___Mini_Version.Features.Enrollments.Queries.GetEnrollmentById;
using LMS___Mini_Version.Features.Interns.Queries.GetInternByIdQuery;
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
    /// //Seif Emam
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public EnrollmentController(
            IMediator mediator)
        {

            _mediatr = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediatr.Send(new GetAllEnrollmentsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediatr.Send(new GetEnrollmentByIdQuery(id)).ConfigureAwait(false);
            return Ok(result.ToViewModel());
        }

        [HttpGet("intern/{internId}")]
        public async Task<IActionResult> GetByIntern(int internId)
        {
            var result = await _mediatr.Send(new GetEnrollmentsByInternId(internId)).ConfigureAwait(false);
            return Ok(result);

        }

        [HttpGet("intern/{internId:int}/active")]
        public async Task<IActionResult> GetActiveByIntern(int internId)
        {
            var result = await _mediatr.Send(new GetActiveEnrollmentByInternIdQuery(internId)).ConfigureAwait(false);
            return Ok(result?.ToViewModel());
        }
        /// <summary>
        /// Enrolls an intern in a track. This is a multi-step action orchestrated by the Mediator:
        ///   1. Validates intern & track
        ///   2. Checks capacity
        ///   3. Creates enrollment + payment (if paid track)
        ///   4. Commits atomically via UoW
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentInternCommand command)
        {
            var result = await _mediatr.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new
                {
                    succeeded = false,
                    message = result.ErrorMessage
                });
            // return CreatedAtAction(nameof(GetById), new { id = result.Enrollment.Id },
            //     new { id = result.Enrollment.Id });
            return Ok(result.Enrollment.ToViewModel());
        }

        [HttpPatch("{id}/canecl")]
        public async Task<IActionResult> Canecl(int id)
        {
            var result = await _mediatr.Send(new CancelEnrollmentCommand(id)).ConfigureAwait(false);
            if (!result.IsSuccess)
                return BadRequest(new
                {
                    succeeded = false,
                    message = result.ErrorMessage
                });
            return Ok(new
            {
                succeeded = true,
                enrollment = result.Enrollment,
                payment = result.Payment
            });
        }
    
}
}
