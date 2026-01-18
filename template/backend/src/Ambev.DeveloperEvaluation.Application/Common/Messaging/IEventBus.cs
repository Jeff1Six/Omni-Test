namespace Ambev.DeveloperEvaluation.Application.Common.Messaging;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken);
}
