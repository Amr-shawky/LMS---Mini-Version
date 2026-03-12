using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.UpdateInternCommand;

public record UpdateInternCommand(int InternId,string Name,string Email,string phone
,int TrackId) : IRequest<InternDto?>;
