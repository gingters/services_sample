using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
using Domain.Services;

namespace Article.Domain
{
	public class ArtikelGeloeschtEvent : Event
	{
	}

	public partial class Artikel
	{
		public void Loesche()
		{
			
			var evt = new ArtikelGeloeschtEvent()
			{
				TimeStamp = DateTime.UtcNow,
				Version = 1,
				Type = nameof(ArtikelGeloeschtEvent),

				AggregateId = ArtikelNummer,
			};

			ApplyEvent(evt);
			OnEventRaised(new EventRaisedEventArgs(evt));
		}

		private void Apply(ArtikelGeloeschtEvent evt)
		{
			ArtikelNummer = -1;
			Bezeichnung = null;
			Kategorien.Clear();
		}
	}
}
