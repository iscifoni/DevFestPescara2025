using MediatR;

namespace MIB.Core.Domain.Commands
{
    public interface ICommand<out TResult> : IRequest<TResult>;
}