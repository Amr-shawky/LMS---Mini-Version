using MediatR;

namespace LMS___Mini_Version.CQRS
{
    public interface IQuery<T> : IRequest<T>
    {
    }
}
