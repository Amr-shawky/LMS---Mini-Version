using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands;

/// <summary>
/// why record cuz record imutable data type 
/// no one can change the data after creating it
/// </summary>
public record CreateInternCommand(string fullname,string email,
               int birthYear,string state,int trackId) : IRequest<InternDto>;
