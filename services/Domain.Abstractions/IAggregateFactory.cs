namespace Domain.Abstractions
{
	public interface IAggregateFactory
	{
		TAggregate CreateEntity<TAggregate>(int id)
			where TAggregate : class, IAggregateRoot;
	}
}
