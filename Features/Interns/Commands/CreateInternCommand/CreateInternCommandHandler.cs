using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.validators;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.CreateInternCommand;

public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand,int>
{
    private readonly IUnitOfWork _uow;
    private readonly CreateInternValidators _validator;
    public CreateInternCommandHandler(IUnitOfWork uow , CreateInternValidators validator)
    {
        _uow = uow;
        _validator = validator;
    }
    public async Task<int> Handle(CreateInternCommand request, CancellationToken cancellationToken)
    {
        
       var error = await _validator.ValidateAsync(request).ConfigureAwait(false);
       if(error != null) throw new InvalidOperationException(error);

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