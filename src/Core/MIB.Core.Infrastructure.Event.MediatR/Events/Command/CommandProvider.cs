using MediatR;

using MIB.Core.Domain.Commands;

namespace MIB.Core.Infrastructure.Events.MediatR.Events.Command
{
    public class CommandProvider(IMediator mediator) : ICommandProvider
    {
        public async Task SendAsync(ICommand message, CancellationToken cancellationToken = default)
        {
            await mediator.Send(message, cancellationToken);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1174:Remove redundant async/await", Justification = "Ci serve perché nel caso in cui la chiamata vada in errore, senza async/await, l'eccezione non verrebbe catturata")]
        public async Task<M> SendAsync<T, M>(T message, CancellationToken cancellationToken = default) where T : ICommand<M>
        {
            return await mediator.Send(message, cancellationToken);
        }
    }
}