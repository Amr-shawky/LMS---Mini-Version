using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.CreateInternCommand;

public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand,int>
{
    private readonly IUnitOfWork _uow;
    public CreateInternCommandHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<int> Handle(CreateInternCommand request, CancellationToken cancellationToken)
    {
        // First check if the Email is already taken
        var IsEmailTaken = await _uow.Interns.IsEmailAddressTakenAsync(request.Email).ConfigureAwait(false);
        if (IsEmailTaken)
            throw new InvalidOperationException(
                $"Email {request.Email} is already used, Please use unique Email address!");
        
        
        // Track exists check !
        var track = await _uow.Tracks.GetByIdAsync(request.TrackId).ConfigureAwait(false);
        if (track == null) throw new InvalidOperationException($"track with trackId {request.TrackId} Not found");

        var intern = new Intern()
        {
            FullName = request.Name,
            Email = request.Email,
            PhoneNumber = request.Phone,
            TrackId = request.TrackId
        };
        _uow.Interns.Add(intern);
        await _uow.CompleteAsync().ConfigureAwait(false);

        return intern.Id;
    }
}