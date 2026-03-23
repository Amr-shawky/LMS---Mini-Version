using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Result
{
    public record ResultRequest<T>(T Data,bool IsSuccess,ErrorCode error)
    {
     public static ResultRequest<T> Success(T data) => new ResultRequest<T>(data, true,ErrorCode.NoError);
     public static ResultRequest<T> Failure(ErrorCode error) => new ResultRequest<T>(default(T)!, false, error);

     
    }
}
