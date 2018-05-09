using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Article.Domain;
using Domain.Abstractions;

namespace Article.Services
{
	public class ArtikelCommandHandler
	{
		private readonly IArtikelRepository _repo;

		public ArtikelCommandHandler(IArtikelRepository repo)
		{
			_repo = repo;
		}

		public void Handle(ArtikelNeuanlageCommand command)
		{
			var artikel = _repo.LadeArtikelMitKategorien(command.Artikelnummer);

			// pre-checks
			if (artikel != null)
				throw new InvalidOperationException("Artikel existiert schon");

			artikel = _repo.CreateNew(command.Artikelnummer);
			artikel.Neuanlage(command.Artikelbezeichnung, command.Kategorien);
		}
	}

	public interface IEventStore
	{
		void Store(IEvent evt);
		IList<IEvent> Get(int aggregateId, int from = 0);
		IEnumerable<IEvent> GetAllEvents();
	}

	public class EventStore : IEventStore
	{
		private List<IEvent> _events = new List<IEvent>();

		public EventStore()
		{
			// smuliere initialdaten
			Store(new ArtikelAngelegtEvent()
			{
				Type = nameof(ArtikelAngelegtEvent),
				TimeStamp = DateTime.UtcNow,
				Version = 1,

				AggregateId = 1701,
				Artikelbezeichnung = "Plüscheinhorn",
				Kategorien = new string[] { "klein", "mittel", "groß" }
			});
		}

		public void Store(IEvent evt)
		{
			evt.Id = _events.Any() ? _events.Max(e => e.Id) + 1 : 1;
			_events.Add(evt);
		}

		public IList<IEvent> Get(int aggregateId, int from)
		{
			return _events
				.Where(e => e.AggregateId == aggregateId && e.Id >= from)
				.OrderBy(e => e.Id)
				.ToList();
		}

		public IEnumerable<IEvent> GetAllEvents()
		{
			foreach (var evt in _events)
				yield return evt;
		}
	}
}
