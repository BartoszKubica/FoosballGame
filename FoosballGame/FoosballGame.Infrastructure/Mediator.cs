using Microsoft.Extensions.DependencyInjection;

namespace FoosballGame.Infrastructure
{
    public interface IMediator
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
    }

    internal class Mediator : IMediator
    {
        private readonly IServiceProvider serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandType = command.GetType();
            var genericHandlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            var handler = serviceProvider.GetRequiredService(genericHandlerType);
            return ((ICommandHandler<TCommand>)handler).Handle(command);
        }

        public Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
        {
            var queryType = query.GetType();
            var responseType = typeof(TResponse);
            var genericHandlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, responseType);

            var handler = serviceProvider.GetRequiredService(genericHandlerType);
            return ((IQueryHandler<TQuery, TResponse>)handler).HandleAsync(query);
        }
    }
}
