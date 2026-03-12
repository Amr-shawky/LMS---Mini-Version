using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands.CreateInternCommand;

public record CreateInternCommand(string Name, int TrackId, string Email,string phone) : IRequest<InternDto>;