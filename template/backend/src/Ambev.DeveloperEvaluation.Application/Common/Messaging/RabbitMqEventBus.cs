namespace Ambev.DeveloperEvaluation.Application.Common.Messaging;

public sealed class RabbitMqEventBus : IEventBus
{
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement RabbitMQ publish here (exchange, routing key, serialization, etc.)
        throw new NotImplementedException("RabbitMQ publisher not implemented yet.");
    }
}
