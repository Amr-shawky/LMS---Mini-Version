using MediatR;

namespace LMS___Mini_Version.CQRS
{
    public interface ICommand<T> : IRequest<T>
    {
    }
}
