using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Common.Messaging;

public sealed class InMemoryLogEventBus : IEventBus
{
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        Log.Information("EVENT BUS (FAKE): {EventName} | Payload: {@EventPayload}",
            typeof(TEvent).Name,
            @event);

        return Task.CompletedTask;
    }
}
