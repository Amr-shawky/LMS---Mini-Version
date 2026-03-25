using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.CQRS
{
    public record RequestResult<TResult>(TResult Data, bool IsSuccess, ErrorCode ErrorCode) 
    {

    }
}
