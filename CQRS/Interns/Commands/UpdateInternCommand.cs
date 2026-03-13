using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands;

public record UpdateInternCommand (int id,string fullName,string email
    , int birthYear, string state, int trackId) : IRequest<bool>;

