using Azure.Core;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.ApproveAndRegisterIntern.Commands;

public record ApproveAndRegisterInternCommand(int id, PaymentMethod PaymentMethod) :
    IRequest<RequestResponse<int>>;

