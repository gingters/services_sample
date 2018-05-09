using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;

namespace Article.Domain
{
	public partial class Artikel : IAggregateRoot
	{
		private readonly IEventDispatcher _eventDispatcher;

		public int ArtikelNummer { get; private set; }
		public string Bezeichnung { get; private set; }
		public event EventHandler<EventRaisedEventArgs> EventRaised;

		public int Version { get; private set; }
		public HashSet<ArtikelKategorie> Kategorien { get; private set; } = new HashSet<ArtikelKategorie>();

		public bool IsDeleted => ArtikelNummer < 0;

		public Artikel()
		{
		}

		public Artikel(IEventDispatcher<Artikel> eventDispatcher, int artikelNummer)
		{
			ArtikelNummer = artikelNummer;
			_eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
		}

		public void ApplyEvent(IEvent evt)
		{
			_eventDispatcher.DispatchEvent(this, evt);
			Version++;
		}

		protected virtual void OnEventRaised(EventRaisedEventArgs e)
		{
			EventRaised?.Invoke(this, e);
		}

	}
}
