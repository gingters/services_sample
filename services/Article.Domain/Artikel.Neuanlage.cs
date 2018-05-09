using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
using Domain.Services;

namespace Article.Domain
{
	public class ArtikelAngelegtEvent : Event
	{
		// Todo: Events should be immutable. Set this via ctor
		public string Artikelbezeichnung { get; set; }
		public IEnumerable<string> Kategorien { get; set; } = new string[0];
	}

	public partial class Artikel
	{
		public void Neuanlage(string bezeichnung, IEnumerable<string> kategorien)
		{
			if (String.IsNullOrWhiteSpace(bezeichnung))
				throw new ArtikelException("Bezeichnung darf nicht leer sein.");

			var evt = new ArtikelAngelegtEvent()
			{
				TimeStamp = DateTime.UtcNow,
				Version = 1,
				Type = nameof(ArtikelAngelegtEvent),

				AggregateId = ArtikelNummer,
				Artikelbezeichnung = bezeichnung,
				Kategorien = kategorien ?? new string[0],
			};

			ApplyEvent(evt);
			OnEventRaised(new EventRaisedEventArgs(evt));
		}

		private void Apply(ArtikelAngelegtEvent evt)
		{
			Bezeichnung = evt.Artikelbezeichnung;

			foreach (var kategorie in evt.Kategorien)
			{
				Kategorien.Add(new ArtikelKategorie() { ArtikelNummer = ArtikelNummer, Name = kategorie, Artikel = this });
			}
		}
	}
}
