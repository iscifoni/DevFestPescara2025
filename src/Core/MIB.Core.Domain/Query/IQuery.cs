using MediatR;

namespace MIB.Core.Domain.Query
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}