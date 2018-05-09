using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
	public class AggregateFactory : IAggregateFactory
	{
		private readonly ILogger<AggregateFactory> _logger;
		private readonly IServiceProvider _serviceProvider;

		public AggregateFactory(ILogger<AggregateFactory> logger, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public TAggregate CreateEntity<TAggregate>(int id)
			where TAggregate : class, IAggregateRoot
		{
			var entitytype = typeof(TAggregate);
			var ctor = GetEntityConstructor<TAggregate>(entitytype);

			try
			{
				var dispatcher = _serviceProvider.GetService<IEventDispatcher<TAggregate>>();

				return (TAggregate)ctor.Invoke(new object[] { dispatcher, id });
			}
			catch (Exception ex)
			{
				throw new DomainException($"An error occured while instanciating an Entity of type {entitytype.Name}", ex);
			}
		}

		private ConstructorInfo GetEntityConstructor<TAggregate>(Type entityType)
			where TAggregate : class, IAggregateRoot
		{
			var ctor = entityType.GetConstructor(new[]
			{
				// typeof(ILogger<TAggregate>),
				typeof(IEventDispatcher<TAggregate>),
				typeof(int)
			});

			if (ctor == null)
				throw new DomainException($"Default constructor of Entity Type {entityType.Name} not found");

			return ctor;
		}
	}
}
