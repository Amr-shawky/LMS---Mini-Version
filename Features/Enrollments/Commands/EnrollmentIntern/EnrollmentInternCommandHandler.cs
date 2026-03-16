using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Services;
using LMS___Mini_Version.Features.Enrollments.Validators;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Mediators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands.EnrollmentIntern;

public class
    EnrollmentInternCommandHandler : IRequestHandler<EnrollmentIntern.EnrollmentInternCommand, EnrollmentResultDto>
{
    private readonly UnitOfWork _uow;
    private readonly EnrollmentValidator _validator;
    private readonly EnrollmentCreator _Creator;

    public EnrollmentInternCommandHandler(UnitOfWork unitofwork, EnrollmentValidator validator,
        EnrollmentCreator enrollmentCreator)
    {
        _Creator = enrollmentCreator;
        _validator = validator;
        _uow = unitofwork;
    }

    public async Task<EnrollmentResultDto> Handle(EnrollmentIntern.EnrollmentInternCommand request,
        CancellationToken cancellationToken)
    {
        // Validate and test all properties before start enrollment! 
        var error = await _validator.ValidateAsync(request).ConfigureAwait(false);
        if (error != null) throw new InvalidOperationException(error);
        // Start the transaction 
        var track = await _uow.Tracks.GetByIdAsync(request.TrackId).ConfigureAwait(false);
        return await _Creator.CreateAsync(request, track).ConfigureAwait(false);
    }
}