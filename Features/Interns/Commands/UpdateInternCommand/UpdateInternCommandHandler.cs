using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.UpdateInternCommand;

public class UpdateInternCommandHandler : IRequestHandler<UpdateInternCommand,int>
{
    private readonly IUnitOfWork _uow;
    public UpdateInternCommandHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<int> Handle(UpdateInternCommand request, CancellationToken cancellationToken)
    {
        // Check if the Email is already taken && if the intern exists
        var isinternExists = await _uow.Interns.GetByIdAsync(request.InternId).ConfigureAwait(false);
        if (isinternExists == null)
            throw new InvalidOperationException($"Intern with Id {request.InternId} doesn't exist");
        var isEmailTaken = await _uow.Interns.IsEmailAddressTakenAsync(request.Email).ConfigureAwait(false);
        if (isEmailTaken)
            throw new InvalidOperationException(
                $"Email {request.Email} is already used, Please use unique Email address!");
        var intern = new Intern()
        {
            FullName = request.Name,
            Email = request.Email,
            PhoneNumber = request.phone,
            TrackId = request.TrackId
        };
        _uow.Interns.Add(intern);
        await _uow.CompleteAsync().ConfigureAwait(false);
        return intern.Id;
    }
}