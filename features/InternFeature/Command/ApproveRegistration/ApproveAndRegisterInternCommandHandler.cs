using LMS___Mini_Version.features.Orchestrators;
using LMS___Mini_Version.features.shared;
using MediatR;


namespace LMS___Mini_Version.features.InternFeature.Command.ApproveRegistration;

public class ApproveAndRegisterInternCommandHandler(IMediator mediator)
    : IRequestHandler<ApproveAndRegisterInternCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(
        ApproveAndRegisterInternCommand request, CancellationToken cancellationToken)
        => await mediator.Send(
            new ApproveAndRegisterInternOrchestratorCommand(request.InternId, request.PaymentMethod),
            cancellationToken);
}