using TaskFlow.Domain.Events;

namespace TaskFlow.Domain.Services;

public interface IDomainEventDispatcher
{
    Task DespacharAsync(IReadOnlyCollection<IDomainEvent> eventos, CancellationToken ct = default);
}
